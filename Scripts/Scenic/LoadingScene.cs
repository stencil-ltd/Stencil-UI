using System;
using System.Collections;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenic
{
    public class LoadingScene : Controller<LoadingScene>
    {
        [CanBeNull] 
        private static string _load;
        
        public float loadTime = 5f;
        
        public bool IsLoading { get; private set; }
        public event EventHandler<bool> OnLoading;

        private int _myIndex = -1;
        private int _nextScene = -1;
        private AsyncOperation _scene;

        public static void Load(string name)
        {
            _load = name;
            for (var i = 0; i < SceneManager.sceneCount; i++) 
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            SceneManager.LoadScene(0);
        }
        
        public override void Register()
        {
            base.Register();
            _myIndex = SceneManager.GetActiveScene().buildIndex;
            if (!string.IsNullOrEmpty(_load)) 
                _nextScene = SceneManager.GetSceneByName(_load).buildIndex;
            if (_nextScene < 0) 
                _nextScene = _myIndex + 1;
            _load = null;
        }

        private IEnumerator Start()
        {
            IsLoading = true;
            OnLoading?.Invoke(this, IsLoading);
            yield return null;
            _scene = SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Additive);
            yield return new WaitForSeconds(loadTime);
            if (_scene.isDone)
                StartCoroutine(GameReady());
            else
                _scene.completed += operation => StartCoroutine(GameReady());
        }
        
        private IEnumerator GameReady()
        {
            yield return null;
            SceneManager.UnloadSceneAsync(_myIndex);
            IsLoading = false;
            OnLoading?.Invoke(this, IsLoading);
        }
    }
}