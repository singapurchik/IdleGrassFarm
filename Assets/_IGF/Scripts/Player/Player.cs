using IGF.Upgrades;
using UnityEngine;
using Zenject;

namespace IGF.Players
{
	public sealed class Player : MonoBehaviour, IPlayer, IAttackRangeUpgrader, ICartTarget
	{
		[SerializeField] private AttackRangeUpgradeState _attackRange;
		[SerializeField] private Transform _attackContainer;

		[Inject] private PlayerStateMachine _stateMachine;
		
		public Vector3 Position => transform.position;
		public Vector3 Forward => transform.forward;

		private void Start()
		{
			_stateMachine.Initialize();

			ApplyAttackRange();
		}

		void IAttackRangeUpgrader.Upgrade()
		{
			_attackRange.Upgrade();
			ApplyAttackRange();
		}

		private void ApplyAttackRange()
		{
			var currentRange = _attackRange.GetCurrentRange();
			_attackContainer.localScale = new Vector3(currentRange, _attackContainer.localScale.y, currentRange);
		}
	}
}