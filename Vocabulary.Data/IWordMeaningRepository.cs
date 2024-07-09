using Vocabulary.Data.Entities;

namespace Vocabulary.Data.Repository;

public interface IWordMeaningRepository
{
    bool IsWordExist(Guid wordId);
    bool IsMeaningExist(Guid meaningId);
    IWord GetWordWithMeaning(Guid wordId, Guid meaningId);
    ICollection<IWord> GetWords(string? wordName, string userId);
    ICollection<IMeaning> GetMeaningsForWord(Guid wordId);
    bool EnsureAddWordMeaning(IWord word, IMeaning meaning);
    bool UpdateWordMeaning(IWord word, IMeaning meaning);
    bool DeleteWord(Guid wordId);
    bool DeleteMeaning(Guid meaningId);

}