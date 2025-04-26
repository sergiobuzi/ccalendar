using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ccalendar.Models;
using ccalendar.Services.Interfaces;
using ccalendar.Services;
using Microsoft.AspNetCore.Authorization;
using ccalendar.Models.ViewModels.Events;

namespace ccalendar.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IHomeServices _homeServices;

    public HomeController(IHomeServices homeServices)
    {
        _homeServices = homeServices;
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        JsonResult json = await _homeServices.GetEvents();
        return json;
    }

    [HttpPost]
    public async Task<bool> CreateEvent([FromBody] EventCreateDto model)
    {
        bool result = await _homeServices.CreateEvent(model);
        return result;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
