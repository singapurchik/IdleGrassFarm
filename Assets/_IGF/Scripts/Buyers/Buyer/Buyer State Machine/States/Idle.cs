namespace IGF.Buyers.States
{
	public class Idle : BuyerState
	{
		public override BuyerStates Key => BuyerStates.Idle;
		
		public override void Enter()
		{
			Mover.TryStopMove();
		}

		public override void Perform()
		{
			if (MovementTargetHolder.IsHasTarget)
				RequestTransition(BuyerStates.Move);
		}
	}
}