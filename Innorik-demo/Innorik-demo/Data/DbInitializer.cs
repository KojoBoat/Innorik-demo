using InnorikDemo.Data;
using InnorikDemo.Models;
using System.Linq;

namespace Innorik_demo.Data
{
    public static class DbInitializer
    {
        public static void InitializeDB(InnorikDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Books.Any())
            {
                return;
            }

            var books = new Book[]
            {
                new Book{BookName="RichManInBabylon",Description="Talks about prosperity",Category="Financial",Price=45},
                new Book{BookName="Know your c#",Description="Code with Mosh",Category="Technology",Price=200},
                new Book{BookName="Crack the coding",Description="Pass Tech interviews with ease",Category="Software",Price=450},
                new Book{BookName="Rich Dad poor Dad",Description="Talks about wealth and investment",Category="Financial",Price=425},
                new Book{BookName="Serendipity",Description="Forex",Category="Financial",Price=45},
            };

            foreach (Book book in books)
            {
                context.Books.Add(book);
            }
            context.SaveChanges();

            var users = new User[]
            {
                new User{FirstName="Christian",LastName="Egyir",Email="test123@mail.com",Password="admintest"},
                new User{FirstName="Ben",LastName="Danso",Email="test1@mail.com",Password="admintest123"},
                new User{FirstName="Grace",LastName="oppong",Email="test23@mail.com",Password="admintest"},
            };

            foreach (User user in users)
            {
                context.Users.Add(user);
            }
        }
    }
}
