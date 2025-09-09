using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using Moobile_Platform.CampaignManagement.Domain.Model.Aggregates;
using Moobile_Platform.CampaignManagement.Domain.Model.ValueObjects;
using Moobile_Platform.IAM.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.Aggregates;
using Moobile_Platform.RanchManagement.Domain.Model.ValueObjects;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Moobile_Platform.StaffAdministration.Domain.Model.Aggregates;
using Moobile_Platform.StaffAdministration.Domain.Model.ValueObjects;
using Moobile_Platform.VoiceCommand.Domain.Model.Aggregates;

namespace Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        /* IAM BC  */
        //User
        builder.Entity<User>().HasKey(f => f.Id);
        builder.Entity<User>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(f => f.Username).IsRequired();
        builder.Entity<User>().Property(f => f.Password).IsRequired();
        builder.Entity<User>().Property(f => f.Email).IsRequired();
        builder.Entity<User>().Property(f => f.EmailConfirmed).IsRequired();
        //Admin User
        builder.Entity<Admin>().HasKey(f => f.Id);
        builder.Entity<Admin>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Admin>().Property(f => f.Email).IsRequired();
        builder.Entity<Admin>().Property(f => f.EmailConfirmed).IsRequired();
        
        /* ---------------------------------------------------------------------------------------------------------- * /

        /* Ranch BC -------------------------------------------------------------------------------------- */
        //Bovine
        builder.Entity<Bovine>().HasKey(f => f.Id);
        builder.Entity<Bovine>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Bovine>().Property(f => f.Name).IsRequired();
        builder.Entity<Bovine>().Property(f => f.Gender).IsRequired();
        builder.Entity<Bovine>().Property(f => f.BirthDate).IsRequired();
        builder.Entity<Bovine>().Property(f => f.Breed).IsRequired();
        builder.Entity<Bovine>().Property(f => f.Location).IsRequired();
        builder.Entity<Bovine>().Property(f => f.BovineImg).IsRequired();
        builder.Entity<Bovine>().Property(f => f.StableId).IsRequired();
        builder.Entity<Bovine>()
            .Property(f => f.RanchUserId)
            .HasConversion(
                v => v.UserIdentifier,    
                v => new RanchUserId(v))      
            .HasColumnName("user_id")
            .IsRequired();
        
        //Vaccine
        builder.Entity<Vaccine>().HasKey(f => f.Id);
        builder.Entity<Vaccine>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Vaccine>().Property(f => f.Name).IsRequired();
        builder.Entity<Vaccine>().Property(f => f.VaccineType).IsRequired();
        builder.Entity<Vaccine>().Property(f => f.VaccineDate).IsRequired();
        builder.Entity<Vaccine>().Property(f => f.VaccineImg).IsRequired();
        builder.Entity<Vaccine>().Property(f => f.BovineId).IsRequired();
        builder.Entity<Vaccine>()
            .Property(f => f.RanchUserId)
            .HasConversion(
                v => v.UserIdentifier,   
                v => new RanchUserId(v))    
            .HasColumnName("user_id")
            .IsRequired();
        
        //Stable
        builder.Entity<Stable>().HasKey(f => f.Id);
        builder.Entity<Stable>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Stable>().Property(f => f.Limit).IsRequired();
        builder.Entity<Stable>()
            .Property(f => f.RanchUserId)
            .HasConversion(
                v => v.UserIdentifier,    
                v => new RanchUserId(v))        
            .HasColumnName("user_id")
            .IsRequired();
        

        /* ---------------------------------------------------------------------------------------------------------- */
        
        /* Staff Administration BC -------------------------------------------------------------------------------------- */
        //Staff
        builder.Entity<Staff>().HasKey(f => f.Id);
        builder.Entity<Staff>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Staff>().Property(f => f.Name).IsRequired();
        builder.Entity<Staff>()
            .OwnsOne(f => f.EmployeeStatus, navigationBuilder =>
            {
                navigationBuilder.WithOwner().HasForeignKey("Id");
                navigationBuilder.Property(f => f.Value)
                    .IsRequired()
                    .HasColumnName("employee_status");
            });
        builder.Entity<Staff>().Property(f => f.CampaignId).IsRequired();
        builder.Entity<Staff>()
            .Property(f => f.StaffUserId)
            .HasConversion(
                v => v.UserIdentifier,     
                v => new StaffUserId(v))        
            .HasColumnName("user_id")
            .IsRequired();

        /* ---------------------------------------------------------------------------------------------------------- */
        
        /* Campaign Management BC -------------------------------------------------------------------------------------- */
        //Campaign
        builder.Entity<Campaign>().HasKey(c => c.Id);
        builder.Entity<Campaign>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Campaign>().Property(c => c.Name).IsRequired();
        builder.Entity<Campaign>().Property(c => c.Description).IsRequired();
        builder.Entity<Campaign>().Property(c => c.StartDate).IsRequired();
        builder.Entity<Campaign>().Property(c => c.EndDate).IsRequired();
        builder.Entity<Campaign>().Property(c => c.Status).IsRequired();
        builder.Entity<Campaign>().Property(f => f.StableId).IsRequired();
        builder.Entity<Campaign>()
            .Property(f => f.CampaignUserId)
            .HasConversion(
                v => v.UserIdentifier,     
                v => new CampaignUserId(v))        
            .HasColumnName("user_id")
            .IsRequired();
        
        /* ---------------------------------------------------------------------------------------------------------- * /

        /*Voice Command BC -------------------------------------------------------------------------------------- */
        //Voice
        builder.Entity<Voice>()
            .HasKey(vc => vc.Id);
        builder.Entity<Voice>()
            .Property(vc => vc.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Voice>()
            .Property(vc => vc.OriginalText).IsRequired().HasMaxLength(1000);
        builder.Entity<Voice>()
            .Property(vc => vc.CommandType).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Entity<Voice>()
            .Property(vc => vc.Parameters).HasMaxLength(2000);
        builder.Entity<Voice>()
            .Property(vc => vc.IsValid).IsRequired();
        builder.Entity<Voice>()
            .Property(vc => vc.WasExecuted).IsRequired();
        builder.Entity<Voice>()
            .Property(vc => vc.UserId).IsRequired().HasColumnName("user_id");
        builder.Entity<Voice>()
            .Property(vc => vc.CreatedAt).IsRequired().HasColumnType("datetime");
        builder.Entity<Voice>()
            .Property(vc => vc.ExecutedAt).HasColumnType("datetime");
        builder.Entity<Voice>()
            .Property(vc => vc.ErrorMessage).HasMaxLength(500);
        builder.Entity<Voice>()
            .Property(vc => vc.ResponseMessage).HasMaxLength(1000);

        // Voice Command Indexes
        builder.Entity<Voice>()
            .HasIndex(vc => vc.UserId).HasDatabaseName("ix_voice_commands_user_id");
        builder.Entity<Voice>()
            .HasIndex(vc => vc.CreatedAt).HasDatabaseName("ix_voice_commands_created_at");
        builder.Entity<Voice>()
            .HasIndex(vc => new { vc.UserId, vc.CommandType }).HasDatabaseName("ix_voice_commands_user_id_command_type");
        builder.Entity<Voice>()
            .HasIndex(vc => new { vc.UserId, vc.IsValid }).HasDatabaseName("ix_voice_commands_user_id_is_valid");
        builder.Entity<Voice>()
            .HasIndex(vc => new { vc.UserId, vc.WasExecuted }).HasDatabaseName("ix_voice_commands_user_id_was_executed");

        /* ---------------------------------------------------------------------------------------------------------- */
        
        builder.UseSnakeCaseNamingConvention();
    }
}