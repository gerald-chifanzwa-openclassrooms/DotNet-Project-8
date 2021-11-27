using Consultants.Api;
using Consultants.Api.EntitityConfiguration;
using Microsoft.EntityFrameworkCore;

[EntityTypeConfiguration(typeof(ConsultantEntityConfiguration))]
public class Consultant
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }
}