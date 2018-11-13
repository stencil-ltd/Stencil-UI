using State;
using UnityEngine;

namespace Standard.States
{
    [CreateAssetMenu(menuName = "State Machines/" + nameof(EolStates))]
    public class EolStates : StateMachine<EolStates.State>
    {
        public new enum State
        {
            None,
            Challenge,
            Unlocked
        }   
    }
}