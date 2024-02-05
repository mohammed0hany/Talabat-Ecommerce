using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery,ISpecifictaions<TEntity> spec)
        {
            var query = InputQuery; //query=_dbcontext.set<Product>()


            if (spec.Criteria is not null) //(P=>P.Id==1)
                query = query.Where(spec.Criteria);
                //query=_dbcontext.set<Product>().Where(P=>P.Id==1)

            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDesc is not null)
                query =query.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPagenationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query,(currentQuery,IncludeExpression)=>currentQuery.Include(IncludeExpression));
            //query=_dbcontext.set<Product>().Where(P=>P.Id==1).Include(P=>P.Brand)
            //query=_dbcontext.set<Product>().Where(P=>P.Id==1).Include(P=>P.Brand).Include(P=>P.Category)


            return query;
        }
    }
}
