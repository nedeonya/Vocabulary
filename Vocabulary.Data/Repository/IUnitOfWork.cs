namespace Vocabulary.Data.Repository;

public interface IUnitOfWork: IDisposable
{
    IWordMeaningRepository WordMeaningRepository { get; }
    int Complete();
}