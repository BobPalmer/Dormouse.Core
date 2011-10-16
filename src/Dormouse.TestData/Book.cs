using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dormouse.Core.Repository;

namespace Dormouse.TestData
{
    public interface IBookRepository : IRepositorySearch<Book>
    {
    }

    public class BookPersistence : RepositorySearch<Book, int>, IBookRepository
    {
    }

    public class Book
    {
        public virtual int? BookId { get; set; }
        public virtual string Title { get; set; }
        public virtual int? Condition { get; set; }
        public virtual Guid? BookGuid { get; set; }
    }
}
