using Microsoft.EntityFrameworkCore;
using Vocabulary.Data.Entities;

namespace Vocabulary.Data;

public interface IDataContext: IDisposable
{
    DbSet<Word> Words { get;  }
    DbSet<Meaning> Meanings { get;  }
    int SaveChanges();
    bool Save();
}