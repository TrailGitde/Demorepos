using Microsoft.AspNetCore.Mvc;

namespace Framework.Configuration.API.Queries;

public class AppointmentQueryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
