using UnityEngine;

namespace Util.Coroutines
{
    public class WaitForTask : CustomYieldInstruction
    {
        private readonly System.Threading.Tasks.Task _task;
        public WaitForTask(System.Threading.Tasks.Task task)
        {
            _task = task;
        }

        public override bool keepWaiting => !_task.IsCompleted;
    }
}