using EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.EntityTypeConfiguration
{
    public class DeveloperEntityTypeConfiguration : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> entity)
        {
            entity.ToTable("Developers");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(e => new { e.Name, e.Country }, "IX_Developerss_Name_Country")
                .IsUnique();

            entity.HasMany(e => e.Games)
                .WithOne(b => b.Developer)
                .HasForeignKey(b => b.DeveloperId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
