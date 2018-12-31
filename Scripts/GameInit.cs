using System;
using System.Collections;
using Ads;
using Analytics;
using CustomOrder;
using Plugins.UI;
using Scripts.RemoteConfig;
using Store;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

#if !EXCLUDE_FIREBASE
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Messaging;
#endif

#if !EXCLUDE_FACEBOOK
using Facebook.Unity;
#endif

namespace Init
{
    [ExecutionOrder(-100)]
    public class GameInit : Permanent<GameInit>
    {
        public bool useLocation = true;
        
        public bool Started { get; private set; }
        public static bool FirebaseReady;

        #if !EXCLUDE_FACEBOOK
        public static event EventHandler OnFacebookInit;
        #endif
        
        protected sealed override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
            var _ = Tracking.Instance;
            gameObject.AddComponent<Gestures>();
            gameObject.AddComponent<GestureReport>();
            new GameObject("Main Thread Dispatch").AddComponent<UnityMainThreadDispatcher>();
            BuyableManager.Init();
            SceneManager.sceneLoaded += _OnNewScene;
            StartCoroutine(SetupLocation());
            SetupFirebase();
            SetupFacebook();
            StencilAds.Init();
            OnInit();
        }

        private IEnumerator SetupLocation()
        {
            if (!useLocation) yield break;
            Input.location.Start();
            yield return null;
            Input.location.Stop();
        }

        private static void SetupFacebook()
        {
#if !EXCLUDE_FACEBOOK
            FB.Mobile.FetchDeferredAppLinkData();
            FB.Init(() =>
            {
                Debug.Log($"Facebook init: {FB.IsInitialized} [authed={FB.IsLoggedIn}]");
                FB.ActivateApp();
                OnFacebookInit?.Invoke();
            });
#endif
        }

        private void SetupFirebase()
        {
#if !EXCLUDE_FIREBASE
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                var success = dependencyStatus == DependencyStatus.Available;
                if (!success)
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                FirebaseReady = success;

                Objects.Enqueue(() =>
                {
                    if (success)
                    {
                        var settings = FirebaseRemoteConfig.Settings;
                        settings.IsDeveloperMode = !StencilRemote.IsProd();
                        FirebaseRemoteConfig.Settings = settings;
                        var cache = settings.IsDeveloperMode ? TimeSpan.Zero : TimeSpan.FromHours(StencilRemote.CacheHours);
                        FirebaseRemoteConfig.FetchAsync(cache).ContinueWith(task1 =>
                        {
                            if (task1.IsFaulted)
                            {
                                Debug.LogError($"Firebase Remote Config failed. {task1.Exception?.InnerException.Message}");
                                return;
                            }
                            FirebaseRemoteConfig.ActivateFetched();
                            Objects.Enqueue(StencilRemote.NotifyRemoteConfig);
                        });
                        SetupPush();
                    }
                    OnFirebase(success);
                });
            });
#endif
        }
        

#if !EXCLUDE_FIREBASE
        private void SetupPush()
        {
            FirebaseMessaging.RequestPermissionAsync();
            if (StencilRemote.IsDeveloper())
                FirebaseMessaging.SubscribeAsync("dev/remote");
            else 
                FirebaseMessaging.UnsubscribeAsync("dev/remote");
        }
#endif

        private void _OnNewScene(Scene arg0, LoadSceneMode arg1)
        {
            StencilAds.CheckReload();
            OnNewScene(arg0, arg1);
        }

        protected virtual void OnFirebase(bool success)
        {}

        protected virtual void OnInit()
        {}

        protected virtual void OnSettled()
        {}

        protected virtual void OnNewScene(Scene arg0, LoadSceneMode loadSceneMode)
        {}

        protected virtual IEnumerator Start()
        {
            Started = true;
            OnSettled();
            yield break;
        }
    }
}