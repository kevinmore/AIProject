using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
    [TaskCategory("Basic/Animation")]
    [TaskDescription("Blends the animation. Returns Success.")]
    public class Blend : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The name of the animation")]
        public SharedString animationName;
        [Tooltip("The weight the animation should blend to")]
        public float targetWeight = 1;
        [Tooltip("The amount of time it takes to blend")]
        public float fadeLength = 0.3f;

        // cache the animation component
        private Animation targetAnimation;

        public override void OnStart()
        {
            targetAnimation = GetDefaultGameObject(targetGameObject.Value).GetComponent<Animation>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetAnimation == null) {
                Debug.LogWarning("Animation is null");
                return TaskStatus.Failure;
            }

            targetAnimation.Blend(animationName.Value, targetWeight, fadeLength);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            animationName = "";
            targetWeight = 1;
            fadeLength = 0.3f;
        }
    }
}