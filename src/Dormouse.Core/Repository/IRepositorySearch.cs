using System.Collections.Generic;
using Dormouse.Core.Search;

namespace Dormouse.Core.Repository
{
    public interface IRepositorySearch<T>
        where T : class 
    {
        IList<T> Search(SearchCriteria crit);
        IList<T> Search(AdvancedSearchCriteria crit);
        IList<T> Filter(T filterObj);
    }
}