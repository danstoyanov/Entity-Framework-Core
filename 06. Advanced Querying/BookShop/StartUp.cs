namespace BookShop
{
    using Data;
    using System;
    using Initializer;
    using System.Linq;
    using System.Text;
    using BookShop.Models.Enums;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //   DbInitializer.ResetDatabase(context);

            //  problem 1
            //  Console.WriteLine(GetBooksByAgeRestriction(context, "miNor"));

            //  problem 2
            //  Console.WriteLine(GetGoldenBooks(context));

            //  problem 3
            //  Console.WriteLine(GetBooksByPrice(context));

            //  problem 4
            //  Console.WriteLine(GetBooksNotReleasedIn(context, 2000));

            //  problem 5
            //  Console.WriteLine(GetBooksByCategory(context, "horror mystery drama"));

            //  problem 6
            //  Console.WriteLine(GetBooksReleasedBefore(context, "12-04-1992"));

            //  problem 7
            //  Console.WriteLine(GetAuthorNamesEndingIn(context, "e"));

            //  problem 8
            //  Console.WriteLine(GetBookTitlesContaining(context, "sK"));

            //  problem 9
            //  Console.WriteLine(GetBooksByAuthor(context, "po"));

            //  problem 10
            //  Console.WriteLine(CountBooks(context, 40));

            //  problem 11
            //  Console.WriteLine(CountCopiesByAuthor(context));

            //  problem 12
            //  Console.WriteLine(GetTotalProfitByCategory(context));

            //  problem 13
            //  Console.WriteLine(GetMostRecentBooks(context));

            //  problem 14
            //  IncreasePrices(context);

            //  problem 15
            //  Console.WriteLine(RemoveBooks(context));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var result = new StringBuilder();

            var currBooks = context.Books
                .ToList()
                .Where(b => (b.AgeRestriction).ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b);

            foreach (var book in currBooks)
            {
                result.AppendLine(book);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = EditionType.Gold.ToString();
            var result = new StringBuilder();

            var currBooks = context.Books
                .ToList()
                .Where(b => (b.EditionType.ToString() == goldenBooks) && b.Copies < 5000)
                .Select(b => new { b.Title, b.BookId })
                .OrderBy(b => b.BookId);

            foreach (var book in currBooks)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = new StringBuilder();

            var currBooks = context.Books
                .ToList()
                .Where(b => b.Price > 40)
                .Select(b => new { b.Title, b.Price })
                .OrderByDescending(b => b.Price);

            foreach (var book in currBooks)
            {
                result.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var result = new StringBuilder();

            var currBooks = context.Books
                .ToList()
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new { b.Title, b.BookId })
                .OrderBy(b => b.BookId);

            foreach (var book in currBooks)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var result = new StringBuilder();

            var inputCategories = input.Split().Select(i => i.ToLower()).ToArray();

            var currBooks = context.Books
                .Where(b => b.BookCategories.Any(c => inputCategories.Contains(c.Category.Name.ToLower())))
                .Select(b => new { b.Title })
                .OrderBy(b => b.Title)
                .ToList();

            foreach (var book in currBooks)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var result = new StringBuilder();
            var inputDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var currBooks = context.Books
                .Where(b => b.ReleaseDate < inputDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            foreach (var book in currBooks)
            {
                result.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var result = new StringBuilder();

            var currAutors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToList();

            foreach (var autor in currAutors)
            {
                result.AppendLine(autor.FullName);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var result = new StringBuilder();

            var currBooks = context.Books
                .ToList()
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title);

            foreach (var book in currBooks)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var result = new StringBuilder();

            var currBooks = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    AuthorFullName = b.Author.FirstName + " " + b.Author.LastName
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in currBooks)
            {
                result.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return result.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(b => b.Title.Length > lengthCheck).Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var result = new StringBuilder();

            var currAuthors = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalBookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalBookCopies)
                .ToList();

            foreach (var author in currAuthors)
            {
                result.AppendLine($"{author.FullName} - {author.TotalBookCopies}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var result = new StringBuilder();

            var currCategories = context.Categories.
                Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName)
                .ToList();

            foreach (var category in currCategories)
            {
                result.AppendLine($"{category.CategoryName} ${category.TotalProfit:F2}");
            }

            return result.ToString();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var result = new StringBuilder();

            var currCategories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks.OrderByDescending(b => b.Book.ReleaseDate.Value)
                                           .Take(3)
                                           .Select(b => new
                                           {
                                               BookTitle = b.Book.Title,
                                               ReleaseYear = b.Book.ReleaseDate.Value.Year
                                           })
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            foreach (var category in currCategories)
            {
                result.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    result.AppendLine($"{book.BookTitle} ({book.ReleaseYear})");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var currBooks = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in currBooks)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var currBooks = context.Books.Where(b => b.Copies <= 4200).ToList();
            var result = currBooks.Count();

            context.Books.RemoveRange(currBooks);
            context.SaveChanges();

            return result;
        }
    }
}
