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

		
		// public static IQueryable<T> AddFilter<T>(IQueryable<T> query, string propertyName, string searchTerm) {
  //          // Returns : ParameterExpression
  //          // NodeType = Parameter
  //          // Type, Name
  //          var param = Expression.Parameter(typeof(T), "e");

  //          // Returns MemberExpression using the ParameterExpression & a string prop name
  //          var propExpression = Expression.Property(param, propertyName);
    
  //          object value = searchTerm;

  //          // if the type of the property of the parameter in the object != str: convert type to
  //          // asserted property type
  //          if (propExpression.Type != typeof(string))
  //              value = Convert.ChangeType(value, propExpression.Type);

  //          //Expression<Func<T, bool>> filter = ;

  //          //Expression.Lambda<Func<T, bool>>()
  //          var filterLambda = Expression.Lambda<Func<T, bool>>(
  //              Expression.Equal(
  //                  propExpression,
  //                  Expression.Constant(value)
  //              ),
  //              param
  //          );

  //          return query.Where(filterLambda);
  //       }
		

		//public static IEnumerable<T> Search<T>(
  //          this IQueryable<T> source, int skip, int take, string? searchPropertyName, object? searchValue, 
  //          bool useOverridenToString = true, bool convertPropertyType=false, bool useOverridenEquals=false) {

  //          if(!convertPropertyType && !useOverridenToString && !useOverridenEquals) {
  //              throw new ArgumentException("Atleast one of the value comparer flags should be enabled.");
		//	}

  //          var param = Expression.Parameter(typeof(T), "e");
  //          var propExpression = Expression.Property(param, searchPropertyName);

  //          object value = searchValue;
  //          if (propExpression.Type != typeof(string) && convertPropertyType) {
  //              value = Convert.ChangeType(value, propExpression.Type);
  //          } else if (useOverridenToString) {
  //              value = value.ToString();
		//	} // OverridenEquals: do nothing
                

  //          var filterLambda = Expression.Lambda<Func<T, bool>>(
  //              Expression.Equal(
  //                  propExpression,
  //                  Expression.Constant(value)
  //              ),
  //              param
  //          );

  //          return query.Where(filterLambda);
  //      }


	}

}