using Launcher.Constants;
using Launcher.Models;
using Launcher.Utilities;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Launcher.Services
{
    public static class ImageProvider
    {
        public static Result<BitmapImage> GetImage(GameMetadata data)
        {
            System.Diagnostics.Debug.WriteLine($"ImageProvider: {data.ImgName}");
            var path = Path.Join(FilePaths.GamePath, data.DirName, data.ImgName);

            return GetImage(path);
        }

        public static Result<BitmapImage> GetImage(string path)
        {
            if (!File.Exists(path)) return Result<BitmapImage>.Failure("画像が存在しません");

            try
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                var file = new FileInfo(path);
                img.UriSource = new Uri(file.FullName);
                img.EndInit();

                return Result<BitmapImage>.Success(img);
            }
            catch (UriFormatException e)
            {
                return Result<BitmapImage>.Failure($"Uriが無効です: {e.Message}");
            }
            catch (Exception e)
            {
                return Result<BitmapImage>.Failure(e.Message);
            }
        }
    }
}