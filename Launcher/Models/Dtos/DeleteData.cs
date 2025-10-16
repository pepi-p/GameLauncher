namespace Launcher.Models.Dtos
{
    public class DeleteData : JsonBase
    {
        public int Id { get; set; }

        public DeleteData()
        {
            Type = "DeleteData";
        }
    }
}
