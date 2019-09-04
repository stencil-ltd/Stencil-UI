using System;
using UnityEngine;

namespace Scripts.Unity
{
    public class ForegroundTracking : MonoBehaviour
    {
        public static event EventHandler<bool> OnForeground;

        private void OnApplicationPause(bool pauseStatus)
        {
            OnForeground?.Invoke(this, !pauseStatus);
        }
    }
}