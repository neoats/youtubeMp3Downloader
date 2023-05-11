using Business.Abstract;
using NAudio.Lame;
using NAudio.Wave;
using YoutubeExplode;

namespace Business.Concrete
{
    public class YoutubeConverter:IConverter
    {
        private readonly YoutubeClient youtube = new YoutubeClient();
        private string selectedFilePath;

        public void SetSelectedFilePath(string filePath)
        {
            selectedFilePath = filePath;
        }

        public  async Task<bool> DownloadVideo(string videoUrl)
        {
            try
            {
                var video = await youtube.Videos.GetAsync(videoUrl);
                var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                var muxedStreamInfo = streamInfoSet.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).FirstOrDefault();

                var outputPath = video.Title + "." + muxedStreamInfo.Container;
                using (var stream = await youtube.Videos.Streams.GetAsync(muxedStreamInfo))
                using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                ConvertToMp3(outputPath, video.Title);
                File.Delete(outputPath);
                
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                return false;
            }
        }

        public  void ConvertToMp3(string inputPath, string title)
        {
            var outputPath = Path.Combine(selectedFilePath, title + ".mp3");
            using (var reader = new AudioFileReader(inputPath))
            using (var writer = new LameMP3FileWriter(outputPath, reader.WaveFormat, 256))
            {
                reader.CopyTo(writer);
            }
        }
    }
}