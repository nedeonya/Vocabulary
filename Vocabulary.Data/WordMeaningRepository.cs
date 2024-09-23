using Microsoft.EntityFrameworkCore;
using Vocabulary.Contracts;
using Vocabulary.Data.Entities;

namespace Vocabulary.Data.Repository;

public class WordMeaningRepository : IWordMeaningRepository
{
    private readonly IDataContextProvider _contextProvider;

    public WordMeaningRepository(IDataContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
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

    public IWord GetWordWithMeaning(Guid wordId, Guid meaningId)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Include(w => w.Meanings)
            .FirstOrDefault(w => w.Id == wordId && w.Meanings.Any(m => m.Id == meaningId));
    }

    public ICollection<IWord> GetWords(string? wordName, string userId)
    {
        if (string.IsNullOrEmpty(wordName))
        {
            return GetWords(userId);
        }

        using var context = _contextProvider.Create();
        return context.Words
            .Where(w => w.UserId == userId)
            .Include(w => w.Meanings)
            .Where(w => w.Name.Contains(wordName))
            .ToList<IWord>();

    }

    public ICollection<IMeaning> GetMeaningsForWord(Guid wordId)
    {
        using var context = _contextProvider.Create();
        return context.Meanings
            .Where(m => m.WordId == wordId)
            .ToList<IMeaning>();
    }

    public bool EnsureAddWordMeaning(IWord word, IMeaning meaning)
    {
        if (word == null || meaning == null)
        {
            return false;
        }

        var existingWord = GetWord(word.Name, word.UserId);
        if (existingWord != null)
        {
            var addMeaning = new Meaning()
            {
                Id = meaning.Id,
                Description = meaning.Description,
                Example = meaning.Example,
                WordId = existingWord.Id
            };
            return AddMeaning(addMeaning);
        }
        else
        {
            return AddWord(word) && AddMeaning(meaning);
        }
    }

    public bool UpdateWordMeaning(IWord word, IMeaning meaning)
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
        var allMeanings = GetMeaningsForWord(wordId).ToList();
        foreach (Meaning meaning in allMeanings)
        {
            context.Meanings.Remove(meaning);
        }
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

        var word = context.Words.Find(meaning.WordId);
        var allMeanings = GetMeaningsForWord(word.Id).ToList();
        if (allMeanings.Count == 1 && allMeanings[0].Id == meaningId)
        {
            context.Words.Remove(word);
        }

        context.Meanings.Remove(meaning);
        return context.Save();
    }

    internal IWord GetWord(Guid wordId)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Include(w => w.Meanings)
            .FirstOrDefault(w => w.Id == wordId);
    }

    internal bool AddWord(IWord word)
    {
        if (word == null)
        {
            return false;
        }

        var addWord = new Word()
        {
            Id = word.Id,
            Name = word.Name,
            UserId = word.UserId
        };

        using var context = _contextProvider.Create();
        context.Words.Add(addWord);

        return context.Save();
    }

    internal bool AddMeaning(IMeaning meaning)
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

    private bool UpdateWord(IWord word)
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
        if (updateWord.Equals(word))
        {
            return true;
        }
        context.Words.Entry(updateWord).CurrentValues.SetValues(word);
        context.Words.Entry(updateWord).Property(w => w.UserId).IsModified = false;
        return context.Save();
    }

    private bool UpdateMeaning(IMeaning meaning)
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
        if (updateMeaning.Equals(meaning))
        {
            return true;
        }
        context.Meanings.Entry(updateMeaning).CurrentValues.SetValues(meaning);
        context.Meanings.Entry(updateMeaning).Property(m => m.WordId).IsModified = false;
        return context.Save();
    }

    private IWord GetWord(string name, string userId)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Where(w => w.UserId == userId)
            .Include(w => w.Meanings)
            .FirstOrDefault(w => w.Name == name);
    }

    private ICollection<IWord> GetWords(string userId)
    {
        using var context = _contextProvider.Create();
        return context.Words
            .Where(w => w.UserId == userId)
            .Include(w => w.Meanings)
            .ToList<IWord>();
    }


}