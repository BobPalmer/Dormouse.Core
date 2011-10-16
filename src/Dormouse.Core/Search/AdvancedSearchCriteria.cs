using System.Collections.Generic;

namespace Dormouse.Core.Search
{
    public class AdvancedSearchCriteria
    {
        public IList<SearchCriteria> SearchFilter { get; set; }
        public IList<OrderCriteria> OrderBy { get; set; }
        public int StartingIndex { get; set; }
        public int TotalRecords { get; set; }
    }
}