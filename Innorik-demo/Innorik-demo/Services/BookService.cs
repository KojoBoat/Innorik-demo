using InnorikDemo.Data;
using InnorikDemo.Models;
using InnorikDemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnorikDemo.Services
{
    public class BookService : IBookService
    {
        private readonly InnorikDbContext _context;
        private readonly ILogger<BookService> _logger;
        public BookService(InnorikDbContext context, ILogger<BookService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            try
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occured trying to insert book");
            }
            return await Task.FromResult(book);
        }

        public async Task<Book> DeleteBookAsync(int Id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == Id);
            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error trying to delete book resource");
            }
            return await Task.FromResult(book);
        }

        public async Task<Book> GetBookByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            return await _context.Books.FirstOrDefaultAsync(book => book.BookName == name);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            if (bookToUpdate == null)
            {
                throw new ArgumentNullException($"Book not found: {bookToUpdate.Id}");
            }

            bookToUpdate.Description = book.Description;
            bookToUpdate.Category = book.Category;
            bookToUpdate.BookName = book.BookName;
            bookToUpdate.Price = book.Price;

            await _context.SaveChangesAsync();
            return await Task.FromResult(bookToUpdate);
        }
    }
}
