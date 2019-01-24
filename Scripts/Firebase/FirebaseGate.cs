using System;
using Init;
using State.Active;

namespace Firebase
{
    public class FirebaseGate : ActiveGate
    {
        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            GameInit.OnFirebaseInit += OnFirebase;
        }

        public override void Unregister()
        {
            base.Unregister();
            GameInit.OnFirebaseInit -= OnFirebase;
        }

        private void OnFirebase(object sender, EventArgs e)
        {
            RequestCheck();
        }

        public override bool? Check()
        {
            return GameInit.FirebaseReady;
        }
    }
}