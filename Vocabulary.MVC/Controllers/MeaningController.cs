using Microsoft.AspNetCore.Mvc;

namespace Vocabulary.MVC.Controllers;

public class MeaningController : Controller
{
    private readonly string _apiUri = "http://localhost:5031/api/meaning";
    private readonly IHttpClientFactory _clientFactory;

    public MeaningController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpPost]
    public IActionResult Delete(Guid meaningId)
    {
        
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri(_apiUri);
        
        HttpResponseMessage response = client.DeleteAsync($"{client.BaseAddress}/{meaningId}").Result;

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Word");
        }
        return RedirectToAction("Index", "Word");
    }
}