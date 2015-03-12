using UnityEngine;
using AIBehavior;
using System.Collections;

namespace AIBehavior
{
	/// <summary>
	/// Returns true if this AI has health above zero.
	/// </summary>
	public class IsAliveTrigger : BaseTrigger
	{
		protected override void Init()
		{
		}


		protected override bool Evaluate(AIBehaviors fsm)
		{
			return fsm.health > 0;
		}
	}
}