using UnityEngine;
using Zenject;
using System;

namespace IGF
{
	[RequireComponent(typeof(BoxCollider))]
	public class Grass : MonoBehaviour, IDamageable
	{
		[SerializeField] private GameObject _grownGrassVisual;
		[Tooltip("Seconds")]
		[Range(1, 999)][SerializeField] private float _regrowingTime = 10f;
		[SerializeField] private HayBaleType _hayBaleType;

		[Inject] private IHayBaleSpawner _hayBaleSpawner;

		private BoxCollider _collider;
		
		private float _regrowAtTime;

		private bool _isGrown = true;
		
		public event Action<IDamageable> OnDamageTaken;

		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
		}


		void IDamageable.TryTakeDamage()
		{
			if (_isGrown)
			{
				Cut();
				OnDamageTaken?.Invoke(this);
			}
		}

		private void Cut()
		{
			_collider.enabled = false;
			_grownGrassVisual.SetActive(false);
			_regrowAtTime = Time.time + _regrowingTime;
			_hayBaleSpawner.Spawn(_hayBaleType);
			_isGrown = false;
		}

		private void Regrow()
		{
			_grownGrassVisual.SetActive(true);
			_collider.enabled = true;
			_isGrown = true;
		}

		private void Update()
		{
			if (!_isGrown && Time.time > _regrowAtTime)
				Regrow();
		}
	}
}