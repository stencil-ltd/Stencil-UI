using System;
using System.Collections;
using Scripts.Lifecycle;
using Scripts.Prefs;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

#if STENCIL_ANALYTICS
using Analytics;
#endif

namespace Init
{
    public class GameInit : AbstractGameInit
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

        private void SetupAnalytics()
        {
            #if STENCIL_ANALYTICS
            var _ = Tracking.Instance;
            #else
            Debug.LogError("Not using Analytics services! Consider #STENCIL_ANALYTICS");
            #endif
        }

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