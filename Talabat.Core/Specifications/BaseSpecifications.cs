using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifictaions<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagenationEnabled { get; set; }


        public void ApplyPagination(int skip,int take)
        {
            IsPagenationEnabled = true;
            Take = take;
            Skip = skip;
        }
        public BaseSpecifications()
        {
        }
        public BaseSpecifications(Expression<Func<T, bool>> _Cretiria)
        {
            Criteria = _Cretiria;
            Includes = new List<Expression<Func<T, object>>>();
        }

        public void OrderBySet(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }        
        public void OrderByDescSet(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
    }
}
