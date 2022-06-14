using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.EntityDefinition;

namespace Estudio.Repository.Persistence
{
public class ApplicationContext : DbContext
{
 public ApplicationContext() : base ("name = cnx")
{
this.Configuration.LazyLoadingEnabled = false;
}
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
modelBuilder.Configurations.Add(new gzSystemConfiguration());
modelBuilder.Configurations.Add(new gzModuleConfiguration());
modelBuilder.Configurations.Add(new gzPageConfiguration());
modelBuilder.Configurations.Add(new gzRoleConfiguration());
modelBuilder.Configurations.Add(new gzPermissionConfiguration());
modelBuilder.Configurations.Add(new PagePermissionConfiguration());
modelBuilder.Configurations.Add(new RolePagePermissionConfiguration());
modelBuilder.Configurations.Add(new gzUserConfiguration());
modelBuilder.Configurations.Add(new UserProfileConfiguration());
modelBuilder.Configurations.Add(new AuthorizationConfiguration());
}
public virtual DbSet<gzSystem> SystemRoutines { get; set; }
public virtual DbSet<gzModule> ModuleRoutines { get; set; }
public virtual DbSet<gzPage> PageRoutines { get; set; }
public virtual DbSet<gzPermission> PermissionRoutines { get; set; }
public virtual DbSet<PagePermission> PagePermissionRoutines { get; set; }

public virtual DbSet<gzRole> RoleRoutines { get; set; }
public virtual DbSet<RolePagePermission> RolePagePermissionRoutines { get; set; }
public virtual DbSet<gzUser> UserRoutines { get; set; }
public virtual DbSet<UserProfile> UserProfileRoutines { get; set; }
public virtual DbSet<Authorization> AuthorizationRoutines { get; set; }
}
}
