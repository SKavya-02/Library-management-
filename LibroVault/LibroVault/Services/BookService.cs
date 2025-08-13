using Library.API.Interfaces;
using Library.API.Models;

namespace Library.API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(string? search, int? categoryId)
        {
            var books = await _repo.GetAllBooksAsync();

            if (!string.IsNullOrWhiteSpace(search))
                books = books.Where(b => b.Title.Contains(search) || b.Author.Contains(search));

            if (categoryId.HasValue)
                books = books.Where(b => b.CategoryId == categoryId.Value);

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _repo.GetBookByIdAsync(id);
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn)
        {
            return await _repo.GetBookByISBNAsync(isbn);
        }

        public async Task AddBookAsync(Book book)
        {
            await _repo.AddBookAsync(book);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _repo.UpdateBookAsync(book);
            await _repo.SaveChangesAsync();
        }

        public async Task SoftDeleteBookAsync(Book book)
        {
            book.IsDeleted = true;
            await _repo.UpdateBookAsync(book);
            await _repo.SaveChangesAsync();
        }
    }
}
