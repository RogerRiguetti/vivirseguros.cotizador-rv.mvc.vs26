using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class AuthorizationConfiguration : EntityTypeConfiguration<Authorization>
{
public AuthorizationConfiguration()
{
ToTable("tAuthorization", "app");
HasKey(p => p.Id);
Property(c => c.UserId).IsRequired();
Property(c => c.RolePagePermissionId).IsRequired();
}
}
}
