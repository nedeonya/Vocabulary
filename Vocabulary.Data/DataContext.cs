using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabulary.Data.Entities;

namespace Vocabulary.Data;

public class DataContext: IdentityDbContext, IDataContext
{

    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Word> Words { get; init; }
    public DbSet<Meaning> Meanings { get; init; }

    public bool Save() => SaveChanges() > 0;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString =
                $@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Vocabulary;AttachDBFilename={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Vocabulary.mdf")}";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
