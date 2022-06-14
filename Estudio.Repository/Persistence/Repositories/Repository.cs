using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Repositories;
using System.Data.Entity;
using System.Linq.Expressions;
namespace Estudio.Repository.Persistence.Repositories
{
public class Repository<T> : IRepository<T> where T : class
{
protected readonly DbContext Context;
public Repository(DbContext _context)
{
Context = _context;
}
public T Get(int _id)
{
return Context.Set<T>().Find(_id);
}
public void Add(T _entity)
{
Context.Set<T>().Add(_entity);
}
public void AddRange(IEnumerable<T> _entities)
{
Context.Set<T>().AddRange(_entities);
}
public void Remove(T _entity)
{
Context.Set<T>().Remove(_entity);
}
public void RemoveRange(IEnumerable<T> _entities)
{
Context.Set<T>().RemoveRange(_entities);
}
public IEnumerable<T> GetAll()
{
return Context.Set<T>().ToList();
}
public IEnumerable<T> Find(Expression<Func<T, bool>> _predicate)
{
return Context.Set<T>().Where(_predicate).ToList();
}
public T Single(Expression<Func<T, bool>> _predicate)
{
return Context.Set<T>().FirstOrDefault(_predicate);
}
public void Attach(T _entity)
{
Context.Entry(_entity).State = EntityState.Modified;
}
}
}
