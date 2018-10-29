using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Lobbing
{    
    [Serializable]
    public struct LobStyle
    {
        public float Duration;
        public bool Elastic;

        public static readonly LobStyle Standard = new LobStyle
        {
            Duration = 1f,
            Elastic = true
        };
    }
    
    [Serializable]
    public struct Lob
    {
        public readonly string Id;
        public readonly long Amount;
        public readonly LobStyle Style;
        public readonly GameObject Projectile;
        
        [CanBeNull] public readonly object Payload;

        public Lob(GameObject projectile, long amount, LobStyle? style = null, [CanBeNull] object payload = null)
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
        public bool OverrideStyle;
        public LobStyle Style;

        public bool OverrideDivision;
        public LobDivision Division;
    }
}