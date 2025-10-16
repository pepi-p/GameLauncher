using Launcher.Models;
using Launcher.Models.Dtos;

namespace Launcher.Utilities
{
    /// <summary>
    /// サーバー通信用の構造体を変換
    /// </summary>
    public static class DotsConverter
    {
        public static GameMetadata ReadDataToMetadata(ReadData readData)
        {
            return new GameMetadata(
                readData.Id,
                readData.Title,
                readData.Author,
                readData.Version,
                readData.Lastupdate,
                readData.Description,
                readData.ExeName,
                readData.ImgName,
                readData.Tags
                );
        }

        public static GameMetadata CreateDataToMetadata(CreateData createData)
        {
            return new GameMetadata(
                createData.Id,
                createData.Title,
                createData.Author,
                createData.Version,
                createData.Lastupdate,
                createData.Description,
                createData.ExeName,
                createData.ImgName,
                createData.Tags
                );
        }

        public static GameMetadata UpdateDataToMetadata(UpdateData updateData)
        {
            return new GameMetadata(
                updateData.Id,
                updateData.Title,
                updateData.Author,
                updateData.Version,
                updateData.Lastupdate,
                updateData.Description,
                updateData.ExeName,
                updateData.ImgName,
                updateData.Tags
                );
        }
    }
}
