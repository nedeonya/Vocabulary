using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;
using Vocabulary.API.Dto;

namespace Vocabulary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class WordController: Controller
{
    private readonly IWordMeaningRepository _repository;
    private string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public WordController(IWordMeaningRepository repository)
    {
        _repository = repository;
    }


    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<IWord>))]
    public IActionResult GetWords(string? wordName)
    {
        var words = _repository.GetWords(wordName, UserId);
        return Ok(words);
    }

    
    [HttpGet("{wordId}/{meaningId}")]
    [ProducesResponseType(200, Type = typeof(WordMeaningDto))]
    public IActionResult GetWordMeaning(Guid wordId, Guid meaningId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var word = _repository.GetWordWithMeaning(wordId, meaningId);
        var meaning = word.Meanings.First(m => m.Id == meaningId);
        var wordMeaning = new WordMeaningDto(word.Id, word.Name, meaning.Id, meaning.Description, meaning.Example);
        return Ok(wordMeaning);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public IActionResult CreateWordWithMeaning([FromBody] WordMeaningDto wordMeaning)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var word  = new WordDto() {Id = wordMeaning.wordId, Name = wordMeaning.Name, UserId = UserId};
        var meaning = new MeaningDto(wordMeaning.meaningId, wordMeaning.Description, wordMeaning.Example, wordMeaning.wordId);

        if (!_repository.EnsureAddWordMeaning(word, meaning))
        {
            ModelState.AddModelError("Error", "Failed to create the word");
            return StatusCode(500, ModelState);
        }

        return Ok();
    }

    [HttpPut("{wordId}/{meaningId}")]
    [ProducesResponseType(204)]
    public IActionResult UpdateWordWithMeaning(Guid wordId, Guid meaningId, [FromBody] WordMeaningDto wordMeaning)
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
            Name = wordMeaning.Name,
            UserId = UserId
        };
        
        var meaning = new MeaningDto()
        {
            Id = meaningId,
            Description = wordMeaning.Description,
            Example = wordMeaning.Example,
            WordId = wordId
        };
        
        if (!_repository.UpdateWordMeaning(word, meaning))
        {
            ModelState.AddModelError("Error", "Failed to update the word with meaning");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    
    [HttpDelete("{wordId}")]
    [ProducesResponseType(204)]
    public IActionResult DeleteWord(Guid wordId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_repository.IsWordExist(wordId))
            return NotFound();
        
        if (!_repository.DeleteWord(wordId))
        {
            ModelState.AddModelError("Error", "Failed to delete the word with meaning");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }
}