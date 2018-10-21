using State;
using UnityEngine;

namespace Standard.States
{
    [CreateAssetMenu(menuName = "State Machines/" + nameof(PlayStates))]
    public class PlayStates : StateMachine<PlayStates.State>
    {
        public new enum State
        {
            Loading, Menu, Store, Playing, Finished, Share, Options
        }   
    }
}