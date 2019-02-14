using System.IO;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Gestures))]
    public class GestureReport : MonoBehaviour
    {
        public int logSize = 1024;
        
        private void Awake()
        {
            LogCollector.Init(logSize);
            GetComponent<Gestures>().OnGesture += (sender, args) => Report();
        }
        
        private void Report()
        {
            var str = LogCollector.GetLogString();
            var path = Application.temporaryCachePath + "/logs.txt";
            File.WriteAllText(path, str);
            if (!Application.isEditor)
                NativeShare.Share("", path, mimeType: "text/plain", chooser: true);
            else Debug.Log($"Share: {path}");      
        }
    }
}