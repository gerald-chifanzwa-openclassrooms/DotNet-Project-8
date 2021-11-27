using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consultants.Api.EntitityConfiguration;

public class ConsultantEntityConfiguration : IEntityTypeConfiguration<Consultant>
{
    public void Configure(EntityTypeBuilder<Consultant> builder)
    {
        builder.ToTable("Consultants");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).IsRequired().UseIdentityColumn();
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.ImageUrl).IsRequired().HasMaxLength(100);
        builder.Property(c => c.SpecialtyId).IsRequired();

        builder.HasOne(c => c.Specialty).WithMany().HasForeignKey(c => c.SpecialtyId);

        builder.HasData(new List<Consultant>
        {
            new () { Id = 1, FirstName = "Jessica", LastName = "Wally", SpecialtyId = 1, ImageUrl = "img/doctor1.jpg" },
            new () { Id = 2, FirstName = "Iai", LastName = "Donnas", SpecialtyId = 2, ImageUrl = "img/doctor2.jpg" },
            new () { Id = 3, FirstName = "Amanda", LastName = "Deny", SpecialtyId = 3, ImageUrl = "img/doctor3.jpg" },
            new () { Id = 4, FirstName = "Jason", LastName = "Davis", SpecialtyId = 1, ImageUrl = "img/doctor4.jpg" },
        });
    }
}