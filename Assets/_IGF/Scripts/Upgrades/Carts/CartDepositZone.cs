using Zenject;

namespace IGF.Upgrades.Carts
{
	public class CartDepositZone : TwoCurrencyDepositUpgradeZone
	{
		[Inject] private ICartSpawner _cartSpawner;

		protected override void ApplyUpgrade() => _cartSpawner.Spawn();
	}
}