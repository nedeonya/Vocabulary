using Microsoft.AspNetCore.Mvc;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;

namespace Vocabulary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeaningController: Controller
{
    private readonly IWordMeaningRepository _service;
    
    public MeaningController(IWordMeaningRepository service)
    {
        _service = service;
    }

    [HttpGet("{wordName}")]
    [ProducesResponseType(200, Type = typeof(ICollection<IMeaning>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult GetMeaningsForWord(string wordName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 
        
        if (!_service.IsWordExist(wordName))
            return NotFound();
        
        var meanings = _service.GetMeaningsForWord(wordName);
        return Ok(meanings);

    }
    
    [HttpDelete("{meaningId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult DeleteMeaning(Guid meaningId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_service.IsMeaningExist(meaningId))
            return NotFound();
        
        if (!_service.DeleteMeaning(meaningId))
        {
            ModelState.AddModelError("Error", "Failed to delete the meaning");
            return StatusCode(500);
        }

        return NoContent();
    }
}