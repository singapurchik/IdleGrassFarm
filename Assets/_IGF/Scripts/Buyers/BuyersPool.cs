namespace IGF.Buyers
{
	public class BuyersPool : ObjectPool<Buyer>
	{
		protected override void InitializeObject(Buyer buyer)
		{
			buyer.OnDestroyed += ReturnToPool;
		}

		protected override void CleanupObject(Buyer buyer)
		{
			buyer.OnDestroyed -= ReturnToPool;
		}
	}
}