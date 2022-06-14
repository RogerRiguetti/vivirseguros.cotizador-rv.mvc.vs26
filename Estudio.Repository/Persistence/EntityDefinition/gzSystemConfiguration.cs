using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Estudio.Repository.Core.Domain;
namespace Estudio.Repository.Persistence.EntityDefinition
{
public class gzSystemConfiguration : EntityTypeConfiguration<gzSystem>
{
public gzSystemConfiguration()
{
ToTable("tSystem", "app");
HasKey(p => p.Id);
Property(c => c.Description).IsRequired().HasMaxLength(200);
Property(c => c.Active).IsRequired().HasMaxLength(2);
            HasMany<gzModule>(f => f.Modules).WithRequired(f => f.gzSystem).HasForeignKey(f => f.SystemId);
        }
}
}
