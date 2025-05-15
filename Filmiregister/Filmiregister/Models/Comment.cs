namespace Filmiregister.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string CommentorName { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public int MovieRating { get; set; }
        public int MovieID { get; set; }
    }
}
