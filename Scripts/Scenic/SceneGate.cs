using State.Active;
using UnityEngine.SceneManagement;

namespace Scenic
{
    public class SceneGate : ActiveGate
    {
        public int[] indices;
        public string[] names;

        public override bool? Check()
        {
            var scene = SceneManager.GetActiveScene();
            var idx = scene.buildIndex;
            var name = scene.name;

            foreach (var value in indices)
                if (value == idx)
                    return true;

            foreach (var value in names)
                if (value == name)
                    return true;

            return false;
        }
    }
}