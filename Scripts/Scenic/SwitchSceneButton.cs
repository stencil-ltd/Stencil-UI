using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenic
{
    [RequireComponent(typeof(Button))]
    public class SwitchSceneButton : MonoBehaviour
    {
        public string scene;
        public int buildIndex = -1;

        public bool preventActivation;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                var index = buildIndex;
                if (!string.IsNullOrEmpty(scene))
                    index = SceneManager.GetSceneByName(scene).buildIndex;
                LoadingScene.Load(index, preventActivation);
            });
        }
    }
}