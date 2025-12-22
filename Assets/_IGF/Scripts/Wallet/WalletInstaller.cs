using System;
using UnityEngine;
using Zenject;

namespace IGF.Wallet
{
	public class WalletInstaller : MonoInstaller
	{
		[SerializeField] private CurrencyView _yellowCurrencyView;
		[SerializeField] private CurrencyView _greenCurrencyView;
		
		private readonly WalletView _view = new ();
		private readonly Wallet _wallet = new ();

		public override void InstallBindings()
		{
			Container.BindInstance(_view).WhenInjectedIntoInstance(_wallet);
			
			Container.BindInstance(_yellowCurrencyView)
				.WithId(CurrencyType.Yellow)
				.WhenInjectedIntoInstance(_view);
			
			Container.BindInstance(_greenCurrencyView)
				.WithId(CurrencyType.Green)
				.WhenInjectedIntoInstance(_view);

			Container.Bind<IWalletCurrencyRemover>().FromInstance(_wallet).AsSingle();
			Container.Bind<IWalletCurrencyAdder>().FromInstance(_wallet).AsSingle();
			Container.QueueForInject(_wallet);
			Container.QueueForInject(_view);
		}

		private void Awake() => _wallet.Initialize();
	}
}