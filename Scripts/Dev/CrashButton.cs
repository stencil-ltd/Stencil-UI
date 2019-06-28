using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dev
{
    [RequireComponent(typeof(Button))]
    public class CrashButton : MonoBehaviour
    {
        public string message = "Crash Test (dummy)";

        private bool _needsCrash;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => _needsCrash = true);
        }

        private void Update()
        {
            if (_needsCrash)
            {
                _needsCrash = false;
                throw new Exception(message);
            }   
        }
    }
}