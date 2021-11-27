using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarApi.EntityConfiguratoin;

public class AppointmentStatusEntityConfiguration : IEntityTypeConfiguration<AppointmentStatus>
{
    public void Configure(EntityTypeBuilder<AppointmentStatus> builder)
    {
        builder.ToTable("AppointmentStatuses");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseIdentityColumn();
        builder.Property(a => a.Status).IsRequired().HasMaxLength(50);
        builder.Property(a => a.IsCompleted).IsRequired();
        builder.Property(a => a.IsRescheduled).IsRequired().HasDefaultValueSql("0");

        builder.HasData(new[]
        {
            new AppointmentStatus{Id=1, Status="Booked", IsCompleted=false, IsRescheduled=false},
            new AppointmentStatus{Id=2, Status="Completed", IsCompleted=true, IsRescheduled=false},
            new AppointmentStatus{Id=3, Status="Rescheduled", IsCompleted=false, IsRescheduled=true},
            new AppointmentStatus{Id=4, Status="Cancelled", IsCompleted=true, IsRescheduled=false},
        });
    }
}