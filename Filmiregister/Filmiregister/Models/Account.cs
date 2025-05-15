namespace Filmiregister.Models
{
    public class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> ChosenMovies { get; set; }
    }
}
