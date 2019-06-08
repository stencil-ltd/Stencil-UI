using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dev
{
    [RequireComponent(typeof(Button))]
    public class CrashButton : MonoBehaviour
    {
        public string message = "Crash Test (dummy)";
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => throw new Exception(message));
        }
    }
}