using System;
using System.Linq;

namespace OO
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read list of books from a json file (currently hardcoded)
            Book[] libraryDatabase = JSON.BookReader.readList("hardcoded");

            // Build queries from user input (currently hardcoded)
            IQuery<Book> query_1 = new AuthorQuery("ted").and(new PageCountQuery(10).or(new PageCountQuery(5))); // Books authored by ted that also have pagecount of either 10 or 5
            IQuery<Book> query_2 = new PageCountQuery(0).not();                                                  // Books that have a non-zero pagecount

            // Look for books that match either query
            foreach (Book book in libraryDatabase)
            {
                if (query_1.query(book))
                    Console.WriteLine("matches query 1 \n" + book + "\n\n");
                if (query_2.query(book))
                    Console.WriteLine("matches query 2 \n" + book + "\n\n");
            }

            // The example displays the following output:
            //      matches query 1 
            //      author: ted 
            //      title: 
            //      pageCount: 10
            //      genre: 
            //      
            //      
            //      matches query 2 
            //      author: ted 
            //      title: 
            //      pageCount: 10
            //
            //
            //      matches query 2 
            //      author:  
            //      title: 
            //      pageCount: 5
            //      genre:
            //
            // 
        }
    }
    public interface IQuery<T> where T : IQueriable
    {
        public bool query(T queriable);

        public IQuery<T> and(IQuery<T> queryB);
        public IQuery<T> or(IQuery<T> queryB);
        public IQuery<T> not();
    }
    public abstract class AQuery<T> : IQuery<T> where T : IQueriable
    {
        public abstract bool query(T queriable);

        public IQuery<T> and(IQuery<T> queryB)
        {
            return new AndQuery<T>(this, queryB);
        }

        public IQuery<T> or(IQuery<T> queryB)
        {
            return new OrQuery<T>(this, queryB);
        }

        public IQuery<T> not()
        {
            return new NotQuery<T>(this);
        }
    }

    public abstract class AComboQuery<T> : AQuery<T> where T : IQueriable
    {
        protected IQuery<T> queryA;
        protected IQuery<T> queryB;
        
        public AComboQuery(IQuery<T> queryA, IQuery<T> queryB)
        {
            this.queryA = queryA;
            this.queryB = queryB;
        }
    }

    sealed class AndQuery<T> : AComboQuery<T> where T : IQueriable
    {

        public override bool query(T queriable)
        {
            return queryA.query(queriable) && queryB.query(queriable);
        }

        public AndQuery(IQuery<T> queryA, IQuery<T> queryB) : base(queryA, queryB)
        {}
    }

    sealed class OrQuery<T> : AComboQuery<T> where T : IQueriable
    {
        public override bool query(T queriable)
        {
            return queryA.query(queriable) || queryB.query(queriable);
        }

        public OrQuery(IQuery<T> queryA, IQuery<T> queryB) : base(queryA, queryB)
        {}
    }

    sealed class NotQuery<T> : AQuery<T> where T: IQueriable
    {
        private IQuery<T> negatedQuery;

        public override bool query(T queriable)
        {
            return !negatedQuery.query(queriable);
        }

        public NotQuery (IQuery<T> negatedQuery)
        {
            this.negatedQuery = negatedQuery;
        }
    }

    public abstract class BookQuery<T> : AQuery<Book>
    {
        protected T queryData;
        public BookQuery(T queryData)
        {
            this.queryData = queryData;
        }
    }

    public class AuthorQuery : BookQuery<string>
    {
        public override bool query(Book queriable)
        {
            return queriable.author == this.queryData;
        }

        public AuthorQuery(String queryString) : base(queryString) {}
    }

    public class PageCountQuery : BookQuery<uint>
    {
        public override bool query(Book queriable)
        {
            return queriable.pageCount == this.queryData;
        }

        public PageCountQuery(uint queryNum) : base(queryNum) {}
    }

    public class TitleQuery : BookQuery<string>
    {
        public override bool query(Book queriable)
        {
            return queriable.title == this.queryData;
        }

        public TitleQuery(String queryString) : base(queryString) {}
    }

    public class GenreQuery : BookQuery<string[]>
    {
        public override bool query(Book queriable)
        {
            return queriable.genre.SequenceEqual(this.queryData);
        }

        public GenreQuery(String[] queryString) : base(queryString) {}
    }
    public interface IQueriable
    {}
    public struct Book : IQueriable, IEquatable<Book>
    {
        public String author;
        public String title;
        public uint pageCount;
        public String[] genre;

        public override string ToString()
        {

            // need to account for null values
            return
                $"\n" + 
                $"author: {author ?? ""}\n" + 
                $"title: {title ?? ""}\n" + 
                $"pageCount: {pageCount}\n" + 
                "genre: " + ((genre == null) ? "" : $"{String.Join(",", genre)}") + 
                $"\n";
        }

        public bool Equals(Book book)
        {
            if (this.genre?.Length != book.genre?.Length) 
                    return false;

            if (this.genre != null && book.genre != null)
            {
                foreach (System.Tuple<String, String> s in this.genre.Zip(book.genre, Tuple.Create))
                {
                    if (s.Item1 != s.Item2)
                        return false;
                }
            }

            if (this.author != book.author
                || this.pageCount != book.pageCount
                || this.title != book.title
            )
                return false;

            return true;
        }
    }
}
