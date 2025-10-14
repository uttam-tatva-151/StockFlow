using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StockFlow.Repository.Entities.Configurations;
public class UserAuthConfiguration : IEntityTypeConfiguration<UserAuth>
{
    public void Configure(EntityTypeBuilder<UserAuth> builder)
        {
            builder.ToTable("user_auth");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.IsActive)
                   .HasDefaultValue(true);

            builder.Property(u => u.CreatedDate)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    
}
