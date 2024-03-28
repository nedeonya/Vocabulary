using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Vocabulary.Data.Data;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;
using Vocabulary.Services;

namespace Vocabulary_Tests;

[TestFixture]
public class WordMeaningServiceTests
{
    private WordMeaningService _service;
    private DataContext _context;
    
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new DataContext(options);
        var repository = new WordMeaningRepository(_context);
        var unitOfWork = new UnitOfWork(_context, repository);
        _service = new WordMeaningService(unitOfWork);
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    

    [Test]
    public void IsWordExist_WhenWordExist_ReturnsTrue()
    {
        var word = new Word() { Name = "existing-word" };
        _context.Add(word);
        _context.SaveChanges();
        
         using var scope = new AssertionScope();
         _service.IsWordExist(word.Name).Should().BeTrue();
         _service.IsWordExist(word.Id).Should().BeTrue();
    }

    [Test]
    public void IsWordExist_WhenWordNotExist_ReturnFalse()
    {
        var word = new Word() { Name = "non-existing-word" };
        
        using var scope = new AssertionScope();
        _service.IsWordExist(word.Name).Should().BeFalse();
        _service.IsWordExist(word.Id).Should().BeFalse();
    }
    
    [Test]
    public void IsMeaningExist_WhenMeaningExist_ReturnsTrue()
    {
        
        var meaning = new Meaning()
        {
            Word = new Word() { Name = "word-with-meaning" },
            Description = "definition",
            Example = "example"
        };
        _context.Add(meaning);
        _context.SaveChanges();
        
        _service.IsMeaningExist(meaning.Id).Should().BeTrue();
    }
    
    [Test]
    public void IsMeaningExist_WhenMeaningNotExist_ReturnsFalse()
    {
        var meaning = new Meaning()
        {
            Word = new Word() { Name = "non-existing-word" },
            Description = "non-existing-definition",
            Example = "non-existing-example"
        };
        
        _service.IsMeaningExist(meaning.Id).Should().BeFalse();
    }
    
    [Test]
    public void GetWord_WhenWordExist_ReturnsWord()
    {
        var word = new Word() { Name = "existing-word-named" };
        _context.Add(word);
        _context.SaveChanges();
        
        _service.GetWord(word.Name).Should().NotBeNull();
    }
    
    [Test]
    public void GetWord_WhenWordNotExist_ReturnsNull()
    {
        var word = new Word() { Name = "non-existing-word-name" };
        
        _service.GetWord(word.Name).Should().BeNull();
    }
    
    [Test]
    public void GetWordsByName_WhenWordsExist_ReturnsWords()
    {
        var word1 = new Word() { Name = "test-word-name-1" };
        var word2 = new Word() { Name = "test-word-name-2" };
        _context.AddRange(word1, word2);
        _context.SaveChanges();
        
        _service.GetWordsByName("test-word-name").Should()
            .BeEquivalentTo(new List<IWord> { word1, word2 });
    }
    
    [Test]
    public void GetWordsByName_WhenWordsNotExist_ReturnsEmptyList()
    {
      _service.GetWordsByName("non-existing-test-word-name").Should().BeEmpty();
    }
    
    [Test]
    public void GetMeaning_WhenMeaningExist_ReturnsMeaning()
    {
        var meaning = new Meaning()
        {
            Word = new Word() { Name = "get-meaning-word-with-meaning" },
            Description = "get-meaning-definition",
            Example = "get-meaning--example"
        };
        _context.Add(meaning);
        _context.SaveChanges();
        
        _service.GetMeaning(meaning.Id).Should().NotBeNull();
    }

    [Test]
    public void GetMeaningsForWord_WhenMeaningsExist_ReturnsMeanings()
    {
        var word = new Word() { Name = "word-with-meanings" };
        var meaning1 = new Meaning()
        {
            Word = word,
            Description = "test-definition-1",
            Example = "test-example-1"
        };
        var meaning2 = new Meaning()
        {
            Word = word,
            Description = "test-definition-2",
            Example = "test-example-2"
        };
        _context.AddRange(word, meaning1, meaning2);
        _context.SaveChanges();
        
        _service.GetMeaningsForWord(word.Name).Should()
            .Contain(new List<IMeaning> { meaning1, meaning2 });
    }
    
    [Test]
    public void GetMeaningsForWord_WhenMeaningsNotExist_ReturnsEmptyList()
    {
        var word = new Word() { Name = "word-without-meaning" };
        _context.Add(word);
        _context.SaveChanges();

        _service.GetMeaningsForWord(word.Name).Should().BeEmpty();
    }
    
