using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vocabulary.Data.Entities;
using Vocabulary.Data.Repository;

namespace Vocabulary.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MeaningController: Controller
{
    private readonly IWordMeaningRepository _service;
    
    public MeaningController(IWordMeaningRepository service)
    {
        _service = service;
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