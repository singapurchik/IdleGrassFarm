using System.Linq;
using UnityEngine;

namespace IGF.Players.States
{
	public class Attack : PlayerState
	{
		[SerializeField] private MeshRenderer _attackRangeView;
		[SerializeField] private ParticleSystem _attackEffect;
		[SerializeField] private float _rotationSpeed = 20f;
		[Range(0, 1)][SerializeField] private float _maxLocomotionValue = 0.5f;
		
		public override PlayerStates Key => PlayerStates.Attack;
		
		public override void Enter()
		{
			AnimEvents.OnAttack += TryAttack;
			_attackRangeView.enabled = true;
			Animator.PlayAttackAnim();
		}

		public override void Perform()
		{
			var locomotionValue = Mathf.Min(_maxLocomotionValue, Input.GetJoystickDirection2D().magnitude);
			Animator.RequestSetLocomotionValue(locomotionValue);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
			
			if (!DamageablesFinderResult.IsHasTargets || !HayBalesHolder.IsHasAnySpace())
				RequestTransition(PlayerStates.FreeRun);
		}

		private void TryAttack()
		{
			if (DamageablesFinderResult.Damageables.Any())
			{
				_attackEffect.Play();
				
				foreach (var damageable in DamageablesFinderResult.Damageables)
					damageable.TryTakeDamage();
			}
		}

		public override void Exit()
		{
			Animator.StopAttackAnim();
			_attackRangeView.enabled = false;
			AnimEvents.OnAttack -= TryAttack;
			base.Exit();
		}
	}
}