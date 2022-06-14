using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
{
public UserProfileConfiguration()
{
ToTable("tUserProfile", "app");
HasKey(p => p.UserId);
Property(c => c.Name).IsRequired().HasMaxLength(100);
}
}
}
