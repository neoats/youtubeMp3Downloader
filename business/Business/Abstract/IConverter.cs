namespace Business.Abstract;

public  interface IConverter
{
   Task<bool> DownloadVideo(string videoUrl);
   void ConvertToMp3(string inputPath, string title);
   void SetSelectedFilePath(string filePath);
}