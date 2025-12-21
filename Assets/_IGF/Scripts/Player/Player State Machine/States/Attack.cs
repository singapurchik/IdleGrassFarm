using System.Linq;
using UnityEngine;

namespace IGF.Players.States
{
	public class Attack : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 20f;
		[Range(0, 1)][SerializeField] private float _maxLocomotionValue = 0.5f;
		
		public override PlayerStates Key => PlayerStates.Attack;
		
		public override void Enter()
		{
			AnimEvents.OnAttack += TryAttack;
			Animator.PlayAttackAnim();
		}

		public override void Perform()
		{
			var locomotionValue = Mathf.Min(_maxLocomotionValue, Input.GetJoystickDirection2D().magnitude);
			Animator.RequestSetLocomotionValue(locomotionValue);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
			
			if (!DamageablesFinderResult.IsHasTargets || !HayBaleHolders.IsHasAnySpace)
				RequestTransition(PlayerStates.FreeRun);
		}

		private void TryAttack()
		{
			if (DamageablesFinderResult.Damageables.Any())
				foreach (var damageable in DamageablesFinderResult.Damageables)
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