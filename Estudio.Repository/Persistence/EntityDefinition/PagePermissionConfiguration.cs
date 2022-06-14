using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class PagePermissionConfiguration : EntityTypeConfiguration<PagePermission>
{
public PagePermissionConfiguration()
{
ToTable("tPagePermission", "app");
HasKey(p => p.Id);
Property(c => c.PageId).IsRequired();
Property(c => c.PermissionId).IsRequired();

            HasMany<RolePagePermission>(f => f.RolePagePermissions).WithRequired(f => f.PagePermission).HasForeignKey(f => f.PagePermissionId);
        }
}
}
