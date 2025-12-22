using IGF.Wallet;
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

		public bool TrySellTo(HayBaleHolder holder, out CurrencyType currencyForSaleType)
		{
			if (_holders.TryPopLast(out var hayBale))
			{
				holder.TryPlace(hayBale);
				currencyForSaleType = hayBale.CurrencyForSaleType;
				return true;
			}

			currencyForSaleType = default;
			return false;
		}
	}
}