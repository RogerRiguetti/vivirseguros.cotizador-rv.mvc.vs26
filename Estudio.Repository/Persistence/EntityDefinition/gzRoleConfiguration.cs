using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class gzRoleConfiguration : EntityTypeConfiguration<gzRole>
{
public gzRoleConfiguration()
{
ToTable("tRole", "app");
HasKey(p => p.Id);
Property(c => c.Description).IsRequired().HasMaxLength(100);
Property(c => c.Active).IsRequired().HasMaxLength(2);

            HasMany<RolePagePermission>(f => f.RolePagePermissions).WithRequired(f => f.Role).HasForeignKey(f => f.RoleId);
        }
}
}
