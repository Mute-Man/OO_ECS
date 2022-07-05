using NUnit.Framework;
using OO;

namespace Integration_Tests
{
    [TestFixture]
    public class Tests
    {
        private Book[] testLibrary;
        
        private IQuery<Book> bookQuery;

        [SetUp]
        public void Setup()
        {
            testLibrary = JSON.BookReader.readList("hardcoded");
        }

        [Test]
        public void AuthorAnd10Or5()
        {
            bookQuery = new AuthorQuery("ted").and(new PageCountQuery(10).or(new PageCountQuery(5)));
            
            bool[] expected = new bool[] {true, true, false, false};

            bool[] actual = new bool[testLibrary.Length];

            for (int i = 0; i < testLibrary.Length; i++)
            {
                actual[i] = bookQuery.query(testLibrary[i]);
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SerializeDeserialize()
        {
            Book[] inputLibrary = new Book[] {
            new OO.Book {author = "ted", pageCount = 10},
            new OO.Book {author = "ted", pageCount = 5},
            new OO.Book {pageCount = 5},
            default(OO.Book)
            };

            JSON.BookReader.serialize(inputLibrary, "hardcoded");

            Book[] Expected = new Book[] {
            new OO.Book {author = "ted", pageCount = 10},
            new OO.Book {author = "ted", pageCount = 5},
            new OO.Book {pageCount = 5},
            default(OO.Book)
            };

            Book[] Actual = JSON.BookReader.readList("hardcoded");

            Assert.That(Actual, Is.EqualTo(Expected));

        }

        [Test]
        public void SerializeDeserializeDifferent()
        {
            Book[] inputLibrary = new Book[] {
            new OO.Book {author = "ted", pageCount = 10},
            new OO.Book {author = "ted", pageCount = 5},
            new OO.Book {pageCount = 5},
            default(OO.Book)
            };

            JSON.BookReader.serialize(inputLibrary, "hardcoded");

            Book[] UnExpected = new Book[] {
            new OO.Book {author = "ted", pageCount = 10},
            new OO.Book {author = "ted", pageCount = 6},
            new OO.Book {pageCount = 5},
            default(OO.Book)
            };

            Book[] Actual = JSON.BookReader.readList("hardcoded");

            Assert.That(Actual, Is.Not.EqualTo(UnExpected));
        }
    }
}