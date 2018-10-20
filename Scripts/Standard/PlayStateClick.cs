using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Standard.States
{
    public class PlayStateClick : MonoBehaviour, IPointerClickHandler
    {
        public PlayStates.State State;

        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn?.onClick.AddListener(Execute);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Execute();
        }

        private void Execute()
        {
            PlayStates.Instance.RequestState(State);
        }
    }
}