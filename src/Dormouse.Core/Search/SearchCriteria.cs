namespace Dormouse.Core.Search
{
    public class SearchCriteria
    {
        public ComparisonType Compare { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}