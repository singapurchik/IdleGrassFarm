using IGF.Wallet;

namespace IGF
{
	public interface IHayBalesDistributor
	{
		public bool IsHasHayBale { get; }
		
		public bool TrySellTo(HayBaleHolder holder, out CurrencyType currencyForSaleType);
	}
}