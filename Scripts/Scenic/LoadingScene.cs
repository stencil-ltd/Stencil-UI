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
        private static int _requestIndex = -1;
        private static bool _preventActivation;
        
        public float loadTime = 5f;
        
        public bool IsLoading { get; private set; }
        public event EventHandler<bool> OnLoading;

        private int _myIndex = -1;
        private int _nextScene = -1;
        private AsyncOperation _scene;

        public static void Load(string name, bool preventActivation)
        {
            Load(SceneManager.GetSceneByName(name).buildIndex, preventActivation);
        }

        public static void Load(int buildIndex, bool preventActivation)
        {
            _preventActivation = preventActivation;
            _requestIndex = buildIndex;
            for (var i = 0; i < SceneManager.sceneCount; i++) 
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            SceneManager.LoadScene(0);
        }
        
        public override void Register()
        {
            base.Register();
            _myIndex = SceneManager.GetActiveScene().buildIndex;
            _nextScene = _requestIndex;
            if (_nextScene < 0) 
                _nextScene = _myIndex + 1;
            _requestIndex = -1;
        }

        private IEnumerator Start()
        {
            IsLoading = true;
            OnLoading?.Invoke(this, IsLoading);
            yield return null;
            _scene = SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Additive);
            if (_preventActivation)
                _scene.allowSceneActivation = false;
            yield return new WaitForSeconds(loadTime);
            _scene.allowSceneActivation = true;
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