using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class gzModuleConfiguration : EntityTypeConfiguration<gzModule>
{
public gzModuleConfiguration()
{
ToTable("tModule", "app");
HasKey(p => p.Id);
Property(c => c.Description).IsRequired().HasMaxLength(200);
Property(c => c.Active).IsRequired().HasMaxLength(2);
Property(c => c.SystemId).IsRequired();

            HasMany<gzPage>(f => f.Pages).WithRequired(f => f.Module).HasForeignKey(f => f.ModuleId);
        }
}
}
