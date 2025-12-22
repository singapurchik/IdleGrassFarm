namespace IGF.Wallet
{
	public interface IWalletCurrencyRemover
	{
		public bool TryRemoveCurrency(CurrencyType currencyType, int amount = 1);
		
		public bool TryRemoveYellow(int amount = 1);
		
		public bool TryRemoveGreen(int amount = 1);
	}
}