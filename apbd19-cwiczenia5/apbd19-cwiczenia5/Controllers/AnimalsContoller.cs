using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace apbd19_cwiczenia5.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AnimalsContoller : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsContoller(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpGet]
    public IActionResult GetAnimals()
    {
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        return Ok();
    }
}