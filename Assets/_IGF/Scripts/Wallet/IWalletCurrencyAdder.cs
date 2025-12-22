namespace IGF.Wallet
{
	public interface IWalletCurrencyAdder
	{
		public void AddCurrency(CurrencyType currencyType, int amount = 1);
		
		public void AddYellow(int amount = 1);
		
		public void AddGreen(int amount = 1);
	}
}