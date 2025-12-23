using UnityEngine;
using IGF.Buyers;
using IGF.Wallet;
using Zenject;

namespace IGF
{
	public class HayBaleSaleZone : ProgressTriggerZone
	{
		[SerializeField, Min(0f)] private float _maxPurchaseDistance = 0.5f;
		
		[Inject] private IHayBalesDistributor _distributor;
		[Inject] private IWalletCurrencyAdder _wallet;
		[Inject] private IBuyersQueue _buyersQueue;

		protected override void OnPlayerInside()
		{
			if (_buyersQueue.IsHasBuyers && _distributor.IsHasHayBale)
				base.OnPlayerInside();
		}

		protected override void OnProgressComplete()
		{
			if (_buyersQueue.TryGetCurrentBuyer(out var buyer) &&
			    Vector3.SqrMagnitude(buyer.Position - _buyersQueue.GetFirstPointPosition())
					< _maxPurchaseDistance * _maxPurchaseDistance)
			{
				var holder = buyer.CompletePurchase();

				if (_distributor.TrySellTo(holder, out var currencyForSaleType))
					_wallet.AddCurrency(currencyForSaleType);	
			}

			ResetProgress();
		}
	}
}