using System;
using System.Collections.Generic;
using Common;
using JetBrains.Annotations;
using UnityEngine;
using Util;

namespace State
{
    [Serializable]
    public abstract class StencilData : ScriptableObject, INameable, IIdentifiable
    {
        private static Dictionary<string, StencilData> _idMap 
            = new Dictionary<string, StencilData>();
        
        [Header("Base")]
        [LabelOverride("Id")]
        public string Id;
        public string Name;
        
        public string GetId() => Id;
        public string GetName() => Name;

        public bool HasId => !string.IsNullOrEmpty(Id);

        public static void Reload()
        {
            var old = _idMap;
            _idMap = new Dictionary<string, StencilData>();
            foreach (var stencilData in old.Values)
                stencilData.ReloadId();
        }
        
        [CanBeNull]
        public static StencilData Find(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (!_idMap.ContainsKey(id))
            {
                Debug.LogError($"Cannot find {id}");
                return null;
            }
            return _idMap[id];
        }

        protected virtual void OnEnable()
        {
            ReloadId();
        }

        private void ReloadId()
        {
            if (!HasId) return;
            if (_idMap.ContainsKey(Id))
                Debug.LogError($"Duplicate id {Id}");
//            Debug.Log($"[{Id}]: {Name} ({GetType().ShortName()})");
            _idMap[Id] = this;
        }

        private void OnDisable()
        {
            if (!HasId) return;
            if (_idMap.ContainsKey(Id))
                _idMap.Remove(Id);
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }
}