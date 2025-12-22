using UnityEngine;
using Zenject;

namespace IGF
{
	public class HayBalesDistributor : MonoBehaviour, IHayBalesDistributor
	{
		[Inject] private IHayBaleSpawnEvents _events;
		[Inject] private HayBaleHolders _holders;

		public bool IsHasHayBale => !_holders.IsAllEmpty;

		private void OnEnable()
		{
			_events.OnSpawned += OnHayBaleSpawned;
		}

		private void OnDisable()
		{
			_events.OnSpawned -= OnHayBaleSpawned;
		}

		private void OnHayBaleSpawned(HayBale hayBale)
		{
			if (!_holders.TryPlace(hayBale)) 
				hayBale.Destroy();
		}

		public void TryPlaceTo(HayBaleHolder holder)
		{
			if (_holders.TryPopLast(out var hayBale))
				holder.TryPlace(hayBale);
		}
	}
}