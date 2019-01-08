using Binding;
using Dirichlet.Numerics;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class BigNumberText : MonoBehaviour
    {
        [Header("UI")] 
        public NumberFormats.Format format = NumberFormats.Format.MassiveAmount;
        public string prefix = "";
        
        [Bind]
        private Text _text;
        
        public UInt128 Amount { get; private set; }

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _text.text = prefix + format.FormatAmount(Amount);
        }
    }
}