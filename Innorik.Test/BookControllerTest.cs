using Innorik_demo.Controllers;
using InnorikDemo.Models;
using InnorikDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Innorik.Test
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> mockBookService;
        public BookControllerTest()
        {
            mockBookService = new Mock<IBookService>();
        }
        [Fact]
        public async void GetAllBooks_Success()
        {
            var mockOrder = new List<Book>
            {
                Mock.Of<Book>(book => book.BookName == "Test Book 1" && book.Price == 25 && book.Category == "Test Category"
                && book.Description == "Test Description" && book.Id == 1),
                Mock.Of<Book>(book => book.BookName == "Test Book 2" && book.Price == 35 && book.Category == "Test Category 2"
                && book.Description == "Test Description 2" && book.Id == 2)
            };
            var mockBooksController = new BooksController(mockBookService.Object);
            mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(mockOrder);

            var result = await mockBooksController.GetAllBooks();
            var actual = result.Result;

            Assert.NotNull(actual);
            Assert.IsType<OkObjectResult>(actual);
            mockBookService.Verify();
            Assert.Equal(2, mockOrder.Count);
        }

        [Fact]
        public async void GetAllBooks_Failure()
        {
            var mockOrder = new List<Book>();
            mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(mockOrder);
            var mockBooksController = new BooksController(mockBookService.Object);

            var result = await mockBooksController.GetAllBooks();

            Assert.Empty(mockOrder);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetBookByName_Success()
        {
            var expectedBook = Mock.Of<Book>(book => book.Id == 1 && book.Category == "Test Category" 
            && book.BookName == "Test Name" && book.Description == "Test Description" && book.Price == 35);

            mockBookService.Setup(s => s.GetBookByName(It.IsAny<string>())).ReturnsAsync(expectedBook);
            var mockBookController = new BooksController(mockBookService.Object);

            var result = await mockBookController.GetBookByName(expectedBook.BookName);

            var actualResult = (OkObjectResult)result.Result;
            var actualValue = (Book)actualResult.Value;

            Assert.Equal(expectedBook.BookName, actualValue.BookName);
        }

        [Fact]
        public async void GetBookByName_NotFound()
        {
            var bookName = "test-name";
            mockBookService.Setup(s => s.GetBookByName(It.IsAny<string>())).Returns(Task.FromResult<Book>(null));

            var mockBookController = new BooksController(mockBookService.Object);
            var actual = await mockBookController.GetBookByName(bookName);

            Assert.IsType<NotFoundObjectResult>(actual.Result);
        }

        [Fact]
        public async void DeleteBookbyId_Success()
        {
            var mockBooks = new List<Book> {
                Mock.Of<Book>(book => book.Id == 1 && book.BookName == "Test-Name"
                && book.Category == "Test-Category" && book.Description == "Test Description" && book.Price == 45),
        };
            var mockBookController = new BooksController(mockBookService.Object);
            mockBookService.Setup(s => s.DeleteBookAsync(It.IsAny<int>())).Returns(Task.FromResult(mockBooks[0]));

            var result = await mockBookController.DeleteBookById(1);
            var actual = (OkObjectResult)result.Result;
            var actualValue = (Book)actual.Value;

            Assert.Collection(mockBooks,s =>
            {
                Assert.Equal(s.Id, actualValue.Id);
            });
            mockBookService.Verify(x => x.DeleteBookAsync(1));
        }

        [Fact]
        public async void DeleteBookById_Failure()
        {
            var bookId = 1;
            mockBookService.Setup(s => s.DeleteBookAsync(It.IsAny<int>())).Returns(Task.FromResult<Book>(null));

            var mockBookController = new BooksController(mockBookService.Object);
            var actual = await mockBookController.DeleteBookById(bookId);

            Assert.IsType<NotFoundObjectResult>(actual.Result);
        }
    }
}
