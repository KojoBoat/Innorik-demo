using InnorikDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnorikDemo.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByName(string name);
        Task<Book> UpdateBookAsync(Book book);
        Task<Book> CreateBookAsync(Book book);
        Task<Book> DeleteBookAsync(int Id);
    }
}
