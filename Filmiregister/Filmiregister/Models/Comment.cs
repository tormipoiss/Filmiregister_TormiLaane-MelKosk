using System.ComponentModel.DataAnnotations;

namespace Filmiregister.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int MovieID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string CommentorName { get; set; }

        [Required(ErrorMessage = "Comment text is required")]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public int Rating { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
