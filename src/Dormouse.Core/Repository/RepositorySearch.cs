using System.Collections.Generic;
using Dormouse.Core.Search;

namespace Dormouse.Core.Repository
{
    public abstract class RepositorySearch<TD, TK> 
        : RepositoryCRUD<TD, TK>
          , IRepositorySearch<TD>
        where TD : class
    {
        public IList<TD> Search(AdvancedSearchCriteria search)
        {
            return Search(search.SearchFilter, search.OrderBy, search.StartingIndex, search.TotalRecords);
        }
        
        private IList<TD> Search(IList<SearchCriteria> critList, IList<OrderCriteria> orderCritList, int startingIndex, int totalRecords)
        {
            var rlist = new List<TD>();
            using (CurrentSession.BeginTransaction())
            {
                var q = CurrentSession.CreateCriteria(typeof(TD));
                if (critList != null)
                {
                    q = Utilities.SetComplexSearchCriteria(critList, q);
                }
                if (orderCritList != null)
                {
                    q = Utilities.OrderComplexSearch(orderCritList, q);
                }
                if (totalRecords > 0)
                {
                    q.SetFirstResult(startingIndex);
                    q.SetMaxResults(totalRecords);
                }
                rlist = q.List<TD>() as List<TD>;
            }
            return rlist;
        }

        public IList<TD> Search(SearchCriteria crit)
        {
            var critList = new List<SearchCriteria>();
            critList.Add(crit);
            return Search(critList,null,0,0);
        }

        public IList<TD> Filter(TD criteriaobj)
        {
            var rlist = new List<TD>();
            using (CurrentSession.BeginTransaction())
            {
                var q = CurrentSession.CreateCriteria(typeof(TD));
                q = Utilities.SetSearchCriteria(criteriaobj, q);
                rlist = q.List<TD>() as List<TD>;
            }
            return rlist;
        }

    }
}