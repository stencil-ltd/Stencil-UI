using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Standard.Audio;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Random = UnityEngine.Random;

namespace Lobbing
{
    [Serializable]
    public class LobDivision
    {
        public bool ConcreteAmount = false;
        public float AmountPerLob = 0.1f;
        public float RandomizeAmount = 0f;
        
        public float Interval = 0.1f;
        public float RandomizeInterval = 0f;

        public float RandomizeDuration = 0f;
    }
    
    [Serializable]
    public class LobEvent : UnityEvent<Lob>
    {}

    public class Lobber : MonoBehaviour
    {
        public static int ActiveCount = 0;
        
        [Header("General")] 
        public bool ForceToUi;
        public bool ResetLobScale;
        
        [Header("Objects")] 
        public GameObject Prefab;
        public Transform From;
        public Transform To;
        public Transform Container;

        [Header("Particles")] 
        public GameObject FromParticle;
        public GameObject FromParticleSingle;
        public GameObject ToParticle;

        [Header("Style")]
        public LobStyle Flight;
        public LobDivision Division = new LobDivision();

        [Header("Events")] 
        public LobEvent OnLobBegan;
        public LobEvent OnLobEnded;

        [Header("Sfx")] 
        public AudioClip SfxBegin;
        public AudioClip SfxEnd;

        public ILobFunction Function = new ClassicTweenLob();

        public IEnumerator LobSingle(ulong amount, bool applyRandomization = false, LobOverrides overrides = null)
        {
            var projectile = overrides?.Projectile ?? Prefab;
            var from = overrides?.From ?? From;
            var to = overrides?.To ?? To;
            if (overrides?.ReverseDirection == true)
            {
                var tmp = to;
                to = from;
                from = tmp;
            }
            
            var obj = Instantiate(projectile, Container ?? to.parent, true);
            obj.transform.position = from.position;
            if (ResetLobScale) obj.transform.localScale = Vector3.one;
            if (ForceToUi)
                obj.transform.CastIntoUi();
            obj.transform.SetAsLastSibling();
            if (overrides != null)
            {
                var scale = obj.transform.localScale;
                if (overrides.ForceScale != null)
                {
                    scale = overrides.ForceScale.Value;
                }
                scale.Scale(overrides.InitialScale);
                obj.transform.localScale = scale;
            }
            obj.SetActive(true);
            
            var style = Flight;
            if (overrides?.OverrideStyle ?? false)
                style = overrides.Style;

            var division = Division;
            if (overrides?.OverrideDivision ?? false)
                division = overrides.Division;
            
            if (applyRandomization)
            {
                style.Duration += Random.Range(-division.RandomizeDuration, division.RandomizeDuration);
            }
            
            var lob = new Lob(obj, amount, style);
            Begin(lob);
            yield return Objects.StartCoroutine(Function.Lob(lob, from, to));
            End(lob, overrides);
        }

        public IEnumerator LobMany(ulong amount, LobOverrides overrides = null)
        {
            var from = overrides?.From ?? From;
            var to = overrides?.To ?? To;
            if (overrides?.ReverseDirection == true)
            {
                var tmp = to;
                to = from;
                from = tmp;
            }

            var routines = new List<Coroutine>();
            var remaining = amount;
            var division = Division;
            if (overrides?.OverrideDivision ?? false)
                division = overrides.Division;

            if (FromParticleSingle)
            {
                var part = Instantiate(FromParticleSingle, from.position, Quaternion.identity, Container ?? to.parent);
                if (ForceToUi)
                    part.transform.CastIntoUi();
            }
            
            while (remaining > 0L)
            {
                ulong single;
                if (division.ConcreteAmount)
                {
                    single = (ulong) (division.AmountPerLob + 
                                     Random.Range(-division.RandomizeAmount, division.RandomizeAmount));
                }
                else
                {
                    var div = division.AmountPerLob + Random.Range(-division.RandomizeAmount, division.RandomizeAmount);
                    single = (ulong) (div * amount);
                }
                if (single < 0) continue;
                if (single < 1) single = 1; 
                if (single > remaining) single = remaining;
                remaining -= single;
                routines.Add(Objects.StartCoroutine(LobSingle(single, true, overrides)));
                if (remaining > 0)
                {
                    var interval = division.Interval +
                                   Random.Range(-division.RandomizeInterval, division.RandomizeInterval);
                    yield return new WaitForSeconds(interval);
                }
            }

            foreach (var r in routines)
                yield return r;
            
            overrides?.OnManyComplete?.Invoke();
        }

        private void Begin(Lob lob)
        {
            ActiveCount++;
            if (FromParticle != null)
            {
                var part = Instantiate(FromParticle, lob.Projectile.transform.position, Quaternion.identity, lob.Projectile.transform.parent);
                if (ForceToUi)
                    part.transform.CastIntoUi();
            }
            if (SfxBegin != null) SfxOneShot.Instance.Play(SfxBegin);
            OnLobBegan?.Invoke(lob);
        }

        private void End(Lob lob, [CanBeNull] LobOverrides overrides)
        {
            if (ToParticle != null)
            {
                var part = Instantiate(ToParticle, lob.Projectile.transform.position, Quaternion.identity, lob.Projectile.transform.parent);
                if (ForceToUi)
                    part.transform.CastIntoUi();
            }
            
            if (SfxEnd != null) SfxOneShot.Instance.Play(SfxEnd);
            OnLobEnded?.Invoke(lob);
            overrides?.OnComplete(lob);
            Destroy(lob.Projectile);
            ActiveCount--;
        }
    }
}