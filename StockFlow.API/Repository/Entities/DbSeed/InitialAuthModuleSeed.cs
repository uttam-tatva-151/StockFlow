using Microsoft.EntityFrameworkCore;

namespace StockFlow.Repository.Entities.DbSeed;

public class InitialAuthModuleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // ===== Roles =====
        Guid roleAdminId = Guid.NewGuid();
        Guid roleOwnerId = Guid.NewGuid();
        Guid roleStaffId = Guid.NewGuid();

        // ===== Users =====
        Guid adminUserId = Guid.NewGuid();
        Guid ownerUserId = Guid.NewGuid();
        Guid staffUserId = Guid.NewGuid();

        // ===== Roles =====
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = roleAdminId,
                RoleName = "Admin",
                Description = "Full access role",
                CreatedDate = DateTime.Now
            },
            new Role
            {
                Id = roleOwnerId,
                RoleName = "Owner",
                Description = "Inventory & order management role",
                CreatedDate = DateTime.Now
            },
            new Role
            {
                Id = roleStaffId,
                RoleName = "Staff",
                Description = "Day-to-day operations role",
                CreatedDate = DateTime.Now
            }
        );

        // ===== Users =====
        modelBuilder.Entity<UserAuth>().HasData(
            new UserAuth
            {
                Id = adminUserId,
                Email = "admin@example.com",
                PasswordHash = "Admin@123", // hash later
                IsActive = true,
                CreatedDate = DateTime.Now
            },
            new UserAuth
            {
                Id = ownerUserId,
                Email = "owner@example.com",
                PasswordHash = "Owner@123",
                IsActive = true,
                CreatedDate = DateTime.Now
            },
            new UserAuth
            {
                Id = staffUserId,
                Email = "staff@example.com",
                PasswordHash = "Staff@123",
                IsActive = true,
                CreatedDate = DateTime.Now
            }
        );

        // ===== UserRoles =====
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserAuthId = adminUserId,
                RoleId = roleAdminId,
                CreatedDate = DateTime.Now
            },
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserAuthId = ownerUserId,
                RoleId = roleOwnerId,
                CreatedDate = DateTime.Now
            },
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserAuthId = staffUserId,
                RoleId = roleStaffId,
                CreatedDate = DateTime.Now
            }
        );
    }
}
