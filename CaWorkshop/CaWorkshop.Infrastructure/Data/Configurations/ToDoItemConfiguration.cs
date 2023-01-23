using CaWorkshop.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaWorkshop.Infrastructure.Data.Configurations;

public class ToDoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(280);

        builder.Property(p => p.Note)
               .HasMaxLength(4000);
    }
}