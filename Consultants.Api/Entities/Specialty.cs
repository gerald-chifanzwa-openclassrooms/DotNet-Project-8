using Consultants.Api.EntitityConfiguration;
using Microsoft.EntityFrameworkCore;

[EntityTypeConfiguration(typeof(SpecialtyEntityConfiguration))]
public class Specialty
{
    public int Id { get; set; }
    public string? Name { get; set; }
}