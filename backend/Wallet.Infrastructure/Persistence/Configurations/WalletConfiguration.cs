using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletEntity = Wallet.Domain.Entities.Wallet;

namespace Wallet.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<WalletEntity>
    {
        public void Configure(EntityTypeBuilder<WalletEntity> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.UserId)
                   .IsRequired();

            // ✅ Mapping for Value Object: Money -> Currency
            builder.OwnsOne(w => w.Balance, balance =>
            {
                balance.Property(b => b.Amount)
                       .HasColumnName("Balance_Amount")
                       .IsRequired();

                balance.OwnsOne(b => b.Currency, currency =>
                {
                    currency.Property(c => c.Code)
                            .HasColumnName("Balance_CurrencyCode")
                            .HasMaxLength(3)
                            .IsRequired();

                    currency.Property(c => c.Symbol)
                            .HasColumnName("Balance_CurrencySymbol")
                            .HasMaxLength(5)
                            .IsRequired(false);
                });
            });


            builder.Property(w => w.Status)
                   .IsRequired();

            builder.Property(w => w.CreatedAt)
                   .IsRequired();

            builder.Property(w => w.UpdatedAt);

            // Optional: Table name
            builder.ToTable("Wallets");
        }
    }
}
