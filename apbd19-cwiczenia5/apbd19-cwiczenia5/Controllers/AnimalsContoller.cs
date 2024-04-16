using Microsoft.AspNetCore.Mvc;

namespace apbd19_cwiczenia5.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AnimalsContoller : ControllerBase
{
    [HttpGet]
    public IActionResult GetAnimals()
    {
        return Ok();
    }
}