using InnorikDemo.Data;
using InnorikDemo.Models;
using InnorikDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innorik_demo.Controllers
{
    //[Authorize]
    [EnableCors("MyPolicy")]
    [Route("")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("[Controller]")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var result = await bookService.GetBooksAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("[Controller]/{name}")]
        public async Task<ActionResult<Book>> GetBookByName(string name)
        {
            var result = await bookService.GetBookByName(name);
            if (result == null)
            {
                return NotFound(nameof(name));
            }

            return Ok(result);
        }

        [HttpPost("[Controller]")]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            var addBook = await bookService.CreateBookAsync(book);
            if (addBook != null)
            {
                return Ok(addBook);
            }
            return BadRequest(book);
        }

        [HttpDelete("[Controller]/{id}")]
        public async Task<ActionResult<Book>> DeleteBookById(int id)
        {
            var result = await bookService.DeleteBookAsync(id);
            if (result == null)
            {
                return NotFound(nameof(id));
            }

            return Ok(result);
        }

        [HttpPut("[Controller]")]
        public async Task<ActionResult<Book>> UpdateBook(Book book)
        {
            var result = await bookService.UpdateBookAsync(book);
            if (result != null)
            {
                return Created("GetBookByName", book);
            }
            return NotFound(book);
        }
    }
}
