using State;
using UnityEngine;

namespace Standard.States
{
    [CreateAssetMenu(menuName = "State Machines/" + nameof(StoreStates))]
    public class StoreStates : StateMachine<StoreStates.State>
    {
        public new enum State
        {
            First, Second
        }   
    }
}