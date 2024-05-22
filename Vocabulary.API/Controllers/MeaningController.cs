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
    public IActionResult GetMeaningsForWord(string wordName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 
               
        var meanings = _service.GetMeaningsForWord(wordName);
        if (meanings.Count == 0)
            return NotFound();

        return Ok(meanings);

    }
    
    [HttpDelete("{meaningId}")]
    [ProducesResponseType(204)]
    public IActionResult DeleteMeaning(Guid meaningId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
               
        if (!_service.DeleteMeaning(meaningId))
        {
            ModelState.AddModelError("Error", "Failed to delete the meaning");
            return StatusCode(500);
        }

        return NoContent();
    }
}