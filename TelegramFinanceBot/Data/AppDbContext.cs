using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Models;

namespace TelegramFinanceBot.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BotChat> Chats => Set<BotChat>();

    public DbSet<Spending> Spendings => Set<Spending>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BotChat>(entity =>
        {
            entity.ToTable("Chats");
            entity.HasKey(chat => chat.Id);
            entity.Property(chat => chat.Id).ValueGeneratedNever();
            entity.Property(chat => chat.ReportToken).IsRequired().HasMaxLength(64);
            entity.Property(chat => chat.StartedAt).HasColumnType("timestamp with time zone");
            entity.HasIndex(chat => chat.TelegramChatId).IsUnique();
            entity.HasIndex(chat => chat.ReportToken).IsUnique();
        });

        modelBuilder.Entity<Spending>(entity =>
        {
            entity.ToTable("Spendings");
            entity.HasKey(spending => spending.Id);
            entity.Property(spending => spending.Id).ValueGeneratedNever();
            entity.Property(spending => spending.Amount).HasPrecision(18, 2);
            entity.Property(spending => spending.Category).IsRequired().HasMaxLength(64);
            entity.Property(spending => spending.SpentAt).HasColumnType("timestamp with time zone");
            entity.HasIndex(spending => new { spending.ChatId, spending.SpentAt });
            entity.HasOne(spending => spending.Chat)
                .WithMany(chat => chat.Spendings)
                .HasForeignKey(spending => spending.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
