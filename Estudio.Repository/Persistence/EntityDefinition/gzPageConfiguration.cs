using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class gzPageConfiguration : EntityTypeConfiguration<gzPage>
{
public gzPageConfiguration()
{
ToTable("tPage", "app");
HasKey(p => p.Id);
Property(c => c.Description).IsRequired().HasMaxLength(100);
Property(c => c.Active).IsRequired().HasMaxLength(2);
Property(c => c.ModuleId).IsRequired();
            HasMany<PagePermission>(f => f.PagePermissions).WithRequired(f => f.gzPage).HasForeignKey(f => f.PageId);
        }
}
}
