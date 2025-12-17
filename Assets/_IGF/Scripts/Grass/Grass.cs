using UnityEngine;

namespace IGF
{
	public class Grass : MonoBehaviour, IDamageable, IPickupable
	{
		[SerializeField] private GameObject _grownGrassVisual;
		[SerializeField] private GameObject _cutGrassVisual;
		[Range(1, 999)][SerializeField] private float _regrowingTime = 10f;
		
		private float _regrowAtTime;
		
		public GrassState CurrentState { get; private set; } = GrassState.Grown;
		
		void IDamageable.TryTakeDamage()
		{
			if (CurrentState == GrassState.Grown)
				Cut();
		}
		
		void IPickupable.TryPickUp()
		{
			if (CurrentState == GrassState.Cut)
				PickUp();
		}

		private void Cut()
		{
			_grownGrassVisual.SetActive(false);
			_cutGrassVisual.SetActive(true);
			CurrentState = GrassState.Cut;
		}

		private void PickUp()
		{
			_grownGrassVisual.SetActive(false);
			_cutGrassVisual.SetActive(false);
			_regrowAtTime = Time.time + _regrowingTime;
			CurrentState = GrassState.Regrowing;
		}

		private void Regrow()
		{
			_grownGrassVisual.SetActive(true);
			_cutGrassVisual.SetActive(false);
			CurrentState = GrassState.Grown;
		}

		private void Update()
		{
			if (CurrentState == GrassState.Regrowing && Time.time > _regrowAtTime)
				Regrow();
		}
	}
}