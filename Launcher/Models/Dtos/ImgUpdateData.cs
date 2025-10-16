namespace Launcher.Models.Dtos
{
    public class ImgUpdateData : JsonBase
    {
        public int Id { get; set; }

        public ImgUpdateData()
        {
            Type = "ImgUpdateData";
        }
    }
}
