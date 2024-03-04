using LogisticsTrack.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace LogisticsTrack.Service
{
    public class SearchFilterInSqlService<TEntity>
      where TEntity : Entity, ISoftDeletableEntity, IEntity, new()
    {
        public static IQueryable<TEntity> GetSearchQuery(IQueryable<TEntity> query, string propertyName, string mappedTotypeName, string queryString)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return query;
            }

            queryString = queryString.ToLower();

            string columnPropertyName = ColumnMappingService.MapColumn(typeof(TEntity).Name, mappedTotypeName + '.' + propertyName);

            // get property with "column" name ignoring casing and taking only public instance properties
            PropertyInfo property = typeof(TEntity).GetProperty(columnPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
            {
                return query;
            }

            // create parameter node for parameter identification ("x" in lambda expression)
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "x");
            MemberExpression memberExpression = Expression.Property(parameterExpression, columnPropertyName);

            ConstantExpression constant = Expression.Constant(queryString, queryString.GetType());

            Expression body = Expression.Empty();
            if (property.PropertyType == typeof(string))
            {
                var left = Expression.Call(
                    memberExpression, // type/member that contains method defined
                    "ToLower", // method name
                    null // types method parameters if generic method
                );

                var right = constant;

                body = Expression.Call(
                    memberExpression, // type/member that contains method defined
                    "StartsWith", // method name
                    null, // types method parameters if generic method
                    constant // values of method arguments
                );
            }
            // to handle decimal starts with query filter in SQL server
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {

                MethodInfo sqlConvertMethod = typeof(DbFunctions).GetMethod("StringConvert", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(decimal?), typeof(int?), typeof(int?) }, null);

                var firstArg = Expression.Call(
                    sqlConvertMethod, // method
                    Expression.Convert(memberExpression, typeof(decimal?)),
                    Expression.Constant(30, typeof(int?)),
                    Expression.Constant(30, typeof(int?))
                );
                firstArg = Expression.Call(firstArg, "Trim", null); // trim resulting string from string convert

                var secondArgValueExpression = Expression.Call(
                    constant, // type/member that contains method defined
                    "ToString", // method name
                    null // types method parameters if generic method
                );
                var secondArgValue = (string)Expression.Lambda(secondArgValueExpression).Compile().DynamicInvoke();
                var secondArg = Expression.Constant(secondArgValue + "%", typeof(string));

                MethodInfo dbLikeMethod = typeof(DbFunctions).GetMethod("Like", BindingFlags.Public | BindingFlags.Static, null,
                                                                        new Type[] { typeof(string), typeof(string) }, null);
                body = Expression.Call(dbLikeMethod, firstArg, secondArg);
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                MethodInfo dbDateTime = typeof(DateTime).GetMethod("ToString", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
                var expDt = Expression.Constant("dd.MM.yyyy HH:mm:ss");
                var left = Expression.Call(
                    memberExpression, // type/member that contains method defined
                     dbDateTime,
                     new Expression[]
                     {
                         expDt
                     }
                    );
                var right = constant;
                var secondArgValueExpression = Expression.Call(
                  constant, // type/member that contains method defined
                  "ToString",
                  null
                 );

                var secondArgValue = (string)Expression.Lambda(secondArgValueExpression).Compile().DynamicInvoke();
                var secondArg = Expression.Constant(secondArgValue + "%", typeof(string));
                var likeMethod = typeof(DbFunctionsExtensions)
                        .GetMethods()
                        .Where(p => p.Name == "Like")
                        .First();



                body = Expression.Call(likeMethod, new Expression[]
                                    {
                                        Expression.Property(null, typeof(EF).GetProperty("Functions")),
                                        left,
                                        secondArg
                                    });


            }
            else
            {
                var left = Expression.Call(
                    memberExpression, // type/member that contains method defined
                    "ToString", // method name
                    null // types method parameters if generic method
                );
                var right = constant;

                left = Expression.Call(left, "ToLower", null);
                body = Expression.Equal(left, right);
            }

            // get expression refering to property in question
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(body, parameterExpression);

            query = query.Where(lambdaExpression); // Define query.

            return query;
        }
    }
}
