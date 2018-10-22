﻿using System.Linq;
using UnityEngine;

namespace Lifecycle
{
    /// <summary>
    /// Abstract class for making reload-proof singletons out of ScriptableObjects
    /// Returns the asset created on the editor, or null if there is none
    /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public abstract class PlayerSingleton<T> : ScriptableObject where T : ScriptableObject {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                return _instance;
            }
        }
        
        protected virtual void OnPlayerValidate() {}
        protected void OnValidate()
        {
            if (Application.isPlaying) OnPlayerValidate();
        }

        protected virtual void OnPlayerEnable() {}
        protected void OnEnable()
        {
            if (Application.isPlaying) OnPlayerEnable();
        }

        protected virtual void OnPlayerDisable() {}
        protected void OnDisable()
        {
            if (Application.isPlaying) OnPlayerDisable();
        }
        
        protected virtual void OnPlayerDestroy() {}
        protected void OnDestroy()
        {
            if (Application.isPlaying) OnPlayerDestroy();
        }
    }
}