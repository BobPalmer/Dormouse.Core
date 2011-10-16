namespace Dormouse.Core.Search
{
    public enum ComparisonType
    {
        NotEqual,
        Equals,
        GreaterThan,
        GreaterThanOrEqualTo,
        EqualsOrNull,
        GreaterThanOrNull,
        InGUID,
        InInt,
        InString,
        IsNull,
        IsNotNull,
        LessThan,
        LessThanOrEqualTo,
        LessThanOrNull,
        Like,
        LikeAnywhere,
        LikeEndWith,
        LikeStartWith,
        NotEqualsOrNull,
        NotInInt,
        SqlExp
    }
}