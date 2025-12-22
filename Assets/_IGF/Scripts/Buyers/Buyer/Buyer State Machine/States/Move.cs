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
			Mover.NavMeshMove(MovementTargetHolder.Position);
		}

		public override void Perform()
		{
			Animator.RequestWalkAnim();
			
			if (MovementTargetHolder.IsHasTarget)
			{
			print(MovementTargetHolder.Position);
				Mover.NavMeshMove(MovementTargetHolder.Position);
			}

			if (Mover.IsFinishMoveThisFrame)
			{
				MovementTargetHolder.Clear();
				
				if (MovementTargetHolder.Type == BuyerMovementTargetType.QueuePoint)
					RequestTransition(BuyerStates.Idle);
				else
					Destroyable.Destroy();
			}
		}
	}
}