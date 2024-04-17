using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Vocabulary.Controllers;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;
using Vocabulary.Dto;

namespace Vocabulary_Tests;

[TestFixture]
public class MeaningControllerTests
{
    private IWordMeaningRepository _mockRepository;
    private MeaningController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = Substitute.For<IWordMeaningRepository>();
        _controller = new MeaningController(_mockRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }
    
    [Test]
    public void GetMeaningsForWord_WhenWordExist_ReturnsMeanings()
    {
        var wordName = "existing-word";
        var meanings = new List<IMeaning>()
        {
            new MeaningDto() { Description = "meaning1", Example = "example1" },
            new MeaningDto() { Description = "meaning2", Example = "example2" }
        };
        _mockRepository.IsWordExist(wordName).Returns(true);
        _mockRepository.GetMeaningsForWord(wordName).Returns(meanings);
        
        var result = _controller.GetMeaningsForWord(wordName) as OkObjectResult;
        
        using var scope = new AssertionScope();
        result.Value.Should().BeEquivalentTo(meanings);
    }
    
    [Test]
    public void GetMeaningsForWord_WhenWordNotExist_ReturnsNotFound()
    {
        var wordName = "non-existing-word";
        _mockRepository.IsWordExist(wordName).Returns(false);
        
        var result = _controller.GetMeaningsForWord(wordName);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }

    [Test]
    public void GetMeaningsForWord_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");
        
        var result = _controller.GetMeaningsForWord("word");
        result.Should().BeEquivalentTo(new BadRequestObjectResult(_controller.ModelState));
    }
    
    [Test]
    public void DeleteMeaning_WhenMeaningExist_ReturnsNoContent()
    {
        var meaningId = Guid.NewGuid();
        _mockRepository.IsMeaningExist(meaningId).Returns(true);
        _mockRepository.DeleteMeaning(meaningId).Returns(true);
        
        var result = _controller.DeleteMeaning(meaningId);
        result.Should().BeEquivalentTo(new NoContentResult());
    }
    
    [Test]
    public void DeleteMeaning_WhenMeaningNotExist_ReturnsNotFound()
    {
        var meaningId = Guid.NewGuid();
        _mockRepository.IsMeaningExist(meaningId).Returns(false);
        
        var result = _controller.DeleteMeaning(meaningId);
        result.Should().BeEquivalentTo(new NotFoundResult());
    }
    
    [Test]
    public void DeleteMeaning_WhenFailedToDelete_ReturnsInternalServerError()
    {
        var meaningId = Guid.NewGuid();
        _mockRepository.IsMeaningExist(meaningId).Returns(true);
        _mockRepository.DeleteMeaning(meaningId).Returns(false);
        
        var result = _controller.DeleteMeaning(meaningId);
        
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(new StatusCodeResult(500));
        _controller.ModelState.Should().ContainKey("Error");
        _controller.ModelState["Error"].Errors.Should().Contain(e => e.ErrorMessage == "Failed to delete the meaning");
    }
    
    [Test]
    public void DeleteMeaning_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");
        
        var result = _controller.DeleteMeaning(Guid.NewGuid());
        result.Should().BeEquivalentTo(new BadRequestObjectResult(_controller.ModelState));
    }
}