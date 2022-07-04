
namespace JSON
{
    public static class BookReader
    {
        internal static OO.Book[] hardcodedList = new OO.Book[] {
            new OO.Book {author = "ted", pageCount = 10},
            new OO.Book {author = "ted", pageCount = 5},
            new OO.Book {pageCount = 5},
            default(OO.Book)
        };

        public static OO.Book[] readList(string filepath)
        {
            return hardcodedList;
        }

        public static void serialize(OO.Book[] data, string filepath)
        {
            hardcodedList = data;
        }

    }
}