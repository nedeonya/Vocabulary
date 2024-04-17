using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Vocabulary.Data;

public interface IDataContextProvider
{
    IDataContext Create();
}