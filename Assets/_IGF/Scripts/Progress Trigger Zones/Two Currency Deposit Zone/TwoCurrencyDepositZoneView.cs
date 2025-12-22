using UnityEngine;
using IGF.Wallet;
using System;

namespace IGF
{
	[Serializable]
	public class TwoCurrencyDepositZoneView
	{
		[SerializeField] private CurrencyView _yellowView;
		[SerializeField] private CurrencyView _greenView;

		public void UpdateView(string yellowAmount, string greenAmount)
		{
			_yellowView.UpdateView(yellowAmount);
			_greenView.UpdateView(greenAmount);
		}
	}
}