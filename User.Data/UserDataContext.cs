using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace User.Data;

public class UserDataContext: IdentityDbContext
{

    public UserDataContext()
    {
    }

    public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
    {
    }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString =
                $@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Identity;AttachDBFilename={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Identity.mdf")}";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
