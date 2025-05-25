using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ccalendar.Models;
using ccalendar.Services.Interfaces;
using ccalendar.Services;
using Microsoft.AspNetCore.Authorization;
using ccalendar.Models.ViewModels.Events;
using ccalendar.Models.ViewModels.Customers;

namespace ccalendar.Controllers;

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
    public async Task<JsonResult> GetEvents()
    {
        JsonResult json = await _homeServices.GetEvents();
        return json;
    }

    [HttpGet]
    public async Task<IActionResult> Customers()
    {
        List<CustomerListViewModel> customers = await _homeServices.GetCustomers();
        return View(customers);
    }

    [HttpGet]
    public async Task<JsonResult> EventDetails([FromQuery] int id)
    {
        JsonResult json = await _homeServices.EventDetails(id);
        return json;
    }

    [HttpGet]
    [Route("Home/EditEvent/{eventId}")]
    public async Task<IActionResult> EditEvent(int eventId)
    {
        EventDetails model = await _homeServices.EditEvent(eventId);
        return View("EditEvent", model);
    }

    [HttpGet]
    public async Task<JsonResult> SearchCustomers([FromQuery]string query)
    {
        JsonResult json = await _homeServices.SearchCustomers(query);
        return json;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerById([FromQuery]int id)
    {
        JsonResult json = await _homeServices.GetCustomerById(id);
        return json;
    }

    [HttpPost]
    public async Task<int> CreateEvent([FromBody] EventCreateDto model)
    {
        int result = await _homeServices.CreateEvent(model);
        return result;
    }

    [HttpPost]
    public async Task<int> CreateCustomerAndEvent([FromBody] EventCreateDto model)
    {
        int result = await _homeServices.CreateCustomerAndEvent(model);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEvent(EventDetails dto)
    {
        EventDetails result = await _homeServices.UpdateEvent(dto);
        return View("EditEvent", result);
    }

    [HttpPost]
    public async Task<bool> DeleteEvent([FromQuery] int eventId)
    {
        bool result = await _homeServices.DeleteEvent(eventId);
        return result;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
