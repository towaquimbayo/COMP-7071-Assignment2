using Assignment2.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Server.Data
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

            // ---------------------------
            // Asset -> OccupancyHistory
            // ---------------------------
            modelBuilder.Entity<Asset>()
                .HasMany(a => a.OccupancyHistory)
                .WithOne(o => o.Asset)
                .HasForeignKey(o => o.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Asset -> RentHistory
            // ---------------------------
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

            // ---------------------------
            // Attendance -> Employee
            // ---------------------------
            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(at => at.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Attendance -> Shift
            // ---------------------------
            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Shift)
                .WithMany(s => s.Attendances)
                .HasForeignKey(at => at.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Client -> ServiceBooking
            // ---------------------------
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

            // ---------------------------
            // Employee -> Self-Reference (Manager)
            // ---------------------------
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany() // or you can define a property in Employee for a manager's subordinates
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Employee -> Payroll
            // ---------------------------
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.PayrollHistory)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Employee -> Shift
            // ---------------------------
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Shifts)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Employee -> Attendance
            // (already covered above, but clarifying both sides)
            // ---------------------------
            // modelBuilder.Entity<Employee>()
            //     .HasMany(e => e.Attendances)
            //     .WithOne(a => a.Employee)
            //     .HasForeignKey(a => a.EmployeeId);

            // ---------------------------
            // Employee -> VacationRequest
            // ---------------------------
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.VacationRequests)
                .WithOne(vr => vr.Employee)
                .HasForeignKey(vr => vr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Invoice -> Client
            // ---------------------------
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices) // or .WithMany() if you prefer no collection
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // OccupancyHistory -> Renter
            // ---------------------------
            modelBuilder.Entity<Renter>()
                .HasMany(r => r.OccupancyHistories)
                .WithOne(o => o.Renter)
                .HasForeignKey(o => o.RenterId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Renter -> RentHistory
            // ---------------------------
            modelBuilder.Entity<Renter>()
                .HasMany(r => r.RentHistories)
                .WithOne(rh => rh.Renter)
                .HasForeignKey(rh => rh.RenterId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Renter -> RentInvoice
            // ---------------------------
            modelBuilder.Entity<Renter>()
                .HasMany(r => r.RentInvoices)
                .WithOne(ri => ri.Renter)
                .HasForeignKey(ri => ri.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Service -> ServiceBooking
            // ---------------------------
            modelBuilder.Entity<Service>()
                .HasMany(s => s.Bookings)
                .WithOne(sb => sb.Service)
                .HasForeignKey(sb => sb.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // ServiceBooking -> Many-to-Many -> Employee
            // ---------------------------
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

            // ---------------------------
            // Shift -> Attendance
            // ---------------------------
            modelBuilder.Entity<Shift>()
                .HasMany(s => s.Attendances)
                .WithOne(a => a.Shift)
                .HasForeignKey(a => a.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // VacationRequest -> Employee
            // ---------------------------
            modelBuilder.Entity<VacationRequest>()
                .HasOne(vr => vr.Employee)
                .WithMany(e => e.VacationRequests)
                .HasForeignKey(vr => vr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}