using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Dormouse.Core;
using Dormouse.Core.Search;
using Dormouse.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace Dormouse.Tests
{
    [TestClass]
    public class When_working_with_repository_instances
    {
        [TestMethod]
        public void Should_create_a_repository_with_default_constructor()
        {
            //Arrange
            //Act
            var repo = new BookPersistence();
            //Assert
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void Should_properly_dispose_of_sessions()
        {
            //Arrange
            ISession s;
            //Act
            using (var repo = new BookPersistence())
            {
                s = repo.CurrentSession;
            }
            //Assert
            Assert.IsFalse(s.IsConnected);
        }
    }

    [TestClass]
    public class When_manipulating_records : With_Book_Context
    {
        [TestMethod]
        public void Should_be_able_to_create_a_new_record()
        {
            //Arrange
            var book = new Book { Title = "The Silmarillion" };
            var expected = 5;
            //Act
            _repo.Create(book);
            var books = _repo.Search(new AdvancedSearchCriteria());
            var actual = books.Count();
            _repo.Delete(book);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_be_able_to_update_an_existing_record()
        {
            //Arrange
            var expected = "The Hobbit";
            //Act
            var book = _repo.Get(32768);
            var actual = book.Title;
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class When_searching_records : With_Book_Context
    {
        [TestMethod]
        public void Should_be_able_to_filter_records()
        {
            //Arrange
            var expected = 2;
            var book = new Book() { Condition = 1 };
            //Act
            var books = _repo.Filter(book);
            var actual = books.Count;
            //Assert 
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_be_able_to_Search_and_Sort()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            advCrit.SearchFilter = new List<SearchCriteria>();
            advCrit.OrderBy = new List<OrderCriteria> { new OrderCriteria { PropertyName = "Title" } };
            advCrit.StartingIndex = 2;
            advCrit.TotalRecords = 1;
            var expectedTitle = "The Hobbit";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Sort_Descending()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            advCrit.SearchFilter = new List<SearchCriteria>();
            advCrit.OrderBy = new List<OrderCriteria> { new OrderCriteria { PropertyName = "Title", Order = OrderType.DESC } };
            advCrit.StartingIndex = 0;
            advCrit.TotalRecords = 2;
            var expectedTitle = "The Two Towers";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }
    }

    [TestClass]
    public class When_exercising_search_comparisons : With_Book_Context
    {
        [TestMethod]
        public void Should_be_able_to_Search_by_NotEquals()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.NotEqual;
            crit.PropertyName = "Title";
            crit.Value = "The Hobbit";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 3;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_Equals()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.Equals;
            crit.PropertyName = "Title";
            crit.Value = "The Hobbit";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_GreaterThan()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.GreaterThan;
            crit.PropertyName = "Condition";
            crit.Value = 1;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_GreaterThanOrEqualTo()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.GreaterThanOrEqualTo;
            crit.PropertyName = "Condition";
            crit.Value = 2;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_GreaterThanOrNull()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.GreaterThanOrNull;
            crit.PropertyName = "Condition";
            crit.Value = 1;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_LessThan()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.LessThan;
            crit.PropertyName = "Condition";
            crit.Value = 2;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_LessThanOrEqualTo()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.LessThanOrEqualTo;
            crit.PropertyName = "Condition";
            crit.Value = 2;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 3;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_LessThanOrNull()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.LessThanOrNull;
            crit.PropertyName = "Condition";
            crit.Value = 1;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Return of the King";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_Like()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.Like;
            crit.PropertyName = "Title";
            crit.Value = "%The%";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 4;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_LikeStartsWith()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.LikeStartWith;
            crit.PropertyName = "Title";
            crit.Value = "The";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_LikeEndsWith()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.LikeEndWith;
            crit.PropertyName = "Title";
            crit.Value = "g";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_LikeAnywhere()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.LikeAnywhere;
            crit.PropertyName = "Title";
            crit.Value = "The";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 4;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_EqualsOrNull()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.EqualsOrNull;
            crit.PropertyName = "Condition";
            crit.Value = 1;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 3;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_NotEqualsOrNull()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.NotEqualsOrNull;
            crit.PropertyName = "Condition";
            crit.Value = 1;
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_IsNull()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.IsNull;
            crit.PropertyName = "Condition";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Return of the King";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_IsNotNull()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.IsNotNull;
            crit.PropertyName = "Condition";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 3;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_InString()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.InString;
            crit.PropertyName = "Title";
            crit.Value = new List<String> { "The Hobbit", "Starship Troopers", "The Two Towers" };
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_InInt()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.InInt;
            crit.PropertyName = "Condition";
            crit.Value = new List<int> { 2, 3, 4 };
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "Fellowship of the Ring";
            var expectedRecords = 1;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_NotInInt()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.NotInInt;
            crit.PropertyName = "Condition";
            crit.Value = new List<int> { 2, 3, 4 };
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_InGuid()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.InGUID;
            crit.PropertyName = "BookGuid";
            crit.Value = new List<Guid> { new Guid("11111111-1111-1111-1111-111111111111"), new Guid(), new Guid("33333333-3333-3333-3333-333333333333") };
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [TestMethod]
        public void Should_be_able_to_Search_by_SQL()
        {
            //arrange
            var advCrit = new AdvancedSearchCriteria();
            var crit = new SearchCriteria();
            crit.Compare = ComparisonType.SqlExp;
            crit.Value = "Condition = 1";
            advCrit.SearchFilter = new List<SearchCriteria> { crit };
            advCrit.OrderBy = new List<OrderCriteria>();

            var expectedTitle = "The Hobbit";
            var expectedRecords = 2;
            //act
            var books = _repo.Search(advCrit);
            var actualTitle = books[0].Title;
            var actualRecords = books.Count;
            //assert
            Assert.AreEqual(expectedRecords, actualRecords);
            Assert.AreEqual(expectedTitle, actualTitle);
        }
    }

    [TestClass]
    public abstract class With_Book_Context
    {
        protected static BookPersistence _repo;

        [AssemblyInitialize]
        public static void TestSetup(TestContext context)
        {
            Utilities.CreateSchema();
            _repo = new BookPersistence();
            _repo.Create(new Book { Title = "The Hobbit", Condition = 1, BookGuid = new Guid("11111111-1111-1111-1111-111111111111") });
            _repo.Create(new Book { Title = "Fellowship of the Ring", Condition = 2 });
            _repo.Create(new Book { Title = "The Two Towers", Condition = 1, BookGuid = new Guid("22222222-2222-2222-2222-222222222222") });
            _repo.Create(new Book { Title = "Return of the King", Condition = null, BookGuid = new Guid("33333333-3333-3333-3333-333333333333") });
        }
    }

    [TestClass]
    public class When_handling_a_shared_session : With_Book_Context
    {
        [TestMethod]
        public void Should_be_able_to_execute_multiple_read_queries()
        {
            //arrange
            var expectedTitle1 = "The Two Towers";
            var expectedTitle2 = "Return of the King";
            //act
            var b1 = _repo.Get(32770);
            var b2 = _repo.Get(32771);
            var actualTitle1 = b1.Title;
            var actualTitle2 = b2.Title;
            //assert 
            Assert.AreEqual(expectedTitle1, actualTitle1);
            Assert.AreEqual(expectedTitle2, actualTitle2);
        }

        [TestMethod]
        public void Should_be_able_to_share_session()
        {
            //arrange
            var b1 = new Book { Title = "Gulliver's Travels" };
            var expectedStart = 5;
            var expectedEnd = 4;
            //act
            _repo.Create(b1);
            var actualStart = _repo.Search(new AdvancedSearchCriteria()).Count;
            _repo.Delete(b1);
            var actualEnd = _repo.Search(new AdvancedSearchCriteria()).Count;
            //assert 
            Assert.AreEqual(expectedStart, actualStart);
            Assert.AreEqual(expectedEnd, actualEnd);
        }

        [TestMethod]
        public void Should_be_able_to_execute_read_and_write_queries_to_same_session()
        {
            //arrange
            var b1 = new Book { Title = "The Silmarillion" };
            var crit = new SearchCriteria
            {
                Compare = ComparisonType.Equals,
                PropertyName = "Title",
                Value = "The Silmarillion"
            };
            //act
            _repo.Create(b1);
            var b2 = _repo.Search(crit)[0];
            _repo.Delete(b1);
            //assert 
            Assert.AreEqual(b1.Title, b2.Title);
        }
    }
}
