using _7071Sprint1Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace _7071Sprint1Demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<OccupancyHistory> OccupancyHistories { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<RentHistory> RentHistories { get; set; }
        public DbSet<RentInvoice> RentInvoices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceBooking> ServiceBookings { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Asset Relationships
            modelBuilder.Entity<Asset>()
                .HasMany(a => a.OccupancyHistory)
                .WithOne(o => o.Asset)
                .HasForeignKey(o => o.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Asset>()
                .HasMany(a => a.RentHistory)
                .WithOne(rh => rh.Asset)
                .HasForeignKey(rh => rh.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Asset>()
                .HasMany(a => a.RentInvoices)
                .WithOne(ri => ri.Asset)
                .HasForeignKey(ri => ri.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Attendance Relationships
            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(at => at.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Shift)
                .WithMany(s => s.Attendances)
                .HasForeignKey(at => at.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client Relationships
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Bookings)
                .WithOne(sb => sb.Client)
                .HasForeignKey(sb => sb.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Client)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee Relationships
          
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.PayrollHistory)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Shifts)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.VacationRequests)
                .WithOne(vr => vr.Employee)
                .HasForeignKey(vr => vr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice Relationships
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Renter Relationships
            modelBuilder.Entity<Renter>()
                .HasMany(r => r.OccupancyHistories)
                .WithOne(o => o.Renter)
                .HasForeignKey(o => o.RenterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Renter>()
                .HasMany(r => r.RentHistories)
                .WithOne(rh => rh.Renter)
                .HasForeignKey(rh => rh.RenterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Renter>()
                .HasMany(r => r.RentInvoices)
                .WithOne(ri => ri.Renter)
                .HasForeignKey(ri => ri.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service Relationships
            modelBuilder.Entity<Service>()
                .HasMany(s => s.Bookings)
                .WithOne(sb => sb.Service)
                .HasForeignKey(sb => sb.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many: ServiceBooking <-> Employee
            modelBuilder.Entity<ServiceBooking>()
                .HasMany(sb => sb.AssignedEmployees)
                .WithMany(e => e.ServiceBookings)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeServiceBooking",
                    join => join
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join => join
                        .HasOne<ServiceBooking>()
                        .WithMany()
                        .HasForeignKey("ServiceBookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
            modelBuilder.Entity<ServiceBooking>()
    .Property(sb => sb.ScheduledDate)
    .HasColumnType("datetime2");

            // Shift Relationships
            modelBuilder.Entity<Shift>()
                .HasMany(s => s.Attendances)
                .WithOne(a => a.Shift)
                .HasForeignKey(a => a.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            // VacationRequest Relationships
            modelBuilder.Entity<VacationRequest>()
                .HasOne(vr => vr.Employee)
                .WithMany(e => e.VacationRequests)
                .HasForeignKey(vr => vr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceBooking>()
    .HasMany(s => s.AssignedEmployees)
    .WithMany(e => e.ServiceBookings);
        }
    }
}
