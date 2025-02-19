using Launcher.Interfaces;
using Launcher.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launcher.Models
{
    public class DataRepository : IDataRepository, IDataRepositoryListener
    {
        public event Action<Dictionary<int, GameMetadata>> OnDataChanged;
        private Dictionary<int, GameMetadata> _datas = new();

        public DataRepository()
        {
            OnDataChanged += (_) => System.Diagnostics.Debug.WriteLine("!!画面更新!!");

            InitLoad();
        }

        private async void InitLoad()
        {
            // Actionが登録されるまで待つ（この処理は要改善かも）
            await Task.Delay(500);

            var result = GameMetadataLoader.LoadAll();
            if (result.IsSuccess)
            {
                foreach (var data in result.Value)
                {
                    _datas.Add(data.Id, data);
                }
            }

            OnDataChanged?.Invoke(_datas);
        }

        public void Registrar(int id, GameMetadata data)
        {
            if (_datas.ContainsKey(id)) _datas.Remove(id);
            _datas.Add(id, data);
            OnDataChanged?.Invoke(_datas);
        }

        public void Unregistrar(int id)
        {
            if (!_datas.ContainsKey(id)) return;
            _datas.Remove(id);
            OnDataChanged?.Invoke(_datas);
        }

        public GameMetadata GetData(int id)
        {
            return _datas[id];
        }
    }
}
