
namespace JSON
{
    public static class Reader<T>
    {
        
        public static OO.Book[] readList()
        {
            return hardcodedList;
        }

        internal static OO.Book[] hardcodedList = new OO.Book[] {
            new OO.Book {author = "ted", pageCount = 10},
            new OO.Book {author = "ted", pageCount = 5},
            new OO.Book {pageCount = 5},
            default(OO.Book)
        };

    }
}