using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consultants.Api.EntitityConfiguration;

public class SpecialtyEntityConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.ToTable("Specialties");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Id).IsRequired().UseIdentityColumn();

        builder.HasData(new List<Specialty>
        {
            new () { Id = 1, Name = "Cardiologist" },
            new () { Id = 2, Name = "General Surgeon" },
            new () { Id = 3, Name = "Doctor" },
            new () { Id = 4, Name = "Cardiologist" },
        });
    }
}
