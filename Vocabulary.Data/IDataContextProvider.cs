

namespace Vocabulary.Data;

public interface IDataContextProvider
{
    IDataContext Create();
}