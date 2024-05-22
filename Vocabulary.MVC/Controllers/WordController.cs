using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Vocabulary.Data.Entities;
using Vocabulary.MVC.Models;

namespace Vocabulary.MVC.Controllers;

public class WordController: Controller
{
    private readonly string _apiUri = "http://localhost:5031/api/word";
    private readonly HttpClient _client;

    public WordController()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_apiUri);
    }
    public IActionResult Index()
    {
        var wordsList = new List<WordViewModel>();
        HttpResponseMessage response = _client.GetAsync(_client.BaseAddress.ToString()).Result;

        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            wordsList = JsonSerializer.Deserialize<List<WordViewModel>>(data, options);
        }
        return View(wordsList);
    }
    
    public IActionResult Create(AddWordMeaningViewModel wordMeaning)
    {
        if (ModelState.IsValid  && !string.IsNullOrEmpty(wordMeaning.Name) && !string.IsNullOrEmpty(wordMeaning.Description))
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var content = new StringContent(JsonSerializer.Serialize(wordMeaning, options), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress.ToString(), content).Result;

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
        HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/{wordId}").Result;

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    } 
    
    [HttpGet]
    public IActionResult Edit(Guid wordId, Guid meaningId)
    {
        HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/{wordId}/{meaningId}").Result;

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
        if (ModelState.IsValid && !string.IsNullOrEmpty(wordMeaning.Name) && !string.IsNullOrEmpty(wordMeaning.Description))
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var content = new StringContent(JsonSerializer.Serialize(wordMeaning, options), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/{wordId}/{meaningId}", content).Result;

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