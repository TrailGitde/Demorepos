using Microsoft.AspNetCore.Mvc;

namespace Framework.Configuration.API.Command;

public class AppointmentCommandController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
