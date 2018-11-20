using System;
using JetBrains.Annotations;
using UnityEngine;
using Util;

namespace Lobbing
{    
    [Serializable]
    public class LobStyle
    {
        public float Duration = 0.4f;
        public bool Elastic = true;
        public float Elasticity = 0.5f;

        public static LobStyle Standard => new LobStyle();
    }
    
    [Serializable]
    public struct Lob
    {
        public readonly string Id;
        public readonly long Amount;
        public readonly LobStyle Style;
        public readonly GameObject Projectile;
        
        [CanBeNull] public readonly object Payload;

        public Lob(GameObject projectile, long amount, [CanBeNull] LobStyle style = null, [CanBeNull] object payload = null)
        {
            Id = Guid.NewGuid().ToString();
            Projectile = projectile;
            Amount = amount;
            Payload = payload;
            Style = style ?? LobStyle.Standard;
        }
    }

    [Serializable]
    public class LobOverrides
    {
        public GameObject Projectile;
            
        public bool OverrideStyle;
        public LobStyle Style;

        public bool OverrideDivision;
        public LobDivision Division;

        public Vector3 InitialScale = Vector3.one;

        public Transform From;
        public Transform To;

        [CanBeNull] public Action OnManyComplete;
        [CanBeNull] public Action<Lob> OnEachComplete;

        public void OnComplete(Lob lob) => OnEachComplete?.Invoke(lob);
    }
}