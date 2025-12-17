using UnityEngine;

namespace IGF.Players.States
{
	public class Free : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 20f;
		
		public override PlayerStates Key => PlayerStates.Free;
		
		public override void Enter()
		{
		}

		public override void Perform()
		{
			Animator.RequestSetLocomotionValue(Input.GetJoystickDirection2D().magnitude);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
		}
	}
}