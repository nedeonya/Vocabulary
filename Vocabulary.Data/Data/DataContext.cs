using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vocabulary.Data.Entities;

namespace Vocabulary.Data.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Word> Words { get; set; }
    public DbSet<Meaning> Meanings { get; set; }

}
