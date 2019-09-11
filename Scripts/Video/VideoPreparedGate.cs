using System;
using State.Active;
using UnityEngine.Video;

namespace DefaultNamespace
{
    public class VideoPreparedGate : ActiveGate
    {
        public VideoPlayer player;
        private bool _active = false;
        
        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            _active = player.isPrepared;
            player.prepareCompleted += OnPrepare;
        }

        private void OnPrepare(VideoPlayer source)
        {
            _active = source.isPrepared;
            RequestCheck();
        }

        public override void Unregister()
        {
            base.Unregister();
            player.prepareCompleted -= OnPrepare;
        }

        public override bool? Check()
        {
            return _active;
        }
    }
}