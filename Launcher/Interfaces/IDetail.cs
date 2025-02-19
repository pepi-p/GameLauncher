using Launcher.Models;

namespace Launcher.Interfaces
{
    public interface IDetail
    {
        public void ShowDetail(GameMetadata data);
        public void HideDetail();
    }
}