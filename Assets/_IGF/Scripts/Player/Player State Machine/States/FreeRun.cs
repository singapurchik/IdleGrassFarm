using UnityEngine;

namespace IGF.Players.States
{
	public class FreeRun : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 20f;
		
		public override PlayerStates Key => PlayerStates.FreeRun;
		
		public override void Enter()
		{
		}

		public override void Perform()
		{
			Animator.RequestSetLocomotionValue(Input.GetJoystickDirection2D().magnitude);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
			
			if (DamageablesFinderResult.IsHasTargets && HayBaleHolders.IsHasAnySpace)
				RequestTransition(PlayerStates.Attack);
		}
	}
}