using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories.Generics {
	public interface IGenericWriteRepository<T> where T : IEntity {
		void Insert(T entity);
		void Update(T entity);
		void SoftDelete(object id);
		void Save();
		void SaveAndClear();
		void Clear();
	}
}
