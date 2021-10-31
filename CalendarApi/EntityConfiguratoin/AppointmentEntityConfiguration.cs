using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarApi.EntityConfiguratoin;

public class AppointmentEntityConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseIdentityColumn();
        builder.Property(a => a.ConsultantId).IsRequired();
        builder.Property(a => a.ConsultantName).IsRequired(false).HasMaxLength(100);
        builder.Property(a => a.PatientId).IsRequired();
        builder.Property(a => a.StartDate).IsRequired().HasColumnType("date");
        builder.Property(a => a.EndDate).IsRequired().HasColumnType("date");
        builder.Property(a => a.StatusId).IsRequired();
        builder.HasOne(a => a.Status).WithMany().HasForeignKey(a => a.StatusId);
    }
}