    [Test]
    public void EnsureAddWordWithMeaning_WhenWordExist_ReturnsTrueAndAddMeaningToWord()
    {
        var word = new Word() { Name = "add-existing-word" };
        _context.Add(word);
        _context.SaveChanges();
        
        var meaning = new Meaning()
        {
            Word = word,
            Description = "test-definition",
            Example = "test-example"
        };
        
        using var scope = new AssertionScope();
        _service.EnsureAddWordWithMeaning(word, meaning).Should().BeTrue();
        _service.IsMeaningExist(meaning.Id).Should().BeTrue();
    }
    
    [Test]
    public void EnsureAddWordWithMeaning_WhenWordNotExist_ReturnsTrueAndAddWordAndMeaning()
    {
        var word = new Word() { Name = "add-non-existing-word" };
        var meaning = new Meaning()
        {
            Word = word,
            Description = "test-definition",
            Example = "test-example"
        };
        
        using var scope = new AssertionScope();
        _service.EnsureAddWordWithMeaning(word, meaning).Should().BeTrue();
        _service.IsMeaningExist(meaning.Id).Should().BeTrue();
        _service.IsWordExist(word.Id).Should().BeTrue();
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenWordAndMeaningExist_ReturnsTrueAndUpdatesWordAndMeaning()
    {
        var word = new Word() { Name = "word-for-update" };
        var meaning = new Meaning()
        {
            Word = word,
            Description = "definition-for-update",
            Example = "example-for-update"
        };
        _context.AddRange(word, meaning);
        _context.SaveChanges();
        
        var updatedWord = new Word() { Id = word.Id, Name = "updated-word" };
        var updatedMeaning = new Meaning()
        {
            Id = meaning.Id,
            Description = "updated-definition",
            Example = "updated-example"
        };
        
        using var scope = new AssertionScope();
        _service.UpdateWordWithMeaning(updatedWord, updatedMeaning).Should().BeTrue();
        _service.GetWord(word.Id).Name.Should().Be(updatedWord.Name);
        var actualMeaning = _service.GetMeaning(meaning.Id);
        actualMeaning.Description.Should().Be(updatedMeaning.Description);
        actualMeaning.Example.Should().Be(updatedMeaning.Example);
        actualMeaning.WordId.Should().Be(word.Id);
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenWordAndMeaningExist_ReturnsTrueAndDeleteWordAndMeaning()
    {
        var word = new Word() { Name = "word-for-delete" };
        var meaning = new Meaning()
        {
            Word = word,
            Description = "definition-for-delete",
            Example = "example-for-delete"
        };
        _context.AddRange(word, meaning);
        _context.SaveChanges();
        
        using var scope = new AssertionScope();
        _service.DeleteWordWithMeaning(word.Id, meaning.Id).Should().BeTrue();
        _service.IsWordExist(word.Id).Should().BeFalse();
        _service.IsMeaningExist(meaning.Id).Should().BeFalse();
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenWordAndMeaningNotExist_ReturnsFalse()
    {
        var word = new Word() { Name = "non-existing-word-for-delete" };
        var meaning = new Meaning()
        {
            Word = word,
            Description = "definition-for-delete",
            Example = "example-for-delete"
        };
        
        _service.DeleteWordWithMeaning(word.Id, meaning.Id).Should().BeFalse();
    }
    
    [Test]
    public void DeleteMeaning_WhenMeaningExist_ReturnsTrueAndDeleteMeaning()
    {
        var meaning = new Meaning()
        {
            Word = new Word() { Name = "word-for-delete-meaning" },
            Description = "definition-for-delete-meaning",
            Example = "example-for-delete-meaning"
        };
        _context.Add(meaning);
        _context.SaveChanges();
        
        using var scope = new AssertionScope();
        _service.DeleteMeaning(meaning.Id).Should().BeTrue();
        _service.IsMeaningExist(meaning.Id).Should().BeFalse();
        _service.IsWordExist(meaning.WordId).Should().BeTrue();
    }
    
    [Test]
    public void DeleteMeaning_WhenMeaningNotExist_ReturnsFalse()
    {
        var meaning = new Meaning()
        {
            Word = new Word() { Name = "non-existing-word-for-delete-meaning" },
            Description = "non-existing-definition-for-delete-meaning",
            Example = "non-existing-example-for-delete-meaning"
        };
        
        _service.DeleteMeaning(meaning.Id).Should().BeFalse();
    }
    
}