using Vocabulary.Data;

namespace Vocabulary.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private IDataContext _dataContext;
    public IWordMeaningRepository WordMeaningRepository { get; }

    public UnitOfWork(IDataContext dataContext, IWordMeaningRepository wordMeaningRepository)
    {
        _dataContext = dataContext;
        WordMeaningRepository = wordMeaningRepository;
    }

    public int Complete()
    {
        return _dataContext.SaveChanges();
    }

    public void Dispose()
    {
        _dataContext.Dispose();
    }
}
