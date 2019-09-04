using UnityEngine;
using UnityEngine.UI;

namespace Lobbing
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Lobber))]
    public class LobButton : MonoBehaviour
    {
        public ulong amount = 10;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => StartCoroutine(GetComponent<Lobber>().LobMany(amount)));
        }
    }
}