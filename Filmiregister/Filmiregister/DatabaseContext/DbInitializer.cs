using Filmiregister.Models;
using System.Globalization;

namespace Filmiregister.DatabaseContext
{
    public class DbInitializer
    {
        public static void Initialize(MovieContext context)
        {
            if (context.Movies.Any()) return;

            var movies = new Movie[]
            {
                new Movie
                {
                    MovieTitle = "Title",
                    MovieDescription = "Description",
                    MovieImage = "https://placehold.co/500x750.png",
                    MovieSecondImage = "https://placehold.co/1280x720.png",
                    MovieRating = 5,
                    Categories = new List<string> { "drama", "action" },
                    PublicationDate =  DateOnly.FromDateTime(DateTime.UtcNow),
                    MovieLanguage = "en",
                    Duration = new TimeOnly(2, 0)
                },
                new Movie // pole film
                {
                    MovieTitle = "The Flash",
                    MovieDescription = "After being struck by lightning, CSI investigator Barry Allen awakens from a nine-month coma to discover he has been granted the gift of super speed. Teaming up with S.T.A.R. Labs, Barry takes on the persona of The Flash, the Fastest Man Alive, to protect his city.",
                    MovieImage = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/yZevl2vHQgmosfwUdVNzviIfaWS.jpg",
                    MovieSecondImage = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/gFkHcIh7iE5G0oVOgpmY8ONQjhl.jpg",
                    MovieRating = 4,
                    Categories = new List<string> { "Drama", "Action" },
                    PublicationDate = DateOnly.ParseExact("14/10/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    MovieLanguage = "en",
                    Duration = new TimeOnly(2, 30)
                }

            };
            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
}
