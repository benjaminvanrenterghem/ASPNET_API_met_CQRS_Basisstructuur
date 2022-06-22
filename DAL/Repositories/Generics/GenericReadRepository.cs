using System.Linq.Expressions;
using DAL;
using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Generics {
	public class GenericReadRepository<T> : IGenericReadRepository<T> where T : Entity {
		protected NetworkDbContext _context = null;
		protected DbSet<T> _entities = null;


		// xUnit
		public GenericReadRepository(DbSet<T> dbSet = null, NetworkDbContext context = null) : this(context) {
			if (dbSet != null) {
				_entities = dbSet;
			}
		}


		public GenericReadRepository(NetworkDbContext ctx = null) {
			if (ctx == null) {
				_context = new NetworkDbContext(null);
			} else {
				_context = ctx;
			}

			_entities = _context.Set<T>();
		}

		// Changetracker nullchecks voor QOL bij unit testing
		public IQueryable<T> GetAll() {
			if (_context.ChangeTracker != null) {
				_context.ChangeTracker.LazyLoadingEnabled = false;
			}
			return GetAllWithLazyLoading();
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate) {
			if (_context.ChangeTracker != null) {
				_context.ChangeTracker.LazyLoadingEnabled = false;
			}
			return GetAllWithLazyLoading(predicate);
		}

		public IQueryable<T> GetAllWithLazyLoading() {
			return _entities;
		}

		public IQueryable<T> GetAllWithLazyLoading(Expression<Func<T, bool>> predicate) {
			return _entities.Where(predicate);
		}

		public T? GetById(object id) {
			return _entities.Find(id);
		}


	}
}
