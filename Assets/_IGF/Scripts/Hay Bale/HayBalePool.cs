namespace IGF
{
	public class HayBalePool : ObjectPool<HayBale>
	{
		protected override void InitializeObject(HayBale hayBale)
		{
			hayBale.OnDestryed += ReturnToPool;
		}

		protected override void CleanupObject(HayBale hayBale)
		{
			hayBale.OnDestryed -= ReturnToPool;
		}
	}
}