using System;
using System.Collections.Generic;
using Dormouse.Core.Search;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;

namespace Dormouse.Core
{
    public static class Utilities
    {
        public static ICriteria SetSearchCriteria(object obj, ICriteria c)
        {
            Type t = obj.GetType();
            string strTemp;
            foreach (PropertyInfo p in t.GetProperties())
            {
                if (p.GetValue(obj, null) == null) continue;
                strTemp = p.GetValue(obj, null).ToString();
                if (strTemp != "")
                    c.Add(Expression.Eq(p.Name, p.GetValue(obj, null)));
            }
            return c;
        }

        public static ICriteria SetComplexSearchCriteria(IList<SearchCriteria> critList, ICriteria c)
        {
            foreach (SearchCriteria s in critList)
            {
                switch (s.Compare)
                {
                    case ComparisonType.NotEqual:
                        c.Add(Expression.Not(Expression.Eq(s.PropertyName, s.Value)));
                        break;
                    case ComparisonType.Equals:
                        c.Add(Expression.Eq(s.PropertyName, s.Value));
                        break;
                    case ComparisonType.GreaterThan:
                        c.Add(Expression.Gt(s.PropertyName, s.Value));
                        break;
                    case ComparisonType.GreaterThanOrEqualTo:
                        c.Add(Expression.Or(
                            Expression.Gt(s.PropertyName, s.Value),
                            Expression.Eq(s.PropertyName, s.Value)
                            ));
                        break;
                    case ComparisonType.GreaterThanOrNull:
                        c.Add(Expression.Or(
                            Expression.Gt(s.PropertyName, s.Value),
                            Expression.IsNull(s.PropertyName)
                            ));
                        break;
                    case ComparisonType.LessThan:
                        c.Add(Expression.Lt(s.PropertyName, s.Value));
                        break;
                    case ComparisonType.LessThanOrEqualTo:
                        c.Add(Expression.Or(
                            Expression.Lt(s.PropertyName, s.Value),
                            Expression.Eq(s.PropertyName, s.Value)
                            ));
                        break;
                    case ComparisonType.LessThanOrNull:
                        c.Add(Expression.Or(
                            Expression.Lt(s.PropertyName, s.Value),
                            Expression.IsNull(s.PropertyName)
                            ));
                        break;
                    case ComparisonType.Like:
                        c.Add(Expression.InsensitiveLike(s.PropertyName, s.Value));
                        break;
                    case ComparisonType.LikeStartWith:
                        c.Add(Expression.InsensitiveLike(s.PropertyName, s.Value.ToString(), MatchMode.Start));
                        break;
                    case ComparisonType.LikeEndWith:
                        c.Add(Expression.InsensitiveLike(s.PropertyName, s.Value.ToString(), MatchMode.End));
                        break;
                    case ComparisonType.LikeAnywhere:
                        c.Add(Expression.InsensitiveLike(s.PropertyName, s.Value.ToString(), MatchMode.Anywhere));
                        break;
                    case ComparisonType.InString:
                        c.Add(Expression.InG(s.PropertyName, s.Value as List<string>));
                        break;
                    case ComparisonType.InInt:
                        c.Add(Expression.InG(s.PropertyName, s.Value as List<int>));
                        break;
                    case ComparisonType.InGUID:
                        c.Add(Expression.InG(s.PropertyName, s.Value as List<Guid>));
                        break;
                    case ComparisonType.NotInInt:
                        c.Add(Expression.Not(Expression.InG(s.PropertyName, s.Value as List<int>)));
                        break;
                    case ComparisonType.SqlExp:
                        c.Add(Expression.Sql(s.Value.ToString()));
                        break;
                    case ComparisonType.EqualsOrNull:
                        c.Add(Expression.Or(Expression.Eq(s.PropertyName, s.Value), Expression.IsNull(s.PropertyName)));
                        break;
                    case ComparisonType.NotEqualsOrNull:
                        c.Add(Expression.Or(
                                Expression.Not(Expression.Eq(s.PropertyName, s.Value)),
                                Expression.IsNull(s.PropertyName)
                            ));
                        break;
                    case ComparisonType.IsNull:
                        c.Add(Expression.IsNull(s.PropertyName));
                        break;
                    case ComparisonType.IsNotNull:
                        c.Add(Expression.IsNotNull(s.PropertyName));
                        break;
                }
            }
            return c;
        }

        public static ICriteria OrderComplexSearch(IList<OrderCriteria> critList, ICriteria c)
        {
            foreach (OrderCriteria s in critList)
            {
                switch (s.Order)
                {
                    case OrderType.ASC:
                        c.AddOrder(Order.Asc(s.PropertyName));
                        break;
                    case OrderType.DESC:
                        c.AddOrder(Order.Desc(s.PropertyName));
                        break;
                }
            }
            return c;
        }

        public static void CreateSchema()
        {
            var configuration = new Configuration().Configure();
            var schema = new SchemaExport(configuration);
            schema.Create(true, true);            
        }
    }
}
