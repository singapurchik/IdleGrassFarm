using UnityEngine;
using Zenject;

namespace IGF
{
	public class HayBalesDistributor : MonoBehaviour
	{
		[Inject] private IHayBaleSpawnEvents _events;
		[Inject] private HaleBaleHolders _holders;

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
			if (_holders.TryGetFreePoint(out var point))
				hayBale.SetParent(point);
			else
				hayBale.Destroy();
		}
	}
}