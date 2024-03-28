using Vocabulary.Data.Data;

namespace Vocabulary.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private DataContext _dataContext;
    public IWordMeaningRepository WordMeaningRepository { get; }

    public UnitOfWork(DataContext dataContext, IWordMeaningRepository wordMeaningRepository)
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
