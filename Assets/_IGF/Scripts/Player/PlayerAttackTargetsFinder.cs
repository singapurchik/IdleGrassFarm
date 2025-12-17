using System.Collections.Generic;
using UnityEngine;

namespace IGF.Players
{
	public class PlayerAttackTargetsFinder : MonoBehaviour
	{
		private BoxCollider _collider;
		
		public HashSet<IDamageable> Damageables { get; private set; } = new ();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IDamageable damageable))
				Damageables.Add(damageable);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IDamageable damageable))
				Damageables.Remove(damageable);
		}
	}
}