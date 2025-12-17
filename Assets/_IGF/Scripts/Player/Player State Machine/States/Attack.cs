using UnityEngine;
using Zenject;

namespace IGF.Players.States
{
	public class Attack : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 20f;
		
		[Inject] private PlayerAttackTargetsFinder _targetsFinder;
		
		public override PlayerStates Key => PlayerStates.Attack;
		
		public override void Enter()
		{
			AnimEvents.OnAttack += TryAttack;
			Animator.PlayAttackAnim();
		}

		public override void Perform()
		{
			Animator.RequestSetLocomotionValue(Input.GetJoystickDirection2D().magnitude);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
		}

		private void TryAttack()
		{
			if (_targetsFinder.Damageables.Count > 0)
				foreach (var damageable in _targetsFinder.Damageables)
					damageable.TryTakeDamage();
		}

		public override void Exit()
		{
			Animator.StopAttackAnim();
			AnimEvents.OnAttack -= TryAttack;
			base.Exit();
		}
	}
}