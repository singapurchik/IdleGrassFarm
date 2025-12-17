using UnityEngine;

namespace IGF
{
	public class Grass : MonoBehaviour, IDamageable
	{
		[SerializeField] private GameObject _grownGrassVisual;
		[Range(1, 999)][SerializeField] private float _regrowingTime = 10f;
		
		private float _regrowAtTime;
		
		public GrassState CurrentState { get; private set; } = GrassState.Grown;
		
		void IDamageable.TryTakeDamage()
		{
			if (CurrentState == GrassState.Grown)
				Cut();
		}

		private void Cut()
		{
			_grownGrassVisual.SetActive(false);
			_regrowAtTime = Time.time + _regrowingTime;
			CurrentState = GrassState.Regrowing;
		}

		private void Regrow()
		{
			_grownGrassVisual.SetActive(true);
			CurrentState = GrassState.Grown;
		}

		private void Update()
		{
			if (CurrentState == GrassState.Regrowing && Time.time > _regrowAtTime)
				Regrow();
		}
	}
}