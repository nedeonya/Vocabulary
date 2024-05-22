using FluentAssertions;
using FluentAssertions.Execution;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;

namespace Vocabulary_Tests;

[TestFixture]
internal sealed class WordMeaningRepositoryTests
{
    [Test]
    public void IsWordExist_WhenWordExist_ReturnsTrue()
    {
        var word = new Word("word");
        var repository = CreateRepository();
        repository.AddWord(word);
        
        using var scope = new AssertionScope();
        repository.IsWordExist(word.Name).Should().BeTrue();
        repository.IsWordExist(word.Id).Should().BeTrue();
    }
    
    [Test]
    public void IsWordExist_WhenWordNotExist_ReturnFalse()
    {
        var word = new Word("word");
        
        var repository = CreateRepository();
        using var scope = new AssertionScope();
        repository.IsWordExist(word.Name).Should().BeFalse();
        repository.IsWordExist(word.Id).Should().BeFalse();
    }
    
    [Test]
    public void IsMeaningExist_WhenMeaningExist_ReturnsTrue()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var repository = CreateRepository();
        repository.AddMeaning(meaning);
        
        repository.IsMeaningExist(meaning.Id).Should().BeTrue();
    }
    
    [Test]
    public void IsMeaningExist_WhenMeaningNotExist_ReturnsFalse()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);

        var repository = CreateRepository();
        repository.IsMeaningExist(meaning.Id).Should().BeFalse();
    }
    
    [Test]
    public void GetWord_WhenWordExist_ReturnsWord()
    {
        var word = new Word("word");
        var repository = CreateRepository();
        repository.AddWord(word);
        
        repository.GetWord(word.Id).Should().BeEquivalentTo(word);
    }
    
    [Test]
    public void GetWord_WhenWordNotExist_ReturnsNull()
    {
        var word = new Word("word");
        
        var repository = CreateRepository();
        repository.GetWord(word.Id).Should().BeNull();
    }
    
    [Test]
    public void GetWords_WhenWordsExist_ReturnsWords()
    {
        var word1 = new Word("word-1");
        var word2 = new Word("word-2");
        var repository = CreateRepository();
        repository.AddWord(word1);
        repository.AddWord(word2);
        
        repository.GetWords().Should().BeEquivalentTo(new List<IWord> { word1, word2 });
    }
    
    [Test]
    public void GetWords_WhenWordsNotExist_ReturnsEmptyList()
    {
        var repository = CreateRepository();
        repository.GetWords().Should().BeEmpty();
    }
    
    [Test]
    public void GetWordsByName_WhenWordsExist_ReturnsWords()
    {
        var word1 = new Word("word-1");
        var word2 = new Word("word-2");
        var repository = CreateRepository();
        repository.AddWord(word1);
        repository.AddWord(word2);
        
        repository.GetWords("word").Should().BeEquivalentTo(new List<IWord> { word1, word2 });
    }
    
    [Test]
    public void GetWordsByName_WhenWordsNotExist_ReturnsEmptyList()
    {
        var repository = CreateRepository();
        repository.GetWords("non-existing-word").Should().BeEmpty();
    }
    
    [Test]
    public void GetMeaning_WhenMeaningExist_ReturnsMeaning()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var repository = CreateRepository();
        repository.AddWord(word);
        repository.AddMeaning(meaning);

        using var scope = new AssertionScope();
        repository.GetMeaning(meaning.Id).Description.Should().Be(meaning.Description);
        repository.GetMeaning(meaning.Id).Example.Should().Be(meaning.Example);
        repository.GetMeaning(meaning.Id).WordId.Should().Be(meaning.WordId);
    }

    [Test]
    public void GetMeaningsForWord_WhenMeaningsExist_ReturnsMeanings()
    {
        var word = new Word("word");
        var meaning1 = new Meaning("definition-1", "example-1", word.Id);
        var meaning2 = new Meaning("definition-2", "example-2", word.Id);
        var repository = CreateRepository();
        repository.AddWord(word);
        repository.AddMeaning(meaning1);
        repository.AddMeaning(meaning2);
        
        repository.GetMeaningsForWord(word.Name).Should()
            .Contain(new List<IMeaning> { meaning1, meaning2 });
    }
    
    [Test]
    public void GetMeaningsForWord_WhenMeaningsNotExist_ReturnsEmptyList()
    {
        var word = new Word("word");
        var repository = CreateRepository();
        repository.AddWord(word);
        
        repository.GetMeaningsForWord(word.Name).Should().BeEmpty();
    }
    
    [Test]
    public void EnsureAddWordWithMeaning_WhenWordExist_ReturnsTrueAndAddMeaningToWord()
    {
        var word = new Word("word");
        var repository = CreateRepository();
        repository.AddWord(word);
        var meaning = new Meaning("definition", "example", word.Id);
        
        using var scope = new AssertionScope();
        repository.EnsureAddWordMeaning(word, meaning).Should().BeTrue();
        repository.IsMeaningExist(meaning.Id).Should().BeTrue();
    }
    
    [Test]
    public void EnsureAddWordWithMeaning_WhenWordNotExist_ReturnsTrueAndAddWordAndMeaning()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var repository = CreateRepository();
        
        using var scope = new AssertionScope();
        repository.EnsureAddWordMeaning(word, meaning).Should().BeTrue();
        repository.IsMeaningExist(meaning.Id).Should().BeTrue();
        repository.IsWordExist(word.Id).Should().BeTrue();
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenWordAndMeaningExist_ReturnsTrueAndUpdatesWordAndMeaning()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var repository = CreateRepository();
        repository.AddWord(word);
        repository.AddMeaning(meaning);
        var updatedWord = new Word(word.Id, "updated-word", word.Meanings);
        var updatedMeaning = new Meaning(meaning.Id, "updated-definition", "updated-example", updatedWord.Id);
        
        using var scope = new AssertionScope();
        repository.UpdateWordMeaning(updatedWord, updatedMeaning).Should().BeTrue();
        repository.GetWord(word.Id).Name.Should().Be(updatedWord.Name);
        var actualMeaning = repository.GetMeaning(meaning.Id);
        actualMeaning.Description.Should().Be(updatedMeaning.Description);
        actualMeaning.Example.Should().Be(updatedMeaning.Example);
        actualMeaning.WordId.Should().Be(word.Id);
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenWordAndMeaningNotExist_ReturnsFalse()
    {
        var repository = CreateRepository();
        var updatedWord = new Word("updated-word");
        var updatedMeaning = new Meaning("updated-definition", "updated-example", updatedWord.Id);
        
        repository.UpdateWordMeaning(updatedWord, updatedMeaning).Should().BeFalse();
    }
    
    [Test]
    public void DeleteWord_WhenWordAndMeaningExist_ReturnsTrueAndDeleteWordAndMeaning()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var repository = CreateRepository();
        repository.AddWord(word);
        repository.AddMeaning(meaning);
        
        using var scope = new AssertionScope();
        repository.DeleteWord(word.Id).Should().BeTrue();
        repository.IsWordExist(word.Id).Should().BeFalse();
        repository.IsMeaningExist(meaning.Id).Should().BeFalse();
    }
    
    [Test]
    public void DeleteWord_WhenWordAndMeaningNotExist_ReturnsFalse()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        
        var repository = CreateRepository();
        repository.DeleteWord(word.Id).Should().BeFalse();
    }
    
    [Test]
    public void DeleteMeaning_WhenFewMeaningsExist_ReturnsTrueAndDeleteTargetMeaning()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var meaning2 = new Meaning("definition2", "example2", word.Id);
        var repository = CreateRepository();
        repository.AddWord(word);
        repository.AddMeaning(meaning);
        repository.AddMeaning(meaning2);
        
        using var scope = new AssertionScope();
        repository.DeleteMeaning(meaning.Id).Should().BeTrue();
        repository.IsMeaningExist(meaning.Id).Should().BeFalse();
        repository.IsMeaningExist(meaning2.Id).Should().BeTrue();
        repository.IsWordExist(word.Id).Should().BeTrue();
    }
    
    [Test]
    public void DeleteMeaning_WhenOneMeaningExist_ReturnsTrueAndDeleteMeaningAndWord()
    {
        var word = new Word("word");
        var meaning = new Meaning("definition", "example", word.Id);
        var repository = CreateRepository();
        repository.AddWord(word);
        repository.AddMeaning(meaning);
        
        using var scope = new AssertionScope();
        repository.DeleteMeaning(meaning.Id).Should().BeTrue();
        repository.IsMeaningExist(meaning.Id).Should().BeFalse();
        repository.IsWordExist(word.Id).Should().BeFalse();
    }
    
    private static WordMeaningRepository CreateRepository()
    {
        return new WordMeaningRepository(new InMemoryDataContextProvider());
    }
}