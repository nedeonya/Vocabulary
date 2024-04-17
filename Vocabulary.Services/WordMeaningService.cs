using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;

namespace Vocabulary.Services;

public class WordMeaningService: IWordMeaningService
{
    private readonly IUnitOfWork _unitOfWork;

    public WordMeaningService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public bool IsWordExist(string wordName)
    {
        return _unitOfWork.WordMeaningRepository.IsWordExist(wordName);
    }
    
    public bool IsWordExist(Guid wordId)
    {
        return _unitOfWork.WordMeaningRepository.IsWordExist(wordId);
    }
    
    public bool IsMeaningExist(Guid meaningId)
    {
        return _unitOfWork.WordMeaningRepository.IsMeaningExist(meaningId);
    }
    
    
    public IWord GetWord(Guid wordId)
    {
        return _unitOfWork.WordMeaningRepository.GetWord(wordId);
    }
    
    public ICollection<IWord> GetWords()
    {
        return _unitOfWork.WordMeaningRepository.GetWords();
    }
    
    public ICollection<IWord> GetWordsByName(string wordName)
    {
        return _unitOfWork.WordMeaningRepository.GetWords(wordName);
    }
    
    public IMeaning GetMeaning(Guid meaningId)
    {
        return _unitOfWork.WordMeaningRepository.GetMeaning(meaningId);
    }
    
    public ICollection<IMeaning> GetMeaningsForWord(string wordName)
    {
        return _unitOfWork.WordMeaningRepository.GetMeaningsForWord(wordName);
    }
    
    public bool EnsureAddWordWithMeaning(IWord word, IMeaning meaning)
    {
        bool result = false;
        if (IsWordExist(word.Name))
        {
            result = _unitOfWork.WordMeaningRepository.AddMeaning(meaning);
            if (result) _unitOfWork.Complete();
        }
        else
        {
            result = _unitOfWork.WordMeaningRepository.AddWord(word) && 
                     _unitOfWork.WordMeaningRepository.AddMeaning(meaning);
            if (result) _unitOfWork.Complete();
        }
        return result;
    }
    
    public bool UpdateWordWithMeaning(IWord word, IMeaning meaning)
    {
        var result = _unitOfWork.WordMeaningRepository.UpdateWord(word) &&
                     _unitOfWork.WordMeaningRepository.UpdateMeaning(meaning);
        if (result) _unitOfWork.Complete();
        return result;
    }

    public bool DeleteWordWithMeaning(Guid wordId, Guid meaningId)
    {
        var result = _unitOfWork.WordMeaningRepository.DeleteWord(wordId) &&
                     _unitOfWork.WordMeaningRepository.DeleteMeaning(meaningId);
        if (result) _unitOfWork.Complete();
        return result;
    }
    
    public bool DeleteWordWithRelatedMeanings(Guid wordId)
    {
        var meanings = _unitOfWork.WordMeaningRepository.GetMeaningsForWord(wordId);
        var result = _unitOfWork.WordMeaningRepository.DeleteWord(wordId);
        if (result)
        {
            if (meanings.Any(meaning => !_unitOfWork.WordMeaningRepository.DeleteMeaning(meaning.Id)))
            {
                result = false;
            }

            if (result) _unitOfWork.Complete();
        }
        return result;
    }
    
    public bool DeleteMeaning(Guid meaningId)
    {
        var result = _unitOfWork.WordMeaningRepository.DeleteMeaning(meaningId);
        if (result) _unitOfWork.Complete();
        return result;
    }
}