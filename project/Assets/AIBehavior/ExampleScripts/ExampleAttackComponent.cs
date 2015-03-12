using UnityEngine;
using AIBehavior;


namespace AIBehaviorExamples
{
	public class ExampleAttackComponent : MonoBehaviour
	{
		public GameObject projectilePrefab;
		public Transform launchPointWeapon;


		public void MeleeAttack(AttackData attackData)
		{
			Debug.Log ("Melee attack");
			// Handle Melee attack behavior here...
		}


		public void RangedAttack(AttackData attackData)
		{
			if ( attackData.target != null )
			{
				launchPointWeapon.LookAt(attackData.target);

				GameObject projectile = GameObject.Instantiate(projectilePrefab, launchPointWeapon.position, launchPointWeapon.rotation) as GameObject;
				ExampleProjectile projectileComponent = projectile.GetComponent<ExampleProjectile>();
				projectileComponent.damage = attackData.damage;

				Debug.Log ("Attacked target '" + attackData.target.name + "' with attack state named '" + attackData.attackState.name + "' with damage " + attackData.damage);
			}
			else
			{
				Debug.LogWarning("attackData.target is null, you may want to have a NoPlayerInSight trigger on the AI '" + attackData.attackState.transform.parent.name + "'");
			}
		}
	}
}