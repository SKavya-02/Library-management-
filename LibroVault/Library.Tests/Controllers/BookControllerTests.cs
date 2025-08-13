using Library.API.Controllers;
using Library.API.DTO;
using Library.API.Interfaces;
using Library.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.Tests.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockService = new Mock<IBookService>();
            _controller = new BookController(_mockService.Object);
        }

        [Fact]
        public async Task AddBook_ReturnsOk_WhenBookIsAdded()
        {
            var dto = new BookDto { ISBN = "123", Title = "Test Book" };
            _mockService.Setup(s => s.GetBookByISBNAsync(dto.ISBN)).ReturnsAsync((Book?)null);

            var result = await _controller.AddBook(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Book added successfully.", okResult.Value);
            _mockService.Verify(s => s.AddBookAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task AddBook_ReturnsBadRequest_WhenBookExists()
        {
            var dto = new BookDto { ISBN = "123" };
            _mockService.Setup(s => s.GetBookByISBNAsync(dto.ISBN)).ReturnsAsync(new Book());

            var result = await _controller.AddBook(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book with this ISBN already exists.", badRequest.Value);
        }

        [Fact]
        public async Task AddBook_Returns500_OnException()
        {
            var dto = new BookDto { ISBN = "123" };
            _mockService.Setup(s => s.GetBookByISBNAsync(dto.ISBN)).ThrowsAsync(new Exception("DB error"));

            var result = await _controller.AddBook(dto);

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
            Assert.Contains("Failed to add book", error.Value.ToString());
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOk_WithBooks()
        {
            _mockService.Setup(s => s.GetAllBooksAsync(It.IsAny<string>(), It.IsAny<int?>()))
                        .ReturnsAsync(new List<Book> { new Book { Title = "Book1" } });

            var result = await _controller.GetAllBooks();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Single(books);
        }

        [Fact]
        public async Task GetAllBooks_Returns500_OnException()
        {
            _mockService.Setup(s => s.GetAllBooksAsync(It.IsAny<string>(), It.IsAny<int?>()))
                        .ThrowsAsync(new Exception("Server error"));

            var result = await _controller.GetAllBooks();

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOk_WhenUpdated()
        {
            var dto = new BookDto { Title = "Updated Book", ISBN = "123" };
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync(new Book());

            var result = await _controller.UpdateBook(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Book updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFound_WhenBookMissing()
        {
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync((Book?)null);

            var result = await _controller.UpdateBook(1, new BookDto());

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book not found.", notFound.Value);
        }

        [Fact]
        public async Task UpdateBook_Returns500_OnException()
        {
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ThrowsAsync(new Exception("Update failed"));

            var result = await _controller.UpdateBook(1, new BookDto());

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }

        [Fact]
        public async Task SoftDeleteBook_ReturnsOk_WhenDeleted()
        {
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync(new Book());

            var result = await _controller.SoftDeleteBook(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Book soft-deleted successfully.", okResult.Value);
        }

        [Fact]
        public async Task SoftDeleteBook_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync((Book?)null);

            var result = await _controller.SoftDeleteBook(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book not found.", notFound.Value);
        }

        [Fact]
        public async Task SoftDeleteBook_Returns500_OnException()
        {
            _mockService.Setup(s => s.GetBookByIdAsync(1)).ThrowsAsync(new Exception("Soft delete error"));

            var result = await _controller.SoftDeleteBook(1);

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }
    }
}
