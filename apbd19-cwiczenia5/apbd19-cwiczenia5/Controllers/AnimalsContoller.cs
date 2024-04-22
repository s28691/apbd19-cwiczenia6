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
        command.CommandText = "SELECT * FROM Animal ORDER BY {orderByColumn}";
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
    
    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, [FromBody] Animal updatedAnimal)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();
            
            SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Animal WHERE IdAnimal = @IdAnimal", connection);
            checkCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
            int animalCount = (int)checkCommand.ExecuteScalar();

            if (animalCount == 0)
            {
                return NotFound(); 
            }
            
            SqlCommand updateCommand = new SqlCommand("UPDATE Animal SET Name = @Name WHERE IdAnimal = @IdAnimal", connection);
            updateCommand.Parameters.AddWithValue("@Name", updatedAnimal.Name);
            updateCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
            updateCommand.ExecuteNonQuery();

            return NoContent(); 
        }
    }
    
    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();
            SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Animal WHERE IdAnimal = @IdAnimal", connection);
            checkCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
            int animalCount = (int)checkCommand.ExecuteScalar();
            if (animalCount == 0)
            {
                return NotFound(); 
            }
            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @IdAnimal", connection);
            deleteCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
            deleteCommand.ExecuteNonQuery();

            return NoContent(); 
        }
    }
}