using Vocabulary.Data.Entities;

namespace Vocabulary.Services;

public interface IWordMeaningService
{
    bool IsWordExist(string wordName);
    bool IsWordExist(Guid wordId);
    bool IsMeaningExist(Guid meaningId);
    IWord GetWord(string wordName);
    IWord GetWord(Guid wordId);
    ICollection<IWord> GetWordsByName(string wordName);
    IMeaning GetMeaning(Guid meaningId);
    ICollection<IMeaning> GetMeaningsForWord(string wordName);
    bool EnsureAddWordWithMeaning(IWord word, IMeaning meaning);
    bool UpdateWordWithMeaning(IWord word, IMeaning meaning);
    bool DeleteWordWithRelatedMeanings(Guid wordId);
    bool DeleteWordWithMeaning(Guid wordId, Guid meaningId);
    bool DeleteMeaning(Guid meaningId);
}