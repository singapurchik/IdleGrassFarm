namespace IGF
{
	public class CartsPool : ObjectPool<Cart>
	{
		protected override void InitializeObject(Cart cart)
		{
			//cart.OnDestroyed += ReturnToPool;
		}

		protected override void CleanupObject(Cart cart)
		{
			//cart.OnDestroyed -= ReturnToPool;
		}
	}
}