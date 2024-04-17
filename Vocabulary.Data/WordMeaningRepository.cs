
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Vocabulary.Data.Entities;

namespace Vocabulary.Data.Repository;

public class WordMeaningRepository : IWordMeaningRepository
{
    private readonly IDataContextProvider _contextProvider;

    public WordMeaningRepository(IDataContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public bool IsWordExist(string wordName)
    {
        using var context = _contextProvider.Create();
        return context.Words.Any(word => word.Name == wordName);
    }

    public bool IsWordExist(Guid wordId)
    {
        using var context = _contextProvider.Create();
        return context.Words.Any(word => word.Id == wordId);
    }

    public bool IsMeaningExist(Guid meaningId)
    {
        using var context = _contextProvider.Create();
        return context.Meanings.Any(m => m.Id == meaningId);
    }

    public IWord GetWord(Guid wordId)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Include(w=>w.Meanings)
            .FirstOrDefault(w=>w.Id == wordId);
    }  
    
    public IWord GetWord(string name)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Include(w=>w.Meanings)
            .FirstOrDefault(w=>w.Name == name);
    }
    
    public ICollection<IWord> GetWords()
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Include(w=>w.Meanings)
            .ToList<IWord>();
    }

    public ICollection<IWord> GetWords(string wordName)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Include(w=>w.Meanings)
            .Where(w => w.Name.Contains(wordName))
            .ToList<IWord>();
    }

    public IMeaning GetMeaning(Guid meaningId)
    {
        using var context = _contextProvider.Create();
        return context.Meanings.Find(meaningId);
    }

    public ICollection<IMeaning> GetMeaningsForWord(string wordName)
    {
        using var context = _contextProvider.Create();
        return context.Words.Where(w=>w.Name == wordName)
            .SelectMany(w=>w.Meanings)
            .ToList<IMeaning>();
    }

    public ICollection<IMeaning> GetMeaningsForWord(Guid wordId)
    {
        using var context = _contextProvider.Create();
        return context.Meanings
            .Where(m => m.WordId == wordId)
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
        
        using var context = _contextProvider.Create();
        context.Words.Add(addWord);

        return context.Save();
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
        
        using var context = _contextProvider.Create();
        context.Meanings.Add(addMeaning);
        return context.Save();
    }
    
    public bool EnsureAddWordWithMeaning(IWord word, IMeaning meaning)
    {
        var existingWord = GetWord(word.Name);
        if (existingWord != null)
        {
            var newMeaning = new Meaning(meaning.Id, meaning.Description, meaning.Example, existingWord.Id);
            return AddMeaning(newMeaning);
        }
        else
        {
            return AddWord(word) && AddMeaning(meaning);
        }
    }

    public bool UpdateWord(IWord word)
    {
        if (word == null)
        {
            return false;
        }
        using var context = _contextProvider.Create();
        var updateWord = context.Words.Find(word.Id);
        if (updateWord == null)
        {
            return false;
        }
        
        context.Words.Entry(updateWord).CurrentValues.SetValues(word);
        return context.Save();
    }

    public bool UpdateMeaning(IMeaning meaning)
    {
        if (meaning == null)
        {
            return false;
        }
        using var context = _contextProvider.Create();
        var updateMeaning = context.Meanings.Find(meaning.Id);
        if (updateMeaning == null)
        {
            return false;
        }
        
        context.Meanings.Entry(updateMeaning).CurrentValues.SetValues(meaning);
        context.Meanings.Entry(updateMeaning).Property(m=>m.WordId).IsModified = false;
        return context.Save();
    }
    
    public bool UpdateWordWithMeaning(IWord word, IMeaning meaning)
    {
        return UpdateWord(word) && UpdateMeaning(meaning);
    }

    public bool DeleteWord(Guid wordId)
    {
        using var context = _contextProvider.Create();
        var word = context.Words.Find(wordId);
        if (word == null)
        {
            return false;
        }

        context.Words.Remove(word);
        return context.Save();
    }

    public bool DeleteMeaning(Guid meaningId)
    {
        using var context = _contextProvider.Create();
        var meaning = context.Meanings.Find(meaningId);
        if (meaning == null)
        {
            return false;
        }

        context.Meanings.Remove(meaning);
        return context.Save();
    }

    public bool DeleteWordWithMeaning(Guid wordId, Guid meaningId)
    {
        return DeleteWord(wordId) && DeleteMeaning(meaningId);
    }
}