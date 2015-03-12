using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCharacterController
{
    [TaskCategory("Basic/CharacterController")]
    [TaskDescription("Sets the slope limit of the CharacterController. Returns Success.")]
    public class SetSlopeLimit : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The slope limit of the CharacterController")]
        public SharedFloat slopeLimit;

        private CharacterController characterController;

        public override void OnStart()
        {
            characterController = GetDefaultGameObject(targetGameObject.Value).GetComponent<CharacterController>();
        }

        public override TaskStatus OnUpdate()
        {
            if (characterController == null) {
                Debug.LogWarning("CharacterController is null");
                return TaskStatus.Failure;
            }

            characterController.slopeLimit = slopeLimit.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            slopeLimit = 0;
        }
    }
}