using System;
using Binding;
using UnityEngine;

namespace UI.Performance
{
    [RequireComponent(typeof(Animator))]
    public class FramerateAnimator : MonoBehaviour
    {
        private bool _animating;

        [Bind] private Animator _animator;

        private void Awake()
        {
            this.Bind();
        }

        private void OnEnable()
        {
            if (_animating)
                StencilFramerate.Acquire();
        }

        private void OnDisable()
        {
            if (_animating)
                StencilFramerate.Release();
        }

        private void Update()
        {
            var animating = Animating();
            if (_animating == animating) return;
            _animating = animating;
            if (animating) 
                StencilFramerate.Acquire();
            else 
                StencilFramerate.Release();
        }

        private bool Animating()
        {
            if (_animator.GetCurrentAnimatorClipInfoCount(0) == 0) return false;
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.length == 0f) return false;
            if (state.speed == 0f) return false;
            if (state.speedMultiplier == 0f) return false;
            return state.normalizedTime < 1f;
        }
    }
}