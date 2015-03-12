using UnityEngine;

namespace AIBehavior
{
	public struct AttackData
	{
		public Transform target;
		public float damage;
		public AttackState attackState;


		public AttackData(Transform target, float damage, AttackState attackState)
		{
			this.target = target;
			this.damage = damage;
			this.attackState = attackState;
		}
	}
}