using Microsoft.EntityFrameworkCore;
using Vocabulary.Data;

namespace Vocabulary_Tests;

public class InMemoryDataContextProvider: IDataContextProvider
{
    private readonly string _databaseName = Guid.NewGuid().ToString();
    public IDataContext Create()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: _databaseName)
            .Options;
        return new DataContext(options);
    }
}