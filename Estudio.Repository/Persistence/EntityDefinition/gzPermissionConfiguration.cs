using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class gzPermissionConfiguration : EntityTypeConfiguration<gzPermission>
{
public gzPermissionConfiguration()
{
ToTable("tPermission", "app");
HasKey(p => p.Id);
Property(c => c.Description).IsRequired().HasMaxLength(100);
Property(c => c.Active).IsRequired().HasMaxLength(2);

            HasMany<PagePermission>(f => f.PagePermissions).WithRequired(f => f.gzPermission).HasForeignKey(f => f.PermissionId);
        }
}
}
