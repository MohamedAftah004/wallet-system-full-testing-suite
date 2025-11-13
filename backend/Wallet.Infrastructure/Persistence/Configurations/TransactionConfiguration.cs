using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.WalletId)
                   .IsRequired();

            builder.Property(t => t.Type)
                   .IsRequired();

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.Property(t => t.ReferenceId)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(t => t.Description)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                   .IsRequired();

            builder.Property(t => t.UpdatedAt)
                   .IsRequired(false);

            // 🟢 Owned type configuration for Money
            builder.OwnsOne(t => t.Amount, amount =>
            {
                amount.Property(a => a.Amount)
                      .HasColumnName("Amount_Value")
                      .IsRequired();

                amount.OwnsOne(a => a.Currency, currency =>
                {
                    currency.Property(c => c.Code)
                            .HasColumnName("Amount_CurrencyCode")
                            .HasMaxLength(3)
                            .IsRequired();

                    currency.Property(c => c.Symbol)
                            .HasColumnName("Amount_CurrencySymbol")
                            .HasMaxLength(5);
                });
            });

            // 🔗 العلاقة مع Wallet
            builder.HasOne(t => t.Wallet)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.WalletId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
