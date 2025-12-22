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
		}

		public override void Perform()
		{
			Animator.RequestWalkAnim();
			
			if (MovementTargetHolder.IsHasTarget)
				Mover.NavMeshMove(MovementTargetHolder.Position);

			var targetPosition = MovementTargetHolder.Position;
			targetPosition.y = transform.position.y;
			
			if (Vector3.SqrMagnitude(transform.position - targetPosition) < 0.001f && Mover.IsFinishMoveThisFrame)
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