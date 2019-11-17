using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Dev
{
    [RequireComponent(typeof(Text))]
    public class FpsText : MonoBehaviour
    {
        public bool ColorCodeForTarget = true;
        
        [Bind] private Text _text;
        float _deltaTime = 0.0f;
        
        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            var fps = (int) (1.0f / _deltaTime);
            _text.text = $"Fps: {fps} [{Application.targetFrameRate}]";
            
            Color color = Color.black;
            switch (Application.targetFrameRate)
            {
                case 30:
                    color = Color.red;
                    break;
                case 60:
                    color = Color.green;
                    break;
                default:
                    color = Color.blue;
                    break;
            }

            _text.color = color;
        }
    }
}