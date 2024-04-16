using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace apbd19_cwiczenia5.Models.DTOs;

public class AddAnimal
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}