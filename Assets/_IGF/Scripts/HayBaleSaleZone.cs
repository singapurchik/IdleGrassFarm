using IGF.Buyers;
using Zenject;

namespace IGF
{
	public class HayBaleSaleZone : ProgressTriggerZone
	{
		[Inject] private HayBaleHolders _hayBaleHolders;
		[Inject] private IBuyersQueue _buyersQueue;

		protected override void OnPlayerInside()
		{
			if (_buyersQueue.IsHasBuyers && !_hayBaleHolders.IsAllEmpty)
				base.OnPlayerInside();
		}

		protected override void OnProgressComplete()
		{
			if (_buyersQueue.TryGetCurrentBuyer(out var buyer) && _hayBaleHolders.TryPopLast(out var hayBale))
				buyer.Buy(hayBale);

			ResetProgress();	
		}
	}
}