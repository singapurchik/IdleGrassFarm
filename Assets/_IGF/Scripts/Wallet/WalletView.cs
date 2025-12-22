using Zenject;

namespace IGF.Wallet
{
	public class WalletView
	{
		[Inject(Id = CurrencyType.Yellow)] private CurrencyView _yellowCurrencyView;
		[Inject(Id = CurrencyType.Green)] private CurrencyView _greenCurrencyView;

		public void UpdateCurrenciesView(string greenAmount, string yellowAmount)
		{
			_yellowCurrencyView.UpdateView(yellowAmount);
			_greenCurrencyView.UpdateView(greenAmount);
		}
	}
}