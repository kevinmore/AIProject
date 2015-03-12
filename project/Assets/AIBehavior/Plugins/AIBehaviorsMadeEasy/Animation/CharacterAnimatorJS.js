// move the root AIBehavior folder to the "Standard Assets" folder then move this script into any regular folder and uncomment.
/*
import AIBehavior;

private var anim : Animation = null;
private var hasAnimationComponent : boolean = false;


function Awake()
{
	anim = GetComponentInChildren(Animation);

	hasAnimationComponent = anim != null;

	if ( !hasAnimationComponent )
	{
		Debug.LogWarning("No animation component found for the '" + gameObject.name + "' object or child objects");
	}
}


function ReceiveAnimation(animState : AIAnimationState)
{
	if ( hasAnimationComponent && animState != null )
	{
		var stateName : String = animState.name;

		if ( anim[stateName] != null )
		{
			anim[stateName].wrapMode = animState.animationWrapMode;
			anim[stateName].speed = animState.speed;
			anim.CrossFade(stateName);
		}
		else
			Debug.LogWarning("The animation state \"" + stateName + "\" couldn't be found.");
	}
}
*/
