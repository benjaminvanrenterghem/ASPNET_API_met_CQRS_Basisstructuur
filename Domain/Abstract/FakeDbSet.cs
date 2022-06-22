using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract {
    public class FakeDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T> where T : class {
        List<T> _data;

        public FakeDbSet(List<T> items = null) {
            _data = items ?? new List<T>();
        }

        public void SetCollection(List<T> collection) {
            this.Clear();
            this.AddRange(collection);
        }

        public void SetSingle(T item) {
            this.Clear();
            this.Add(item);
        }

        public void Clear() {
            _data = new();
        }

        public override T Find(params object[] keyValues) {
            foreach (var obj in keyValues) {
                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (PropertyInfo property in properties) {
                    var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;

                    if (attribute != null) {
                        foreach (var entity in _data) {
                            if ((property.GetValue(entity) ?? "").ToString() == obj.ToString()) {
                                return entity;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public override EntityEntry<T> Add(T item) {
            _data.Add(item);
            return item as EntityEntry<T>;
        }

        public override EntityEntry<T> Remove(T item) {
            _data.Remove(item);
            return item as EntityEntry<T>;
        }

        public override EntityEntry<T> Attach(T item) {
            return null;
        }

        public T Detach(T item) {
            _data.Remove(item);
            return item;
        }

        public T Create() {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public new List<T> Local {
            get { return _data; }
        }

        public override IEntityType EntityType => throw new NotImplementedException();

        public override void AddRange(IEnumerable<T> entities) {
            _data.AddRange(entities);
        }

        public override void RemoveRange(IEnumerable<T> entities) {
            for (int i = entities.Count() - 1; i >= 0; i--) {
                T entity = entities.ElementAt(i);
                if (_data.Contains(entity)) {
                    Remove(entity);
                }
            }

        }

        Type IQueryable.ElementType {
            get { return _data.AsQueryable().ElementType; }
        }

        Expression IQueryable.Expression {
            get { return _data.AsQueryable().Expression; }
        }

        IQueryProvider IQueryable.Provider {
            get { return _data.AsQueryable().Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return _data.GetEnumerator();
        }

    }

}
