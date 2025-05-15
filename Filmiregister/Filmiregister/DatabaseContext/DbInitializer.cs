using Filmiregister.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Filmiregister.DatabaseContext
{
    public class DbInitializer
    {
        private class MovieObj
        {
            public bool Adult { get; set; }
            public string Backdrop_path { get; set; }
            public List<int> Genre_ids { get; set; }
            public int Id { get; set; }
            public string Original_language { get; set; }
            public string Original_Title { get; set; }
            public string Overview { get; set; }
            public float Popularity { get; set; }
            public string Poster_path { get; set; }
            public string Release_date { get; set; }
            public string Title { get; set; }
            public bool Video { get; set; }
            public float Vote_average { get; set; }
            public int Vote_count { get; set; }
        }
        private class PopularMoviesJson
        {
            public int Page { get; set; }
            public List<MovieObj> Results { get; set; }
        }

        public async static Task Initialize(MovieContext context)
        {
            try
            {
                if (context.Movies.Any())
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Movie database could not be accessed: {e}");
                return;
            }

            HttpClient _httpClient = new HttpClient();

            try
            {
                var movies = new List<Movie>();
                const string api_key = "91e0d4296bdfc99f07241e1b39b1f41f";
                int nextID = 0;
                for (int i = 0; i <= 5; i++)
                {
                    var popularMoviesResp = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/popular?page={i}&api_key=" + api_key);
                    if (popularMoviesResp.IsSuccessStatusCode)
                    {
                        var content = await popularMoviesResp.Content.ReadAsStringAsync();
                        PopularMoviesJson data = JsonConvert.DeserializeObject<PopularMoviesJson>(content);
                        foreach (MovieObj movieObj in data.Results)
                        {
                            var movie = new Movie();
                            var movieDetailsResp = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{movieObj.Id}?api_key=" + api_key);
                            var detailsContent = await movieDetailsResp.Content.ReadAsStringAsync();
                            JObject detailsData = JObject.Parse(detailsContent);
                            //nextID += 1;
                            //movie.ID = nextID;
                            movie.Categories = detailsData["genres"].Select(g => g["name"].ToString()).ToList();
                            movie.Duration = TimeSpan.FromMinutes(detailsData["runtime"].ToObject<int>());
                            movie.Title = movieObj.Title;
                            movie.Description = movieObj.Overview;
                            movie.Image = "https://image.tmdb.org/t/p/w500" + movieObj.Poster_path;
                            movie.SecondImage = "https://image.tmdb.org/t/p/w500" + movieObj.Backdrop_path;
                            movie.Language = movieObj.Original_language;
                            movie.Rating = movieObj.Vote_average;
                            movie.PublicationDate = DateOnly.Parse(movieObj.Release_date.Replace("-", "/"));
                            // Add the movie to the context
                            context.Movies.Add(movie);
                            Console.WriteLine($"Added movie: {movie}");
                            await context.SaveChangesAsync();

                            Debug.WriteLine($"Movie '{movie.Title}' saved with ID: {movie.ID}");
                            await Task.Delay(200);
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Failed to reach api!");
            }
            finally
            {
                _httpClient.Dispose();
            }
        }
    }
}
