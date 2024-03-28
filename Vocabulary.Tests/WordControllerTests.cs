using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Vocabulary.Controllers;
using Vocabulary.Data.Entities;
using Vocabulary.Dto;
using Vocabulary.Services;

namespace Vocabulary_Tests;

[TestFixture]
public class WordControllerTests
{
    private IWordMeaningService _mockService;
    private WordController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _mockService = Substitute.For<IWordMeaningService>();
        _controller = new WordController(_mockService);
    }
    
    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public void GetWordByName_WhenWordExist_ReturnsWords()
    {
        var wordName = "word";
        var words = new List<IWord>()
        {
            new WordDto() { Name = "word1" },
            new WordDto() { Name = "word2" }
        };
        _mockService.GetWordsByName(wordName).Returns(words);
        
        var result = _controller.GetWordByName(wordName) as OkObjectResult;
        
        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(words);
    }
    
    [Test]
    public void GetWordByName_WhenModelStateIsNotValid_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");
        var result = _controller.GetWordByName("word");
        
        result.Should().BeEquivalentTo(new BadRequestObjectResult(_controller.ModelState));
    }
    
    [Test]
    public void CreateWordWithMeaning_WhenServiceSucceeds_ReturnsOk()
    {
        var wordWithMeaning = new WordWithMeaningDto()
        {
            Name = "word-name", 
            Description = "description", 
            Example = "example"
        };
        
        _mockService.EnsureAddWordWithMeaning(Arg.Any<WordDto>(), Arg.Any<MeaningDto>())
            .Returns(true);
        
        
        var result = _controller.CreateWordWithMeaning(wordWithMeaning) as OkResult;
        
        result.Should().BeEquivalentTo(new OkResult());
    }
    
    [Test]
    public void CreateWordWithMeaning_WhenModelStateIsNotValid_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");
        var result = _controller.CreateWordWithMeaning(new WordWithMeaningDto());
        
        result.Should().BeEquivalentTo(new BadRequestObjectResult(_controller.ModelState));
    }
    
    [Test]
    public void CreateWordWithMeaning_WhenServiceFails_ReturnsInternalServerError()
    {
        var wordWithMeaning = new WordWithMeaningDto()
        {
            Name = "word-name", 
            Description = "description", 
            Example = "example"
        };
        
        _mockService.EnsureAddWordWithMeaning(Arg.Any<WordDto>(), Arg.Any<MeaningDto>())
            .Returns(false);
        
        var result = _controller.CreateWordWithMeaning(wordWithMeaning) as ObjectResult;
        
        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new StatusCodeResult(500));
        _controller.ModelState.Should().ContainKey("Error");
        _controller.ModelState["Error"].Errors.Should()
            .Contain(e => e.ErrorMessage == "Failed to create the word");
    
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenWordAndMeaningExist_ReturnsNoContent()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        var wordWithMeaning = new WordWithMeaningDto()
        {
            Name = "word-name", 
            Description = "description", 
            Example = "example"
        };
        
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.IsMeaningExist(meaningId).Returns(true);
        _mockService.UpdateWordWithMeaning(Arg.Any<IWord>(), Arg.Any<IMeaning>()).Returns(true);
        
        var result = _controller.UpdateWordWithMeaning(wordId, meaningId, wordWithMeaning);
        result.Should().BeEquivalentTo(new NoContentResult());
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenWordDoesNotExist_ReturnsNotFound()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        var wordWithMeaning = new WordWithMeaningDto()
        {
            Name = "word-name", 
            Description = "description", 
            Example = "example"
        };
        
        _mockService.IsWordExist(wordId).Returns(false);
        
        var result = _controller.UpdateWordWithMeaning(wordId, meaningId, wordWithMeaning);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenMeaningDoesNotExist_ReturnsNotFound()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        var wordWithMeaning = new WordWithMeaningDto()
        {
            Name = "word-name", 
            Description = "description", 
            Example = "example"
        };
        
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.IsMeaningExist(meaningId).Returns(false);
        
        var result = _controller.UpdateWordWithMeaning(wordId, meaningId, wordWithMeaning);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }
    
    [Test]
    public void UpdateWordWithMeaning_WhenServiceFails_ReturnsInternalServerError()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        var wordWithMeaning = new WordWithMeaningDto()
        {
            Name = "word-name", 
            Description = "description", 
            Example = "example"
        };
        
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.IsMeaningExist(meaningId).Returns(true);
        _mockService.UpdateWordWithMeaning(Arg.Any<IWord>(), Arg.Any<IMeaning>()).Returns(false);
        
        var result = _controller.UpdateWordWithMeaning(wordId, meaningId, wordWithMeaning) as ObjectResult;
        
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(new StatusCodeResult(500));
        _controller.ModelState.Should().ContainKey("Error");
        _controller.ModelState["Error"].Errors.Should()
            .Contain(e => e.ErrorMessage == "Failed to update the word with meaning");
    }
    
    [Test]
    public void DeleteWordWithRelatedMeanings_WhenWordExist_ReturnsNoContent()
    {
        var wordId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.DeleteWordWithRelatedMeanings(wordId).Returns(true);
        
        var result = _controller.DeleteWordWithRelatedMeanings(wordId);
        result.Should().BeEquivalentTo(new NoContentResult());
    }
    
    [Test]
    public void DeleteWordWithRelatedMeanings_WhenWordDoesNotExist_ReturnsNotFound()
    {
        var wordId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(false);
        
        var result = _controller.DeleteWordWithRelatedMeanings(wordId);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }
    
    [Test]
    public void DeleteWordWithRelatedMeanings_WhenServiceFails_ReturnsInternalServerError()
    {
        var wordId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.DeleteWordWithRelatedMeanings(wordId).Returns(false);
        
        var result = _controller.DeleteWordWithRelatedMeanings(wordId) as ObjectResult;
        
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(new StatusCodeResult(500));
        _controller.ModelState.Should().ContainKey("Error");
        _controller.ModelState["Error"].Errors.Should()
            .Contain(e => e.ErrorMessage == "Failed to delete the word with meanings");
    }
    
    [Test]
    public void DeleteWordWithRelatedMeanings_WhenModelStateIsNotValid_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");
        var result = _controller.DeleteWordWithRelatedMeanings(Guid.NewGuid());
        
        result.Should().BeEquivalentTo(new BadRequestObjectResult(_controller.ModelState));
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenWordAndMeaningExist_ReturnsNoContent()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.IsMeaningExist(meaningId).Returns(true);
        _mockService.DeleteWordWithMeaning(wordId, meaningId).Returns(true);
        
        var result = _controller.DeleteWordWithMeaning(wordId, meaningId);
        result.Should().BeEquivalentTo(new NoContentResult());
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenWordDoesNotExist_ReturnsNotFound()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(false);
        
        var result = _controller.DeleteWordWithMeaning(wordId, meaningId);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenMeaningDoesNotExist_ReturnsNotFound()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.IsMeaningExist(meaningId).Returns(false);
        
        var result = _controller.DeleteWordWithMeaning(wordId, meaningId);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenServiceFails_ReturnsInternalServerError()
    {
        var wordId = Guid.NewGuid();
        var meaningId = Guid.NewGuid();
        _mockService.IsWordExist(wordId).Returns(true);
        _mockService.IsMeaningExist(meaningId).Returns(true);
        _mockService.DeleteWordWithMeaning(wordId, meaningId).Returns(false);
        
        var result = _controller.DeleteWordWithMeaning(wordId, meaningId) as ObjectResult;
        
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(new StatusCodeResult(500));
        _controller.ModelState.Should().ContainKey("Error");
        _controller.ModelState["Error"].Errors.Should()
            .Contain(e => e.ErrorMessage == "Failed to delete the word with meaning");
    }
    
    [Test]
    public void DeleteWordWithMeaning_WhenModelStateIsNotValid_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");
        var result = _controller.DeleteWordWithMeaning(Guid.NewGuid(), Guid.NewGuid());
        
        result.Should().BeEquivalentTo(new BadRequestObjectResult(_controller.ModelState));
    }
}