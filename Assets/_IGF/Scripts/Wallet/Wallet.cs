using Zenject;

namespace IGF.Wallet
{
	public class Wallet : IWalletCurrencyAdder, IWalletCurrencyRemover
	{
		[Inject] private WalletView _view;

		private int _yellowCurrencyAmount;
		private int _greenCurrencyAmount;

		public void Initialize() => UpdateView();

		public bool TryRemoveCurrency(CurrencyType currencyType, int amount = 1)
		{
			switch (currencyType)
			{
				case CurrencyType.Yellow:
				default:
					return TryRemoveYellow(amount);
				case CurrencyType.Green:
					return TryRemoveGreen(amount);
			}
		}

		public bool TryRemoveYellow(int amount = 1) => TryDecreaseCurrency(ref _yellowCurrencyAmount, amount);
		
		public bool TryRemoveGreen(int amount = 1) => TryDecreaseCurrency(ref _greenCurrencyAmount, amount);
		
		public void AddCurrency(CurrencyType currencyType, int amount = 1)
		{
			switch (currencyType)
			{
				case CurrencyType.Yellow:
					default:
					AddYellow(amount);
					break;
				case CurrencyType.Green:
					AddGreen(amount);
					break;
			}
		}

		public void AddYellow(int amount = 1) => IncreaseCurrency(ref _yellowCurrencyAmount, amount);
		
		public void AddGreen(int amount = 1) => IncreaseCurrency(ref _greenCurrencyAmount, amount);

		private void IncreaseCurrency(ref int currency, int amount)
		{
			currency += amount;
			UpdateView();
		}
		
		private bool TryDecreaseCurrency(ref int currency, int amount = 1)
		{
			if (currency >= amount)
			{
				currency -= amount;
				UpdateView();
				return true;
			}
			
			return false;
		}

		private void UpdateView()
			=> _view.UpdateCurrenciesView(_greenCurrencyAmount.ToString(), _yellowCurrencyAmount.ToString());
	}
}