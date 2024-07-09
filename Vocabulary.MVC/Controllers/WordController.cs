using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Vocabulary.MVC.Models;

namespace Vocabulary.MVC.Controllers;

public class WordController: Controller
{
    private readonly string _apiUri = "http://localhost:5031/api/word";
    private readonly IHttpClientFactory _clientFactory;
    public WordController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    private HttpClient CreateClient()
    {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri(_apiUri);
        var token = HttpContext.Session.GetString("jwt");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
    
    public IActionResult Index()
    {
        var wordsList = new List<WordViewModel>();
        
        var client = CreateClient();
        
        HttpResponseMessage response = client.GetAsync(client.BaseAddress.ToString()).Result;

        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            wordsList = JsonSerializer.Deserialize<List<WordViewModel>>(data, options);
            return View(wordsList);
        }
        else
        {
            ModelState.AddModelError("Error", response.ToString());
            return StatusCode((int)response.StatusCode, ModelState);
        }
    }
    
    public IActionResult Create(AddWordMeaningViewModel wordMeaning)
    {
        var client = CreateClient();
        if (ModelState.IsValid  && !string.IsNullOrEmpty(wordMeaning.Name) && !string.IsNullOrEmpty(wordMeaning.Description))
        {
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = new StringContent(JsonSerializer.Serialize(wordMeaning, options), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress.ToString(), content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
        }
        return View(wordMeaning);
    }
    
    [HttpPost]
    public IActionResult Delete(Guid wordId)
    {
        var client = CreateClient();
        
        HttpResponseMessage response = client.DeleteAsync($"{client.BaseAddress}/{wordId}").Result;

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    } 
    
    [HttpGet]
    public IActionResult Edit(Guid wordId, Guid meaningId)
    {
        var client = CreateClient();
        
        HttpResponseMessage response = client.GetAsync($"{client.BaseAddress}/{wordId}/{meaningId}").Result;

        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var wordMeaning = JsonSerializer.Deserialize<EditWordMeaningViewModel>(data, options);

            return View(wordMeaning);
        }
        else
        {
            return View("Error");
        }
    }
    
    [HttpPost]
    public IActionResult Edit(Guid wordId, Guid meaningId, EditWordMeaningViewModel wordMeaning)
    {
        var client = CreateClient();
        
        if (ModelState.IsValid && !string.IsNullOrEmpty(wordMeaning.Name) && !string.IsNullOrEmpty(wordMeaning.Description))
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var content = new StringContent(JsonSerializer.Serialize(wordMeaning, options), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync($"{client.BaseAddress}/{wordId}/{meaningId}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Error", "An error occurred while updating the word. Please try again.");
                return StatusCode(500, ModelState);
            }
        }
        return View(wordMeaning);
    }
}