namespace Framework.Authentication.Domain.Models;
public class ProviderData
{
    [Key]
    [Column("EAC_ID")]
    public long ExternalId { get; set; }
    
    [Column("EAC_MAPPING_ID")]
    public string MappingId { get; set; }
    [Column("EAC_UID")]
    public string AppId { get; set; }
    [Column("EAC_KEY")]
    public string AppKey { get; set; }
    [Column("EAC_IS_ACTIVE")]
    public bool IsActive { get; set; }
    [Column("ATR_CREATED_BY")]
    public long? CreatedBy { get; set; }
    [Column("ATR_MODIFIED_BY")]
    public long? ModifiedBy { get; set; }
    [Column("ATR_CREATED_DATETIME")]
    public DateTime? CreatedDateTime { get; set; }
    [Column("ATR_MODIFIED_DATETIME")]
    public DateTime? ModifiedDateTime { get; set; }
    [Column("CMN_TEXT1")]
    public string? CommonText1 { get; set; }
    [Column("CMN_TEXT2")]
    public string? CommonText2 { get; set; }
    [Column("CMN_VALUE1")]
    public float? CommonValue1 { get; set; }
    [Column("CMN_VALUE2")]
    public float? CommonValue2 { get; set; }
    [Column("CMN_DATETIME1")]
    public DateTime? CommonDateTime1 { get; set; }
    [Column("CMN_DATETIME2")]
    public DateTime? CommonDateTime2 { get; set; }
}

public class TokenMaster
{
    [Key]
    [Column("TOK_ID")] 
    public long TokenId { get; set; }
    [Column("USR_ID")] 
    public long MappingId { get; set; }
    [Column("TOK_AUTH_TOKEN")] 
    public string Token { get; set; }
    [Column("TOK_ISSUED_ON")] 
    public DateTime IssuesOn { get; set; }
    [Column("TOK_EXPIRES_ON")] 
    public DateTime ExpiredOn { get; set; }
    [Column("ATR_CREATED_BY")]
    public int? CreatedBy { get; set; }
    [Column("ATR_MODIFIED_BY")]
    public int? ModifiedBy { get; set; }
    [Column("ATR_CREATED_DATETIME")]
    public DateTime? CreatedDateTime { get; set; }
    [Column("ATR_MODIFIED_DATETIME")]
    public DateTime? ModifiedDateTime { get; set; }
    [Column("CMN_TEXT1")]
    public string? CommonText1 { get; set; }
    [Column("CMN_TEXT2")]
    public string? CommonText2 { get; set; }
    [Column("CMN_VALUE1")]
    public float? CommonValue1 { get; set; }
    [Column("CMN_VALUE2")]
    public float? CommonValue2 { get; set; }
    [Column("CMN_DATETIME1")]
    public DateTime? CommonDateTime1 { get; set; }
    [Column("CMN_DATETIME2")]
    public DateTime? CommonDateTime2 { get; set; }
    [Column("TOK_IS_ACTIVE")]
    public bool? IsActive { get; set; }
}

public class RefreshToken
{
    public string appId { get; set; }
    public string appKey { get; set; }
}
public class TokenResult
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
}