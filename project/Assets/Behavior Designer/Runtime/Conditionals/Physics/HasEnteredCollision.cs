using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success when a collision starts.")]
    [TaskCategory("Physics")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=110")]
    public class HasEnteredCollision : Conditional
    {
        [Tooltip("The object that started the collision")]
        public SharedGameObject collidedGameObject;

        private bool enteredCollision = false;

        public override TaskStatus OnUpdate()
        {
            return enteredCollision ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            enteredCollision = false;
        }

        public override void OnCollisionEnter(Collision collision)
        {
            collidedGameObject.Value = collision.gameObject;
            enteredCollision = true;
        }

        public override void OnReset()
        {
            collidedGameObject = null;
        }
    }
}