﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class ReadonlyTemporalRepo<TEntity, TContext, THistoryContext>
        : ReadonlyRepo<TEntity,TContext>, ITemporalRepo<TEntity,TContext,THistoryContext>
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext: DbContext {


        public THistoryContext HistoryContext { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public ReadonlyTemporalRepo(TContext context, THistoryContext historyContext) :
            base(context) {
            HistoryContext = historyContext;
        }



        public List<TEntity> QueryAsOf(DateTime from,
                DateTime to, Expression<Func<TEntity, bool>> predicate,
                int pageNumber = 1, int pageSize = 10000,
                params Expression<Func<TEntity, dynamic>>[] orderSelectors
                ) {

            var asOfPredicate = GetAsOfRangePredicate(from, to);

            var current = Context.Query<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            var history = HistoryContext.Query<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            if (orderSelectors.Length > 0) {
                var currentOrdered = OrderByUtils
                    .BuildOrderedQueryable(current, orderSelectors)
                    .ToList();
                var historyOrdered = OrderByUtils
                    .BuildOrderedQueryable(history, orderSelectors)
                    .ToList();
                var orderedUnion = currentOrdered.Union(historyOrdered).ToList();
                return orderedUnion;
            }


            var union = current.ToList().Union(history.ToList()).ToList();

            return union;
        }


        public List<TEntity> QueryAsOf(DateTime asOf,
                Expression<Func<TEntity, bool>> predicate,
                int pageNumber = 1, int pageSize = 10000,
                params Expression<Func<TEntity, dynamic>>[] orderSelectors
                ) {

            var asOfPredicate = GetAsOfBetweenPredicate(asOf);

            var current = Context.Query<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            var history = HistoryContext.Query<TEntity>()
                .Where(predicate)
                .Where(asOfPredicate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking();

            if (orderSelectors.Length > 0) {
                var currentOrdered = OrderByUtils
                    .BuildOrderedQueryable(current, orderSelectors)
                    .ToList();
                var historyOrdered = OrderByUtils
                    .BuildOrderedQueryable(history, orderSelectors)
                    .ToList();
                var orderedUnion = currentOrdered.Union(historyOrdered).ToList();
                return orderedUnion;
            }


            var union = current.ToList().Union(history.ToList()).ToList();

            return union;
        }


        public List<TEntity> GetByIdHistory(params object[] key) {
            var current = Context.Find<TEntity>(key);
            var primaryKeyPredicate = GetPrimaryKeyPredicate(current);

            var history = HistoryContext.Query<TEntity>()
                .Where(primaryKeyPredicate)
                .AsNoTracking()
                .ToList();

            var all = new List<TEntity> { current }.Union(history).ToList();
            return all;
        }



        public List<TEntity> GetByIdAsOf(DateTime asOf, params object[] key) {
            var current = Context.Find<TEntity>(key);
            var primaryKeyPredicate = GetPrimaryKeyPredicate(current);
            var asOfPredicate = GetAsOfBetweenPredicate(asOf);

            var curr = Context.Query<TEntity>()
                .Where(primaryKeyPredicate)
                .Where(asOfPredicate)
                .AsNoTracking()
                .ToList();

            var history = HistoryContext.Query<TEntity>()
                .Where(primaryKeyPredicate)
                .Where(asOfPredicate)
                .AsNoTracking()
                .ToList();

            var all = curr.Union(history).ToList();
            return all;
        }


        private Expression<Func<TEntity, bool>> GetPrimaryKeyPredicate(TEntity entity) {

            var state = Context.Entry(entity);
            var metadata = state.Metadata;
            var primaryKey = metadata.FindPrimaryKey();

            var pe = Expression.Parameter(typeof(TEntity), "e");
            Expression finalExpression = null;

            foreach (var pkProperty in primaryKey.Properties) {
                var type = typeof(TEntity);
                var left = Expression.Property(pe, type.GetProperty(pkProperty.Name));
                var right = Expression.Constant(pkProperty.GetGetter().GetClrValue(entity));
                var eq = Expression.Equal(left, right);
                if (finalExpression != null)
                    finalExpression = Expression.AndAlso(finalExpression, eq);
                else
                    finalExpression = eq;
            }

            var expr = Expression.Lambda<Func<TEntity, bool>>(finalExpression, pe);

            return expr;
        }



        private Expression<Func<TEntity, bool>> GetAsOfBetweenPredicate(DateTime asOf) {

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");


            var left1 = Expression.Property(pe, type.GetProperty("SysStart"));
            var right1 = Expression.Constant(asOf);
            var ge = Expression.LessThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, type.GetProperty("SysEnd"));
            var right2 = Expression.Constant(asOf);
            var le = Expression.GreaterThanOrEqual(left2, right2);


            var between = Expression.AndAlso(ge, le);

            var expr = Expression.Lambda<Func<TEntity, bool>>(between, pe);

            return expr;
        }


        private Expression<Func<TEntity, bool>> GetAsOfRangePredicate(DateTime from, DateTime to) {

            var type = typeof(TEntity);

            var pe = Expression.Parameter(type, "e");


            var left1 = Expression.Property(pe, type.GetProperty("SysStart"));
            var right1 = Expression.Constant(to);
            var ge = Expression.LessThanOrEqual(left1, right1);

            var left2 = Expression.Property(pe, type.GetProperty("SysEnd"));
            var right2 = Expression.Constant(from);
            var le = Expression.GreaterThanOrEqual(left2, right2);

            var between = Expression.AndAlso(ge, le);

            var expr = Expression.Lambda<Func<TEntity, bool>>(between, pe);

            return expr;
        }




    }
}

