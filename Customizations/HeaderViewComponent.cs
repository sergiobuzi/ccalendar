using ccalendar.Models;
using ccalendar.Models.ViewModels.Customers;
using ccalendar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize]
public class HeaderViewComponent : ViewComponent
{
    private readonly IHomeServices _homeServices;

    public HeaderViewComponent(IHomeServices homeServices)
    {
        _homeServices = homeServices;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        List<CustomerListViewModel> customers = await _homeServices.GetCustomersToRecall();
        ViewBag.GetCustomersToRecallNumber = customers.Count();
        return View("Default", customers);
    }
}