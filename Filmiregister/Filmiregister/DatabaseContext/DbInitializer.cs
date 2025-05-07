using Filmiregister.Models;

namespace Filmiregister.DatabaseContext
{
    public class DbInitializer
    {
        public static void Initialize(MovieContext context)
        {
            if (context.Movies.Any()) return;

            var movies = new Movie[]
            {
                new Movie()
                {
                    MovieID = 1,
                    MovieTitle = "",
                    MovieDescription = "",
                    MovieImage = "",
                    MovieSecondImage = "",
                    MovieRating = 1,
                    Categories = new() {"",""},
                    PublicationDate = new(),
                    MovieLanguage = "en",
                    Duration = new()

                }

            };
        }
    }
}
