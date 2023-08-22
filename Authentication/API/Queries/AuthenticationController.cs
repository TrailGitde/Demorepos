using Microsoft.AspNetCore.Mvc;

namespace Framework.Authentication.API.Queries
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationQuery _authenticationQuery;

        public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration configuration, IAuthenticationQuery authenticationQuery)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._authenticationQuery = authenticationQuery;
        }

        [HttpGet, Route("Authorize")]
        public async Task<IActionResult> GenerateToken(string appId, string appKey)
        {
            try
            {
                TokenResult result = new TokenResult();
                var validateInput = await _authenticationQuery.Authorize(appId, appKey);
                Response responseData = new Response();
                if (validateInput.ExternalId != null)
                {
                    var tokenExpiryMinute = _configuration["Jwt:ValidTimeMin"];
                    DateTime TokenIssuedOn = DateTime.Now;
                    DateTime TokenExpires = DateTime.Now.AddMinutes(Convert.ToInt32(tokenExpiryMinute));
                    var token = _authenticationQuery.GenerateToken(validateInput, TokenIssuedOn, TokenExpires);
                    if (token != null)
                    {

                        result.access_token = token;
                        result.token_type = "Bearer";
                        result.expires_in = Convert.ToInt32(tokenExpiryMinute);
                        responseData.Status = "Success";
                        responseData.Message = "";
                        responseData.Data = result;
                    }
                }
                else
                {
                    responseData.Status = "Failed";
                    responseData.Message = "App ID or App Key Invalid";
                    responseData.Data = "";
                };
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new Response { Status = "Failed", Message = ex.Message.ToString(), Data = null });
            }
        }

        [Authorize]
        [HttpPost, Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshToken tokenInput)
        {
            try
            {
                TokenResult result = new TokenResult();
                string jwtToken = HttpContext.Request.Headers["Authorization"];
                Response responseData = new Response();
                if (jwtToken != null && jwtToken.StartsWith("Bearer"))
                {
                    jwtToken = jwtToken.Replace("Bearer ", "");
                    var validateInput = await _authenticationQuery.AuthorizeToken(jwtToken,tokenInput.appId, tokenInput.appKey);
                    
                    if (validateInput.AppId != null)
                    {
                        var tokenExpiryMinute = _configuration["Jwt:ValidTimeMin"];
                        DateTime TokenIssuedOn = DateTime.Now;
                        DateTime TokenExpires = DateTime.Now.AddMinutes(Convert.ToInt32(tokenExpiryMinute));
                        var token = _authenticationQuery.GenerateToken(validateInput, TokenIssuedOn, TokenExpires);
                        if (token != null)
                        {
                            result.access_token = token;
                            result.token_type = "Bearer";
                            result.expires_in = Convert.ToInt32(tokenExpiryMinute);
                            responseData.Status = "Success";
                            responseData.Message = "";
                            responseData.Data = result;
                        }
                    }
                    else
                    {
                        responseData.Status = "Exception";
                        responseData.Message = "Expired/ Invalid Token";
                        responseData.Data = "";
                    };
                }
                else
                {
                    responseData.Status = "Exception";
                    responseData.Message = "Invalid Token";
                    responseData.Data = "";
                }
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new Response { Status = "Failed", Message = ex.Message.ToString(), Data = null });
            }
        }
    }
}