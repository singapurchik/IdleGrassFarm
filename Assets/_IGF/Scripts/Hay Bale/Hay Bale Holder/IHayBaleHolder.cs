namespace IGF
{
	public interface IHayBaleHolder
	{
		public bool IsAvailable { get; }
		public bool IsFull { get; }
		
		public bool TryReserve(out HayBaleSlot slot);
		
		public void Release(in HayBaleSlot slot);
	}
}