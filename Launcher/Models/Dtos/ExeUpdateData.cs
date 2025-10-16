namespace Launcher.Models.Dtos
{
    public class ExeUpdateData : JsonBase
    {
        public int Id { get; set; }

        public ExeUpdateData()
        {
            Type = "ExeUpdateData";
        }
    }
}
