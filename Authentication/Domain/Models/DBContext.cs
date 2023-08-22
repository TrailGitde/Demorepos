namespace FrameWork.Authentication.Domain.Models; 
public class AuthorizationDBContext : DbContext
{
    private readonly string _connectionString;
    private IDbConnection _connection;
    private string _conn;

    public DbSet<ProviderData> ProviderData { get; set; } 
    public DbSet<TokenMaster> TokenMaster { get; set; } 

    public AuthorizationDBContext(string connectionString) : base(GetOptions(connectionString))
    {
        //Authentication.Domain.Models.Crypto _crypto = new Authentication.Domain.Class.Crypto();
        //_conn = _crypto.Decrypt(connectionString);
        //_connectionString = _conn;
    }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProviderData>().HasKey("ExternalId");
    }
}