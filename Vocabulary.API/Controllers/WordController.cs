using Microsoft.AspNetCore.Mvc;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;
using Vocabulary.Dto;
using Vocabulary.Services;

namespace Vocabulary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController: Controller
{
    private IWordMeaningService _service;
    
    public WordController(IWordMeaningService service)
    {
        _service = service;
    }

    [HttpGet("{wordName}")]
    [ProducesResponseType(200, Type = typeof(ICollection<IWord>))]
    [ProducesResponseType(400)]
    public IActionResult GetWordByName(string wordName)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var words = _service.GetWordsByName(wordName);
        return Ok(words);

    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateWordWithMeaning([FromQuery] WordWithMeaningDto wordWithMeaning)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var word = new WordDto() { Name = wordWithMeaning.Name };
        var meaning = new MeaningDto()
        {
            Description = wordWithMeaning.Description,
            Example = wordWithMeaning.Example,
            WordId = word.Id
        };

        if (!_service.EnsureAddWordWithMeaning(word, meaning))
        {
            ModelState.AddModelError("Error", "Failed to create the word");
            return StatusCode(500, ModelState);
        }

        return Ok();
    }

    [HttpPut("{wordId}/{meaningId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateWordWithMeaning(Guid wordId, Guid meaningId, [FromBody] WordWithMeaningDto wordWithMeaning)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_service.IsWordExist(wordId))
            return NotFound();
        
        if(!_service.IsMeaningExist(meaningId))
            return NotFound();
        
        var word = new WordDto()
        {
            Id = wordId,
            Name = wordWithMeaning.Name
        };
        
        var meaning = new MeaningDto()
        {
            Id = meaningId,
            Description = wordWithMeaning.Description,
            Example = wordWithMeaning.Example,
            WordId = wordId
        };
        
        if (!_service.UpdateWordWithMeaning(word, meaning))
        {
            ModelState.AddModelError("Error", "Failed to update the word with meaning");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{wordId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult DeleteWordWithRelatedMeanings(Guid wordId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_service.IsWordExist(wordId))
            return NotFound();
        
        if (!_service.DeleteWordWithRelatedMeanings(wordId))
        {
            ModelState.AddModelError("Error", "Failed to delete the word with meanings");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }
    
    [HttpDelete("{wordId}/{meaningId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult DeleteWordWithMeaning(Guid wordId, Guid meaningId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_service.IsWordExist(wordId))
            return NotFound();
        
        if (!_service.IsMeaningExist(meaningId))
            return NotFound();
        
        if (!_service.DeleteWordWithMeaning(wordId, meaningId))
        {
            ModelState.AddModelError("Error", "Failed to delete the word with meaning");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }
}