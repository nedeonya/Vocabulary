using Microsoft.AspNetCore.Mvc;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;
using Vocabulary.Dto;

namespace Vocabulary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController: Controller
{
    private IWordMeaningRepository _repository;
    
    public WordController(IWordMeaningRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<IWord>))]
    public IActionResult GetWords()
    {
        var words = _repository.GetWords();
        return Ok(words);
    }

    [HttpGet("{wordName}")]
    [ProducesResponseType(200, Type = typeof(ICollection<IWord>))]
    [ProducesResponseType(400)]
    public IActionResult GetWordsByName(string wordName)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var words = _repository.GetWords(wordName);
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
        
        var word  = new WordDto() { Name = wordWithMeaning.Name };
        var meaning = new MeaningDto(wordWithMeaning.Description, wordWithMeaning.Example, word.Id);

        if (!_repository.EnsureAddWordWithMeaning(word, meaning))
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
        
        if (!_repository.IsWordExist(wordId))
            return NotFound();
        
        if(!_repository.IsMeaningExist(meaningId))
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
        
        if (!_repository.UpdateWordWithMeaning(word, meaning))
        {
            ModelState.AddModelError("Error", "Failed to update the word with meaning");
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
        
        if (!_repository.IsWordExist(wordId))
            return NotFound();
        
        if (!_repository.IsMeaningExist(meaningId))
            return NotFound();
        
        if (!_repository.DeleteWordWithMeaning(wordId, meaningId))
        {
            ModelState.AddModelError("Error", "Failed to delete the word with meaning");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }
}