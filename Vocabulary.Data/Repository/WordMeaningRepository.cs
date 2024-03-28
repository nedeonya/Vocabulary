using Vocabulary.Data.Data;
using Vocabulary.Data.Entities;

namespace Vocabulary.Data.Repository;

public class WordMeaningRepository: IWordMeaningRepository
{
    private DataContext _context;
    public WordMeaningRepository(DataContext context)
    {
        _context = context;
    }
    public bool IsWordExist(string wordName)
    {
        return _context.Words.Any(word => word.Name == wordName);
    }
    
    public bool IsWordExist(Guid wordId)
    {
        return _context.Words.Any(word => word.Id == wordId);
    }
    
    public bool IsMeaningExist(Guid meaningId)
    {
        return _context.Meanings.Any(m => m.Id == meaningId);
    }
    
    public IWord GetWord(string wordName)
    {
        return _context.Words.FirstOrDefault(word => word.Name == wordName);
    }
    
    public IWord GetWord(Guid wordId)
    {
        return _context.Words.Find(wordId);
    }

    public ICollection<IWord> GetWords(string wordName)
    {
        return _context.Words.Where(w => w.Name.Contains(wordName)).ToList<IWord>();
    }
    
    public IMeaning GetMeaning(Guid meaningId)
    {
        return _context.Meanings.Find(meaningId);
    }

    public ICollection<IMeaning> GetMeaningsForWord(string wordName)
    {
        return _context.Meanings
            .Where(m => m.Word.Name == wordName)
            .ToList<IMeaning>();
    }

    public ICollection<IMeaning> GetMeaningsForWord(Guid wordId)
    {
        return _context.Meanings
            .Where(m => m.Word.Id == wordId)
            .ToList<IMeaning>();
    }

    public bool AddWord(IWord word)
    {
        if (word == null)
        {
            return false;
        }

        var addWord = new Word()
        {
            Id = word.Id,
            Name = word.Name
        };
        
        try
        {
            _context.Add(addWord);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AddMeaning(IMeaning meaning)
    {
        if (meaning == null)
        {
            return false;
        }
        
        var addMeaning = new Meaning()
        {
            Id = meaning.Id,
            Description = meaning.Description,
            Example = meaning.Example,
            WordId = meaning.WordId
        };

        try
        {
            _context.Add(addMeaning);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public bool UpdateWord(IWord word)
    {
        if (word == null)
        {
            return false;
        }
        
        try
        {
            var updateWord = _context.Words.Find(word.Id);
            updateWord.Name = word.Name;
           _context.Update(updateWord);
           return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateMeaning(IMeaning meaning)
    {
        if (meaning == null)
        {
            return false;
        }

        try
        {
            var updateMeaning = _context.Meanings.Find(meaning.Id);
            updateMeaning.Description = meaning.Description;
            updateMeaning.Example = meaning.Example;
            _context.Update(updateMeaning);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public bool DeleteWord(Guid wordId)
    {
        var word = _context.Words.Find(wordId);
        if (word == null)
        {
            return false;
        }
        try
        {
            _context.Remove(word);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool DeleteMeaning(Guid meaningId)
    {
        var meaning = _context.Meanings.Find(meaningId);
        if (meaning == null)
        {
            return false;
        }
        try
        {
            _context.Remove(meaning);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
}