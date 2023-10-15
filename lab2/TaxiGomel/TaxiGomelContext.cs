using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaxiGomel;

public partial class TaxiGomelContext : DbContext
{
    public TaxiGomelContext()
    {
    }

    public TaxiGomelContext(DbContextOptions<TaxiGomelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarModel> CarModels { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<View1> View1s { get; set; }

    public virtual DbSet<View2> View2s { get; set; }

    public virtual DbSet<View3> View3s { get; set; }

    public virtual DbSet<ViewCarsAndDriver> ViewCarsAndDrivers { get; set; }

    public virtual DbSet<ViewEmployeeAndPosition> ViewEmployeeAndPositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TRATSEVSKIY\\SQLEXPRESS01;Database=TaxiGomel;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Call>(entity =>
        {
            entity.HasKey(e => e.CallId).HasName("PK__Calls__5180CF8AEE207507");

            entity.Property(e => e.CallId).HasColumnName("CallID");
            entity.Property(e => e.CallTime).HasColumnType("smalldatetime");
            entity.Property(e => e.EndPosition).HasMaxLength(20);
            entity.Property(e => e.StartPosition).HasMaxLength(20);
            entity.Property(e => e.Telephone)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.Car).WithMany(p => p.Calls)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Calls_Cars");

            entity.HasOne(d => d.Dispatcher).WithMany(p => p.Calls)
                .HasForeignKey(d => d.DispatcherId)
                .HasConstraintName("FK_Calls_Employees");

            entity.HasOne(d => d.Rate).WithMany(p => p.Calls)
                .HasForeignKey(d => d.RateId)
                .HasConstraintName("FK_Calls_Rates");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Cars__68A0340E69AA6E02");

            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.CarcaseNumber).HasMaxLength(17);
            entity.Property(e => e.EngineNumber).HasMaxLength(17);
            entity.Property(e => e.LastTi)
                .HasColumnType("date")
                .HasColumnName("LastTI");
            entity.Property(e => e.RegistrationNumber).HasMaxLength(20);
            entity.Property(e => e.ReleaseYear).HasColumnType("date");
            entity.Property(e => e.SpecialMarks).HasMaxLength(50);

            entity.HasOne(d => d.Driver).WithMany(p => p.CarDrivers)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Cars_EmployeesD");

            entity.HasOne(d => d.Mechanic).WithMany(p => p.CarMechanics)
                .HasForeignKey(d => d.MechanicId)
                .HasConstraintName("FK_Cars_EmployeesM");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Cars_CarModels");
        });

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK__CarModel__E8D7A1CCB9A702FD");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.ModelName).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Specifications).HasMaxLength(50);
            entity.Property(e => e.TechStats).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF136E2FF56");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Employees_Positions");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A599819E018");

            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.PositionName).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Rates__58A7CCBCD3C7663C");

            entity.Property(e => e.RateId).HasColumnName("RateID");
            entity.Property(e => e.RateDescription).HasMaxLength(50);
            entity.Property(e => e.RatePrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<View1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_1");

            entity.Property(e => e.CarcaseNumber).HasMaxLength(17);
            entity.Property(e => e.ModelName).HasMaxLength(50);
        });

        modelBuilder.Entity<View2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_2");

            entity.Property(e => e.RegistrationNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<View3>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_3");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.PositionName).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewCarsAndDriver>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CarsAndDrivers");

            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.Driver).HasMaxLength(101);
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.RegistrationNumber).HasMaxLength(20);
            entity.Property(e => e.SpecialMarks).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewEmployeeAndPosition>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeAndPositions");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.PositionName).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
