namespace IGF
{
	public interface IHayBalesDistributor
	{
		public bool IsHasHayBale { get; }
		
		public void TryPlaceTo(HayBaleHolder holder);
	}
}