namespace MusicHub
{
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context = new MusicHubDbContext();
            DbInitializer.ResetDatabase(context);

            // Test output with => ExportAlbumsInfo()
            // Console.WriteLine(ExportAlbumsInfo(context, 9));

            // Test output with => ExportSongsAboveDuration()
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var result = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    SongTotalPrice = a.Songs.Sum(a => a.Price),
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        WriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(w => w.WriterName)
                })
                .OrderByDescending(s => s.SongTotalPrice)
                .ToList();

            foreach (var album in albums)
            {
                result.AppendLine($"-AlbumName: {album.AlbumName}")
                      .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                      .AppendLine($"-ProducerName: {album.ProducerName}")
                      .AppendLine($"-Songs:");

                int songCount = 1;

                foreach (var song in album.Songs)
                {
                    result.AppendLine($"---#{songCount++}")
                          .AppendLine($"---SongName: {song.SongName}")
                          .AppendLine($"---Price: {song.SongPrice:F2}")
                          .AppendLine($"---Writer: {song.WriterName}");
                }
                result.AppendLine($"-AlbumPrice: {album.SongTotalPrice:F2}");

                songCount = 0;
            }

            return result.ToString().TrimEnd();
        }
        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var result = new StringBuilder();

            var songs = context.Songs
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    PerformerName = s.SongPerformers
                                     .Select(p => p.Performer.FirstName + " " + p.Performer.LastName)
                                     .FirstOrDefault(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album.Producer.Name,
                    SongDuration = s.Duration
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ThenBy(s => s.PerformerName);

            int songCount = 1;

            foreach (var song in songs)
            {
                result.AppendLine($"-Song #{songCount++}")
                      .AppendLine($"---SongName: {song.SongName}")
                      .AppendLine($"---Writer: {song.WriterName}")
                      .AppendLine($"---Performer: {song.PerformerName}")
                      .AppendLine($"---AlbumProducer: {song.AlbumProducerName}")
                      .AppendLine($"---Duration: {song.SongDuration:c}");
            }

            return result.ToString().TrimEnd();
        }
    }
}