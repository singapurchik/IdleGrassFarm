namespace IGF
{
	public interface IHayBaleHolder
	{
		bool IsAvailable { get; }
		bool IsFull { get; }

		bool TryReserve(out HayBaleSlot slot);

		void Attach(in HayBaleSlot slot, HayBale hayBale);

		bool TryReleaseLast(out HayBale hayBale);
	}
}