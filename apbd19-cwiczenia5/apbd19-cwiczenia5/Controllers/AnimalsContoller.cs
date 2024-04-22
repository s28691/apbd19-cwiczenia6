using apbd19_cwiczenia5.Models;
using apbd19_cwiczenia5.Models.DTOs;
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
    public IActionResult GetAnimals(string orderBy = "name")
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        string orderByColumn = "name";
        if (!string.IsNullOrEmpty(orderBy))
        {
            switch (orderBy.ToLower())
            {
                case "name":
                case "description":
                case "category":
                case "area":
                    orderByColumn = orderBy.ToLower();
                    break;
                default:
                    orderByColumn = "name";
                    break;
            }
        }
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal";
        var reader = command.ExecuteReader();
        List<Animal> animals = new List<Animal>();
        int AnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        while (reader.Read())
        {
           animals.Add(new Animal()
           {
               IdAnimal = reader.GetInt32(AnimalOrdinal),
               Name = reader.GetString(nameOrdinal)
           }); 
        }
        
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"INSERT INTO Animal VALUES(@animalName,'','','')";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        command.ExecuteNonQuery();
        
        return Created("", null);
    }
}