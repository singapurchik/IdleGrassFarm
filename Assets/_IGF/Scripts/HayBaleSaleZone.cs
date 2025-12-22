using IGF.Buyers;
using Zenject;

namespace IGF
{
	public class HayBaleSaleZone : ProgressTriggerZone
	{
		[Inject] private IHayBalesDistributor _distributor;
		[Inject] private IBuyersQueue _buyersQueue;

		protected override void OnPlayerInside()
		{
			if (_buyersQueue.IsHasBuyers && _distributor.IsHasHayBale)
				base.OnPlayerInside();
		}

		protected override void OnProgressComplete()
		{
			if (_buyersQueue.TryGetCurrentBuyer(out var buyer))
			{
				var holder = buyer.CompletePurchase();
				_distributor.TryPlaceTo(holder);
			}

			ResetProgress();	
		}
	}
}