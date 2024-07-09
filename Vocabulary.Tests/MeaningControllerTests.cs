using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Vocabulary.API.Controllers;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;
using Vocabulary.API.Dto;

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
    public void DeleteMeaning_WhenMeaningExist_ReturnsNoContent()
    {
        var meaningId = Guid.NewGuid();
        _mockRepository.IsMeaningExist(meaningId).Returns(true);
        _mockRepository.DeleteMeaning(meaningId).Returns(true);
        
        var result = _controller.DeleteMeaning(meaningId);
        result.Should().BeEquivalentTo(new NoContentResult());
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