using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Framework.Authentication.Infrastructure.Repositories;

public class AuthenticationQuery : IAuthenticationQuery
{
    private readonly IConfiguration _configuration;

    public AuthenticationQuery(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    public async Task<ProviderData> Authorize(string appId, string appKey)
    {
        try
        {
            string sqlConnection = string.Empty;
            string connectionString = _configuration.GetConnectionString("Default");
            sqlConnection = GetConnectionString(connectionString);
            ProviderData result = new ProviderData();
            using (var _context = new AuthorizationDBContext(sqlConnection))
            {
                //var authorizeData = await _context.ProviderData.FromSqlInterpolated($"EXECUTE dbo.UNITE_V2_Authorization_AppId_SP").ToListAsync();
                //var data = authorizeData.Where(x => x.AppId == appId && x.AppKey == appKey);
                //if (data.Any())
                //{
                //    tokenInput = data.FirstOrDefault();
                //}
                //return tokenInput;

                SqlParameter[] parameters =
                {
                    new SqlParameter("@EAC_UID", SqlDbType.NVarChar,-1) { Value = appId },
                    new SqlParameter("@EAC_KEY", SqlDbType.NVarChar,-1) { Value = appKey }
                };

                var data = _context.ProviderData
                .FromSqlRaw("dbo.UNITE_V2_EXTERNALPROVIDER_VALIDATE_SP @EAC_UID, @EAC_KEY", parameters)
                .ToList();
                if (data.Count > 0)
                {
                    result = data.FirstOrDefault();
                }
                return result;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<ProviderData> AuthorizeToken(string tokenString, string appId, string appKey)
    {
        try
        {
            string sqlConnection = string.Empty;
            string connectionString = _configuration.GetConnectionString("Default");
            sqlConnection = GetConnectionString(connectionString);
            ProviderData result = new ProviderData();
            using (var _context = new AuthorizationDBContext(sqlConnection))
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TOK_AUTH_TOKEN", SqlDbType.NVarChar,-1) { Value = tokenString },
                    new SqlParameter("@EAC_UID", SqlDbType.NVarChar,-1) { Value = appId },
                    new SqlParameter("@EAC_KEY", SqlDbType.NVarChar,-1) { Value = appKey }
                };
                var data = _context.ProviderData
               .FromSqlRaw("dbo.UNITE_V2_EXTERNALAPIPROVIDER_VALIDATE_TOKEN_SP @TOK_AUTH_TOKEN,@EAC_UID,@EAC_KEY", parameters)
               .ToList();
                if (data != null && data.Count > 0)
                {
                    result = data.FirstOrDefault();
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string GenerateToken(ProviderData validateInput, DateTime TokenIssuedOn, DateTime TokenExpires)
    {
        try
        {
            string tokenString = string.Empty;
            var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Id",validateInput.ExternalId.ToString()),
                    new Claim("appId",validateInput.AppId),
                    new Claim("appKey",validateInput.AppKey),
                    new Claim("mappingId",validateInput.MappingId.ToString()),
                    new Claim("validTo",TokenExpires.ToString()),
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
               issuer: _configuration["JWT:ValidIssuer"],
               audience: _configuration["JWT:ValidAudience"],
               expires: TokenExpires,
               claims: authClaims,
               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );
            tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            bool isSaved = saveToken(tokenString, validateInput.ExternalId, TokenIssuedOn, TokenExpires);
            if (isSaved)
                return tokenString;
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool saveToken(string tokenString, long externalId, DateTime TokenIssuedOn, DateTime TokenExpires)
    {
        bool isSaved = false;
        string sqlConnection = string.Empty;
        string connectionString = _configuration.GetConnectionString("Default");
        sqlConnection = GetConnectionString(connectionString);
        ProviderData tokenInput = new ProviderData();
        using (var _context = new AuthorizationDBContext(sqlConnection))
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@USR_ID", SqlDbType.BigInt) { Value = externalId },
                new SqlParameter("@TOK_AUTH_TOKEN", SqlDbType.NVarChar,-1) { Value = tokenString },
                new SqlParameter("@TOK_ISSUED_ON", SqlDbType.DateTime) { Value = TokenIssuedOn },
                new SqlParameter("@TOK_EXPIRES_ON", SqlDbType.DateTime) { Value = TokenExpires },
            };
            var saveToken = _context.TokenMaster
           .FromSqlRaw("dbo.UNITE_V2_EXTERNALAPIPROVIDER_AUTHTOKEN_SAVE_SP @USR_ID, @TOK_AUTH_TOKEN,@TOK_ISSUED_ON,@TOK_EXPIRES_ON", parameters)
           .ToList();
            if (saveToken.Count > 0)
            {
                isSaved = true;
            }
            else
            {
                isSaved = false;
            }

        }
        return isSaved;
    }


    private string GetConnectionString(string connectionString)
    {
        try
        {
            string result = string.Empty;
            Crypto crypto = new Crypto();
            result = crypto.Decrypt(connectionString);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
