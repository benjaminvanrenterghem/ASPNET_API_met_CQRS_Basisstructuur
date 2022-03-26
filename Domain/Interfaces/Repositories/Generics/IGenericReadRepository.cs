using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories.Generics {
	public interface IGenericReadRepository<T> where T : IEntity {
		IQueryable<T> GetAll();
		IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

		// Id is een object aangezien de primary key zowel een int als string kan zijn
		T? GetById(object id);
	}
}
