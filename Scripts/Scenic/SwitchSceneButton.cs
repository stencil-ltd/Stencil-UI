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

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(scene))
                    SceneManager.LoadScene(scene);
                else 
                    SceneManager.LoadScene(buildIndex);
            });
        }
    }
}