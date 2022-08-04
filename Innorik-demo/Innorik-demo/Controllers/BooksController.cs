using InnorikDemo.Data;
using InnorikDemo.Models;
using InnorikDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innorik_demo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var result = await bookService.GetBooksAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{name}", Name = "GetBookByName")]
        public async Task<ActionResult<Book>> GetBookByName(string name)
        {
            var result = await bookService.GetBookByName(name);
            if (result == null)
            {
                return NotFound(nameof(name));
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            var addBook = await bookService.CreateBookAsync(book);
            if (addBook != null)
            {
                return CreatedAtRoute("GetBookByName", new { book.BookName }, book);
            }
            return BadRequest(book);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBookById(int id)
        {
            var result = await bookService.DeleteBookAsync(id);
            if (result == null)
            {
                return NotFound(nameof(id));
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Book>> UpdateBook(Book book)
        {
            var result = await bookService.UpdateBookAsync(book);
            if (result != null)
            {
                return CreatedAtRoute("GetBookByName", new {book.Id}, book);
            }
            return NotFound(book);
        }
    }
}
