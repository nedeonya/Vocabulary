using Vocabulary.Data;

namespace Vocabulary.Migrations;

public class DataContextProvider: IDataContextProvider
{
    public IDataContext Create()
    {
        var dataContext = new DataContext();
        dataContext.Database.EnsureCreated();
        return dataContext;
    }
}