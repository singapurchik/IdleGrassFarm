using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace IGF.Players
{
	public class PlayerDamageablesFinder : MonoBehaviour, IPlayerDamageablesFinderResult
	{
		[SerializeField] private float _loseTargetsDelay = 0.5f;
		
		private readonly HashSet<IDamageable> _damageablesToRemove = new (16);
		private readonly HashSet<IDamageable> _damageables = new (16);
		private BoxCollider _collider;
		
		private float _timeToLoseTargets;
		
		public IEnumerable<IDamageable> Damageables => _damageables;
		
		public bool IsHasTargets { get; private set; }

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IDamageable damageable))
				Add(damageable);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IDamageable damageable))
				AddToRemove(damageable);
		}

		private void Add(IDamageable damageable)
		{
			_damageables.Add(damageable);
			damageable.OnDamageTaken += AddToRemove;
		}

		private void AddToRemove(IDamageable damageable)
		{
			_damageablesToRemove.Add(damageable);
			damageable.OnDamageTaken -= AddToRemove;
		}
		
		private void Update()
		{
			if (_damageables.Count > 0)
			{
				IsHasTargets = true;
				_timeToLoseTargets = Time.time + _loseTargetsDelay;
			}
			else if (IsHasTargets && Time.time > _timeToLoseTargets)
			{
				IsHasTargets = false;
			}
		}

		private void LateUpdate()
		{
			if (_damageablesToRemove.Count > 0)
			{
				foreach (var damageable in _damageablesToRemove)
					_damageables.Remove(damageable);
				
				_damageablesToRemove.Clear();
			}
		}
        
#if UNITY_EDITOR
		[Header("DEBUG")]
		[SerializeField] private bool _drawGizmos = true;
		[ReadOnly] [SerializeField] private BoxCollider _boxCollider;
		
		private void OnDrawGizmos()
		{
			if (_drawGizmos)
			{
				Gizmos.color = Color.red;
				
				if (_boxCollider == null)
					_boxCollider = GetComponent<BoxCollider>();
				
				var prevMatrix = Gizmos.matrix;
				Gizmos.matrix = transform.localToWorldMatrix;
				Gizmos.DrawCube(_boxCollider.center, _boxCollider.size);
				Gizmos.matrix = prevMatrix;
			}
		}
#endif
	}
}