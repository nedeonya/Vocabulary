using Vocabulary.Data.Entities;

namespace Vocabulary.Data.Repository;

public interface IWordMeaningRepository
{
    bool IsWordExist(string wordName);
    bool IsWordExist(Guid wordId);
    bool IsMeaningExist(Guid meaningId);
    IWord GetWord(Guid wordId);
    IWord GetWord(string name);
    IMeaning GetMeaning(Guid meaningId);
    ICollection<IWord> GetWords();
    ICollection<IWord> GetWords(string wordName);
    ICollection<IMeaning> GetMeaningsForWord(string wordName);
    ICollection<IMeaning> GetMeaningsForWord(Guid wordId);
    bool AddMeaning(IMeaning meaning);
    bool AddWord(IWord word);
    bool EnsureAddWordWithMeaning(IWord word, IMeaning meaning);
    bool UpdateWord(IWord word);
    bool UpdateMeaning(IMeaning meaning);
    bool UpdateWordWithMeaning(IWord word, IMeaning meaning);
    bool DeleteWord(Guid wordId);
    bool DeleteMeaning(Guid meaningId);
    bool DeleteWordWithMeaning(Guid wordId, Guid meaningId);

}