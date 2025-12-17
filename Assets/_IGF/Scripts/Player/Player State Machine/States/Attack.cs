using UnityEngine;

namespace IGF.Players.States
{
	public class Attack : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 20f;
		
		public override PlayerStates Key => PlayerStates.Attack;
		
		public override void Enter()
		{
			Animator.PlayAttackAnim();
		}

		public override void Perform()
		{
			Animator.RequestSetLocomotionValue(Input.GetJoystickDirection2D().magnitude);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
		}

		public override void Exit()
		{
			Animator.StopAttackAnim();
			base.Exit();
		}
	}
}