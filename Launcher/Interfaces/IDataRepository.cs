using Launcher.Models;

namespace Launcher.Interfaces
{
    public interface IDataRepository
    {
        public void Registrar(int id, GameMetadata data);
        public void Unregistrar(int id);
        public GameMetadata GetData(int id);
    }
}
