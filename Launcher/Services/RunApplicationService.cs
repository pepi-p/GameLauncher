using Launcher.Utilities;
using System.Diagnostics;
using System.IO;

namespace Launcher.Services
{
    public class RunApplicationService
    {
        public static Result Run(string path)
        {
            if (!File.Exists(path)) return Result.Failure("ファイルが存在しません");

            var app = new ProcessStartInfo();
            app.FileName = path;
            app.UseShellExecute = true;

            Process.Start(app);

            return Result.Success();
        }
    }
}
