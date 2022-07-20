using Domain.Abstract;
using Domain.Exceptions;
using Domain.Interfaces.Repositories.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Generics {
	public class GenericWriteRepository<T> : IGenericWriteRepository<T> where T : Entity {
		protected NetworkDbContext _context = null;
		protected DbSet<T> _entities = null;

		public GenericWriteRepository(NetworkDbContext ctx = null) {
			if (ctx == null) {
				_context = new NetworkDbContext(null);
			} else {
				_context = ctx;
			}

			_entities = _context.Set<T>();
		}

		public void Insert(T entity) {
			_entities.Add(entity);
		}

		public void Update(T entity, T? updatedEntity=default) {
			_entities.Attach(entity);

			// Indien men (evt middels mapping) beschikt over een entity met nieuwe waarden kan deze optioneel meegegeven worden, de getrackede entity's values worden aangepast
			if(updatedEntity is not null) {
				_context.Entry(entity).CurrentValues.SetValues(updatedEntity);
			}

			_context.Entry(entity).State = EntityState.Modified;
		}


		public void SoftDelete(object id) {
			T? fetchedEntity = _entities.Find(id);

			if (fetchedEntity != null) {
				if (fetchedEntity.Deleted == true) {
					throw new RepositoryException("Entity is already softdeleted.");
				}

				fetchedEntity.Deleted = true;
				_entities.Update(fetchedEntity);
				return;
			}

			throw new RepositoryException($"Could not find object with id {id}");
		}

		public void UndoSoftDelete(object id) {
			T? fetchedEntity = _entities.Find(id);

			if (fetchedEntity != null) {
				if (fetchedEntity.Deleted == false || fetchedEntity.Deleted == null) {
					throw new RepositoryException("Entity is already not softdeleted.");
				}

				fetchedEntity.Deleted = false;
				_entities.Update(fetchedEntity);
				return;
			}

			throw new RepositoryException($"Could not find object with id {id}");
		}

		public void Save() {
			_context.SaveChanges();
		}
		public void Clear() {
			_context.ChangeTracker.Clear();
		}

	}
}
