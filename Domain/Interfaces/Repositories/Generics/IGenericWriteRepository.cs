namespace Domain.Interfaces.Repositories.Generics {
	public interface IGenericWriteRepository<T> where T : IEntity {
		void Insert(T entity);
		void Update(T entity);
		void SoftDelete(object id);
		void Save();
		void Clear();
	}
}
