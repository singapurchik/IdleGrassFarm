using UnityEngine;

namespace IGF.Buyers.States
{
	public class Move : BuyerState
	{
		[SerializeField] private float _angularSpeed = 720;
		
		public override BuyerStates Key =>  BuyerStates.Move;
		
		public override void Enter()
		{
			Rotator.SetAutoAngularSpeed(_angularSpeed);
			Mover.NavMeshMove(MovementTargetHolder.TargetPosition);
		}

		public override void Perform()
		{
			Animator.RequestWalkAnim();

			if (Mover.IsFinishMoveThisFrame)
			{
				MovementTargetHolder.ClearTarget();
				RequestTransition(BuyerStates.Idle);
			}
		}
	}
}