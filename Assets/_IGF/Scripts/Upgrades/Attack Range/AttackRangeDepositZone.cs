using Zenject;

namespace IGF.Upgrades
{
	public class AttackRangeDepositZone : TwoCurrencyDepositUpgradeZone
	{
		[Inject] private IAttackRangeUpgrader _upgrader;

		protected override void ApplyUpgrade() => _upgrader.Upgrade();
	}
}