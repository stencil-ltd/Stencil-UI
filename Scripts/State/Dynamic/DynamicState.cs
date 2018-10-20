using UnityEngine;

namespace State.Dynamic
{
    [CreateAssetMenu(menuName = "Dynamic States/State")]
    public class DynamicState : ScriptableObject
    {
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }
}
