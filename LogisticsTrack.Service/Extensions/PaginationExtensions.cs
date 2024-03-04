
using LogisticsTrack.Domain.BaseEntities;
using LogisticsTrack.Domain.Enums;
using LogisticsTrack.Domain.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text; 

namespace LogisticsTrack.Service.Extensions
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// applies pagination on query
        /// </summary>
        /// <typeparam name="T">on what type queriable is operating</typeparam>
        /// <param name="query">initial query</param>
        /// <param name="parameters">parameters for pagination, default null</param>
        /// <param name="mappedToType">to which type it will be mapped</param>
        /// /// <returns>queriable query with pagination parameters added to expression tree or same query if parameters are null</returns>

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationParameters parameters, string mappedToTypeName)
            where T : class, IEntity
        {
            if (null == parameters)
            {
                return query;
            }

            string column = ColumnMappingService.MapColumn(typeof(T).Name, mappedToTypeName + '.' + parameters.Column);

            // get property with "column" name ignoring casing and taking only public instance properties
            // if column is null, use default "Id"
            PropertyInfo property = typeof(T).GetProperty(column ?? "Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // if it happens that it is null, but should not be (if column exists), then manualy use Id
            if (property == null)
            {
                // default "ordering"
                query = query.OrderBy(x => x.Id);
            }
            else
            {
                // create parameter node for parameter identification ("x" in lambda expression)
                var parameter = Expression.Parameter(typeof(T), "x");
                var memberExpression = Expression.Property(parameter, property.Name);

                // get expression refering to property in question
                LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameter);

                // make query from existing + ordering by calling orderby or orderbydescending method
                query = query.Provider.CreateQuery<T>(
                           Expression.Call(typeof(Queryable), // type/parameter that contains method defined
                                           parameters.Ordering == Ordering.asc ? "OrderBy" : "OrderByDescending", // method name
                                           new Type[] { query.ElementType, lambdaExpression.Body.Type }, // types method parameters if generic method
                                           query.Expression, lambdaExpression)); // values of method arguments
            }

            query = query.Skip(parameters.Skip).Take(parameters.Take);

            return query;
        }
    }
}
