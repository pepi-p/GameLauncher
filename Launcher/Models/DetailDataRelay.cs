using Launcher.Interfaces;
using System;

namespace Launcher.Models
{
    public class DetailDataRelay : IDetail, IDetailListener
    {
        public event Action<GameMetadata> OnDetailShow;
        public event Action OnDetailHide;
        private bool _isShow;

        public void ShowDetail(GameMetadata data)
        {
            if (_isShow) return;
            OnDetailShow.Invoke(data);
            _isShow = true;
        }

        public void HideDetail()
        {
            if (!_isShow) return;
            OnDetailHide.Invoke();
            _isShow = false;
        }
    }
}