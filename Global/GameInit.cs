using System;
using System.Collections;
using CustomOrder;
using Scripts.Prefs;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

#if STENCIL_ANALYTICS
using Analytics;
#endif

#if STENCIL_ADS
using Ads;
using Ads.IronSrc;
#endif

#if STENCIL_FIREBASE
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Messaging;
using Scripts.Auth;
using Scripts.RemoteConfig;
#endif

#if STENCIL_FACEBOOK
using Facebook.Unity;
#endif

namespace Init
{
    [ExecutionOrder(-100)]
    public class GameInit : PermanentV2<GameInit>
    {
        public static DateTime FirstLaunch
        {
            get
            {
                var retval = StencilPrefs.Default.GetDateTime("game_init_first_launch");
                if (retval == null)
                {
                    retval = DateTime.UtcNow;
                    FirstLaunch = retval.Value;
                }
                return retval.Value;
            }
            private set => StencilPrefs.Default.SetDateTime("game_init_first_launch", value).Save();
        }

        public static TimeSpan SinceFirstLaunch => DateTime.Now - FirstLaunch.ToLocalTime();

        public bool Started { get; private set; }
        
        public static bool FirebaseReady;

        public static event EventHandler OnFacebookInit;
        public static event EventHandler OnFirebaseInit;

        protected override void OnAwake()
        {
            base.OnAwake();
            var unused = FirstLaunch;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.orientation = ScreenOrientation.Portrait;
            new GameObject("Main Thread Dispatch").AddComponent<UnityMainThreadDispatcher>();
            SceneManager.sceneLoaded += _OnNewScene;
            StartCoroutine(SetupLocation());
            SetupAnalytics();
            SetupFirebase();
            SetupFacebook();
            SetupAds();
            OnInit();
        }

        private IEnumerator SetupLocation()
        {
            #if STENCIL_LOCATION
            Input.location.Start();
            yield return null;
            Input.location.Stop();
            #endif
            yield break;
        }

        private static void SetupFacebook()
        {
#if STENCIL_FACEBOOK
            FB.Mobile.FetchDeferredAppLinkData();
            FB.Init(() =>
            {
                Debug.Log($"Facebook init: {FB.IsInitialized} [authed={FB.IsLoggedIn}]");
                FB.ActivateApp();
                OnFacebookInit?.Invoke();
            });
#endif
        }

        private void SetupAnalytics()
        {
            #if STENCIL_ANALYTICS
            var _ = Tracking.Instance;
            #else
            Debug.LogError("Not using Analytics services! Consider #STENCIL_ANALYTICS");
            #endif
        }

        private void SetupAds()
        {
            #if STENCIL_ADS
                #if STENCIL_IRONSRC
                    StencilAds.Init(new IronSrcRewarded(), new IronSrcInterstitial());
                #else
                    StencilAds.Init();
                #endif
            #endif
        }

        private void SetupFirebase()
        {
#if STENCIL_FIREBASE
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                var success = dependencyStatus == DependencyStatus.Available;
                if (!success)
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                FirebaseReady = success;

                Objects.Enqueue(() =>
                {
                    Debug.Log($"Firebase Configuration: {success}");
                    if (success)
                    {
                        var prod = StencilRemote.IsProd();
                        if (!prod) Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
                        var settings = FirebaseRemoteConfig.Settings;
                        settings.IsDeveloperMode = !prod;
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
                        OnFirebaseInit?.Invoke();
                    }
                    StencilAuth.Init();
                    OnFirebase(success);
                });
            });
#endif
        }
        

#if STENCIL_FIREBASE
        private void SetupPush()
        {
            FirebaseMessaging.RequestPermissionAsync();
            if (StencilRemote.IsDeveloper())
                FirebaseMessaging.SubscribeAsync("dev/remote");
            else 
                FirebaseMessaging.UnsubscribeAsync("dev/remote");
            FirebaseMessaging.TokenReceived += (sender, args) =>
            {
                Debug.Log($"Firebase/FCM Token: {args.Token}");    
            };
        }
#endif
        
        protected virtual void OnApplicationPause(bool pauseStatus)
        {
#if STENCIL_IRONSRC
            IronSource.Agent.onApplicationPause(pauseStatus);
#endif
        }

        private void _OnNewScene(Scene arg0, LoadSceneMode arg1)
        {
            OnNewScene(arg0, arg1);
        }

        protected virtual void OnFirebase(bool success)
        {
        }

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