using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace GuerillaTrader.Web.Framework
{
    public static class LinqExtensions
    {
        #region Kendo
        public static String SortExpression(this SortDescriptor sortDescriptor)
        {
            return String.Format("{0} {1}", sortDescriptor.Member, sortDescriptor.SortDirection == System.ComponentModel.ListSortDirection.Descending ? "desc" : "asc");
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, SortDescriptor sortDescriptor)
        {
            return source.OrderBy(sortDescriptor.Member, sortDescriptor.SortDirection == System.ComponentModel.ListSortDirection.Descending);
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, IList<IFilterDescriptor> filterDescriptors)
        {
            List<Expression> equalExpressions = new List<Expression>();
            ParameterExpression item = Expression.Parameter(typeof(TEntity), "item");

            IterateIFilterDescriptors(equalExpressions, item, filterDescriptors);

            if (equalExpressions.Count == 1)
            {
                source = source.Where(Expression.Lambda<Func<TEntity, bool>>(equalExpressions.First(), item));
            }
            else if (equalExpressions.Count == 2)
            {
                var and = Expression.And(equalExpressions[0], equalExpressions[1]);
                source = source.Where(Expression.Lambda<Func<TEntity, bool>>(and, item));
            }
            else if (equalExpressions.Count > 2)
            {
                var and = Expression.And(equalExpressions[0], equalExpressions[1]);

                for (int i = 2; i < equalExpressions.Count; i++)
                {
                    and = Expression.And(and, equalExpressions[i]);
                }

                source = source.Where(Expression.Lambda<Func<TEntity, bool>>(and, item));
            }

            return source;
        }

        public static Expression<Func<TEntity, bool>> ToLambda<TEntity>(this PropertyInfo pi, bool compare)
        {
            ParameterExpression item = Expression.Parameter(typeof(TEntity), "item");
            FilterDescriptor fd = new FilterDescriptor(pi.Name, FilterOperator.IsEqualTo, compare);
            Expression expression = fd.GetEqualityExpression(item);
            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(expression, item);
            return lambda;
        }

        public static void IterateIFilterDescriptors(List<Expression> equalExpressions, ParameterExpression item, IList<IFilterDescriptor> filterDescriptors)
        {
            Expression expression = null;

            foreach (IFilterDescriptor iFilterDescriptor in filterDescriptors)
            {
                if (iFilterDescriptor is FilterDescriptor)
                {
                    expression = (iFilterDescriptor as FilterDescriptor).GetEqualityExpression(item);
                    if (expression != null)
                    {
                        equalExpressions.Add(expression);
                    }
                }
                else if (iFilterDescriptor is CompositeFilterDescriptor)
                {
                    IterateIFilterDescriptors(equalExpressions, item, (iFilterDescriptor as CompositeFilterDescriptor).FilterDescriptors);
                }
            }
        }

        public static Expression GetEqualityExpression(this FilterDescriptor filterDescriptor, ParameterExpression item)
        {
            var prop = Expression.Property(item, filterDescriptor.Member);
            ConstantExpression val = null;
            Expression expression = null;

            if (filterDescriptor.ConvertedValue is DateTime || filterDescriptor.ConvertedValue is DateTime?)
            {
                val = Expression.Constant((DateTime)filterDescriptor.ConvertedValue);
            }
            //else if (filterDescriptor.Member == "MpaaRating")
            //{
            //    val = Expression.Constant((MpaaRatings)Int32.Parse(filterDescriptor.ConvertedValue.ToString()));
            //}
            //else if (filterDescriptor.Member == "Genre")
            //{
            //    val = Expression.Constant((MovieGenres)Int32.Parse(filterDescriptor.ConvertedValue.ToString()));
            //}
            //else if (filterDescriptor.Member == "Brand")
            //{
            //    val = Expression.Constant((MovieBrands)Int32.Parse(filterDescriptor.ConvertedValue.ToString()));
            //}
            //else if (filterDescriptor.Member == "MicroSeries" || filterDescriptor.Member == "MacroSeries")
            //{
            //    val = Expression.Constant((MovieSeries)Int32.Parse(filterDescriptor.ConvertedValue.ToString()));
            //}
            else if (prop.Type == typeof(Int32) || prop.Type == typeof(int?))
            {
                val = Expression.Constant(Int32.Parse(filterDescriptor.ConvertedValue.ToString()));
            }
            else if (prop.Type == typeof(Double))
            {
                val = Expression.Constant(Double.Parse(filterDescriptor.ConvertedValue.ToString()));
            }
            else
            {
                val = Expression.Constant(filterDescriptor.ConvertedValue.ToString());
            }

            if (filterDescriptor.Operator == FilterOperator.IsEqualTo)
            {

                if (prop.Type == typeof(int?)) expression = Expression.Equal(prop, Expression.Convert(val, prop.Type));
                else if (prop.Type == typeof(DateTime?)) expression = Expression.Equal(prop, Expression.Convert(val, prop.Type));
                else if (prop.Type == typeof(Boolean))
                {
                    bool b = Boolean.Parse(val.Value.ToString());
                    ConstantExpression c = Expression.Constant(b, typeof(Boolean));
                    expression = Expression.Equal(prop, Expression.Convert(c, prop.Type));
                }
                else expression = expression = Expression.Equal(prop, val);
            }
            else if (filterDescriptor.Operator == FilterOperator.IsNotEqualTo)
            {
                if (prop.Type == typeof(DateTime?)) expression = Expression.NotEqual(prop, Expression.Convert(val, prop.Type));
                else expression = Expression.NotEqual(prop, val);
            }
            else if (filterDescriptor.Operator == FilterOperator.IsGreaterThan)
            {
                if (prop.Type == typeof(DateTime?)) expression = Expression.GreaterThan(prop, Expression.Convert(val, prop.Type));
                else expression = Expression.GreaterThan(prop, val);
            }
            else if (filterDescriptor.Operator == FilterOperator.IsGreaterThanOrEqualTo)
            {
                if (prop.Type == typeof(DateTime?)) expression = Expression.GreaterThanOrEqual(prop, Expression.Convert(val, prop.Type));
                else expression = Expression.GreaterThanOrEqual(prop, val);
            }
            else if (filterDescriptor.Operator == FilterOperator.IsLessThan)
            {
                if (prop.Type == typeof(DateTime?)) expression = Expression.LessThan(prop, Expression.Convert(val, prop.Type));
                else expression = Expression.LessThan(prop, val);
            }
            else if (filterDescriptor.Operator == FilterOperator.IsLessThanOrEqualTo)
            {
                if (prop.Type == typeof(DateTime?)) expression = Expression.LessThanOrEqual(prop, Expression.Convert(val, prop.Type));
                else expression = Expression.LessThanOrEqual(prop, val);
            }
            else if (filterDescriptor.Operator == FilterOperator.Contains)
            {
                expression = Expression.Call(prop, typeof(String).GetMethod("Contains"), val);
            }

            return expression;
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, FilterDescriptor filterDescriptor)
        {
            var item = Expression.Parameter(typeof(TEntity), "item");
            var prop = Expression.Property(item, filterDescriptor.Member);
            var val = Expression.Constant(filterDescriptor.ConvertedValue.ToString());
            var equal = Expression.Equal(prop, val);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);

            //string command = "Where";
            //var type = typeof(TEntity);
            //var property = type.GetProperty(filterDescriptor.Member);
            //var parameter = Expression.Parameter(type, "p");
            //var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            //var whereExpression = Expression.Lambda(propertyAccess, parameter);
            //var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
            //                              source.Expression, Expression.Quote(lambda));
            return source.Where(lambda);
            //return source.Provider.CreateQuery<TEntity>(filterDescriptor.CreateFilterExpression());
        }
        #endregion
    }
}