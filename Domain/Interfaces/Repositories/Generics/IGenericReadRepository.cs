using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories.Generics {
	public interface IGenericReadRepository<T> where T : IEntity {
		IQueryable<T> GetAll();
		IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
		IQueryable<T> GetAllWithLazyLoading();
		IQueryable<T> GetAllWithLazyLoading(Expression<Func<T, bool>> predicate);

		// Id is een object aangezien de primary key zowel een int als string kan zijn
		T? GetById(object id);
	}
}
