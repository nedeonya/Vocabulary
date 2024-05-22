using Microsoft.AspNetCore.Mvc;

namespace Vocabulary.MVC.Controllers;

public class MeaningController : Controller
{
    private readonly string _apiUri = "http://localhost:5031/api/meaning";
    private readonly HttpClient _client;

    public MeaningController()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_apiUri);
    }

    [HttpPost]
    public IActionResult Delete(Guid meaningId)
    {
        HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/{meaningId}").Result;

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Word");
        }
        return RedirectToAction("Index", "Word");
    }
}