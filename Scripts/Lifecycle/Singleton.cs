using System.Linq;
using Lifecycle;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    /// <summary>
    /// Abstract class for making reload-proof singletons out of ScriptableObjects
    /// Returns the asset created on the editor, or null if there is none
    /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public abstract class Singleton<T> : AlwaysScriptableObject where T : ScriptableObject {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                    ((Singleton<T>)(object) _instance)?.OnFirstLoad();
                }
                return _instance;
            }
        }

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        protected virtual void OnSceneLoad(Scene arg0, LoadSceneMode loadSceneMode)
        {
            Debug.Log($"Singleton Awake: {typeof(T).Name}");
            if (Instance == null)
            {
                var _ = ((Singleton<T>) (object) Instance);
            }
        }
        
        protected virtual void OnFirstLoad()
        {}        
    }
}