using System;
using System.Linq;

namespace OO
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
        
        interface IQuery<T> where T : IQueriable
        {
            public bool query(T queriable);

            public IQuery<T> and(IQuery<T> queryB);
            public IQuery<T> or(IQuery<T> queryB);
            public IQuery<T> not();
        }
        abstract class AQuery<T> : IQuery<T> where T : IQueriable
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

        abstract class AComboQuery<T> : AQuery<T> where T : IQueriable
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

        abstract class BookQuery<T> : AQuery<Book>
        {
            protected T queryData;
            public BookQuery(T queryData)
            {
                this.queryData = queryData;
            }
        }

        class AuthorQuery : BookQuery<string>
        {
            public override bool query(Book queriable)
            {
                return queriable.author == this.queryData;
            }

            public AuthorQuery(String queryString) : base(queryString) {}
        }

        class PageCountQuery : BookQuery<uint>
        {
            public override bool query(Book queriable)
            {
                return queriable.pageCount == this.queryData;
            }

            public PageCountQuery(uint queryNum) : base(queryNum) {}
        }

        class TitleQuery : BookQuery<string>
        {
            public override bool query(Book queriable)
            {
                return queriable.title == this.queryData;
            }

            public TitleQuery(String queryString) : base(queryString) {}
        }

        class GenreQuery : BookQuery<string[]>
        {
            public override bool query(Book queriable)
            {
                return queriable.genre.SequenceEqual(this.queryData);
            }

            public GenreQuery(String[] queryString) : base(queryString) {}
        }
    }
    public interface IQueriable
    {}
    public struct Book : IQueriable
    {
        public String author;
        public String title;
        public uint pageCount;
        public String[] genre;

        public override string ToString()
        {
            return 
                $"author: {author} \n" +
                $"title: {title}\n" +
                $"pageCount: {pageCount}\n" +
                $"genre: {genre}";
        }
    }
}
