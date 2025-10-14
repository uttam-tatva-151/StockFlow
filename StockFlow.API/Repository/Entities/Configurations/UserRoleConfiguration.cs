using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StockFlow.Repository.Entities.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");

            builder.HasKey(ur => ur.Id);

            builder.Property(ur => ur.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(ur => ur.UserAuth)
                   .WithMany()
                   .HasForeignKey(ur => ur.UserAuthId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Role)
                   .WithMany()
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
