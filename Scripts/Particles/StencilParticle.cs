using System.Linq;
using Binding;
using Standard.States;
using State;
using UnityEngine;

namespace Ui.Particles
{
    [RequireComponent(typeof(ParticleSystem))]
    [ExecuteInEditMode]
    public class StencilParticle : MonoBehaviour
    {
        public enum DoOnFinish
        {
            Nothing,
            Deactivate,
            Destroy
        }
        
        public DoOnFinish onFinish = DoOnFinish.Destroy;
        public PlayStates.State[] activeStates = { };
        
        [Bind]
        public ParticleSystem ParticleSystem { get; set; }
        private ParticleSystem[] _particles;

        public void Play()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                return;
            }
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Stop()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        private void Awake()
        {
            this.Bind();
            _particles = GetComponentsInChildren<ParticleSystem>();
            if (PlayStates.Instance != null && Application.isPlaying && activeStates.Length > 0)
            {
                PlayStates.Instance.OnChange += OnState;
                OnState(null, new StateChange<PlayStates.State>(null, PlayStates.Instance.State));
            }
        }

        private void OnDestroy()
        {
            if (PlayStates.Instance != null && Application.isPlaying)
                PlayStates.Instance.OnChange -= OnState;
        }

        private void OnState(object sender, StateChange<PlayStates.State> e)
        {
            gameObject.SetActive(activeStates.Contains(e.New));   
        }

        public void SetColor(Color color)
        {
            foreach (var particle in _particles)
            {
                var main = particle.main;
                main.startColor = color;
            }
        }

        private void Start()
        {
            var top = _particles.FirstOrDefault();
            foreach (var particle in _particles)
            {
                particle.GetComponent<Renderer>().sortingLayerName = "Particles";
                var main = particle.main;
                if (Application.isPlaying)
                    main.startColor = top.main.startColor;
            }
        }

        private void Update()
        {
            if (!Application.isPlaying) return;
            if (ParticleSystem.isStopped)
                PerformFinish();
        }

        private void OnDisable()
        {
            PerformFinish();
        }

        private void PerformFinish()
        {
            if (!Application.isPlaying) return;
            switch (onFinish)
            {
                case DoOnFinish.Deactivate:
                    gameObject.SetActive(false);
                    break;
                case DoOnFinish.Destroy:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}