using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Models
{
    public class BorrowedBook
    {
        [Key]
        public int BorrowedBookId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime BorrowedDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; }
        public bool IsLost { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal FineAmount { get; set; }
        public bool IsDeleted { get; set; }

    }
}
