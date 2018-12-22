using State.Active;

namespace Scenic
{
    public class LoaderGate : ActiveGate
    {
        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            if (LoadingScene.Instance != null)
                LoadingScene.Instance.OnLoading += _OnLoading;
        }

        public override void Unregister()
        {
            base.Unregister();
            if (LoadingScene.Instance != null)
                LoadingScene.Instance.OnLoading -= _OnLoading;
        }

        private void _OnLoading(object sender, bool e)
        {
            RequestCheck();
        }

        public override bool? Check()
        {
            var loader = LoadingScene.Instance;
            if (loader == null) return null;
            return !loader.IsLoading;
        }
    }
}