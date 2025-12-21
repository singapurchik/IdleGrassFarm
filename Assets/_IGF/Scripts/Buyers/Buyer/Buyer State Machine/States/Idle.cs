namespace IGF.Buyers.States
{
	public class Idle : BuyerState
	{
		public override BuyerStates Key => BuyerStates.Idle;
		
		public override void Enter()
		{
			Mover.TryStopMove();
		}
	}
}