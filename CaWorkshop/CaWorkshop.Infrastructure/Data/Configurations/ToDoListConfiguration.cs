using CaWorkshop.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaWorkshop.Infrastructure.Data.Configurations;

public class ToDoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        
        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(280);
    }
}