using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class RolePagePermissionConfiguration : EntityTypeConfiguration<RolePagePermission>
{
public RolePagePermissionConfiguration()
{
ToTable("tRolePagePermission", "app");
HasKey(p => p.Id);
Property(c => c.RoleId).IsRequired();
Property(c => c.PagePermissionId).IsRequired();

            HasMany<Authorization>(f => f.Authorizations).WithRequired(f => f.RolePagePermission).HasForeignKey(f => f.RolePagePermissionId);
        }
}
}
