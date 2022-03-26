using Domain.Abstract;
using Domain.Interfaces.Repositories.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Generics {
	public class GenericReadRepository<T> : IGenericReadRepository<T> where T : Entity {
		protected NetworkDbContext _context = null;
		protected DbSet<T> _entities = null;

		public GenericReadRepository(NetworkDbContext ctx = null) {
			if (ctx == null) {
				_context = new NetworkDbContext(null);
			} else {
				_context = ctx;
			}

			_entities = _context.Set<T>();
		}

		// .ToList() weg, wordt best zo laat mogelijk geconcat bij gebruik,
		// zodat er nog gefiltered kan worden alvorens db operatie plaatsvind
		public IQueryable<T> GetAll() {
			return _entities;
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate) {
			return _entities.Where(predicate);
		}

		public T? GetById(object id) {
			return _entities.Find(id);
		}


	}
}
