namespace IGF.Buyers
{
	public interface IBuyersQueue
	{
		public bool IsHasBuyers { get; }
		
		public bool TryGetCurrentBuyer(out IBuyer buyer);
	}
}