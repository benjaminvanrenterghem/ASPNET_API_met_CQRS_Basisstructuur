using AutoMapper.Internal;
using System.Linq.Expressions;

namespace Logic.Extensions {
	public static class QueryableExtensions {
		public static IEnumerable<T> DeprecatedSearch<T>(this IQueryable<T> source, int skip, int take, string? searchPropertyName, object? searchValue) {
			if (source == null) {
				throw new ArgumentNullException();
			}

			if (searchPropertyName != null) {
				var variable = typeof(T).GetFieldOrProperty(searchPropertyName);

				var res = from x in source.ToList()
						  let v = variable.GetMemberValue(x)
						  where (v?.ToString() ?? String.Empty).ToLower().Contains((searchValue?.ToString() ?? String.Empty).ToLower())
						  select x;

				return res.Skip(skip).Take(take).ToList();
			}

				return source.Skip(skip).Take(take).ToList();
			}
		}		

		// todo CRIT - zie expr playground 

}