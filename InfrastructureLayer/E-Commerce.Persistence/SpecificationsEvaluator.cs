using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entryPoint,ISpecifications<TEntity,TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = entryPoint;

            if(specifications is not null)
            {
                if(specifications.Criteria is not null)
                {
                    Query = Query.Where(specifications.Criteria);
                }
                
                if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    Query = specifications.IncludeExpressions.Aggregate(Query,
                        (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
                }

                if(specifications.OrderBy is not null)
                {
                    Query = Query.OrderBy(specifications.OrderBy);
                }

                if(specifications.OrderByDesc is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDesc);
                }

                if(specifications.IsPaginated)
                {
                    Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                }
            }

            return Query;
        }
    }
}
