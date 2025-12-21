using IGF.Buyers.States;
using Zenject;

namespace IGF.Buyers
{
	public class BuyerStateMachine : StateMachine<BuyerStates, BuyerState>, IBuyerStateMachineInfo
	{
		[Inject] private BuyerAnimator _animator;
		[Inject] private Idle _idleState;
		
		private BuyerState _lastControlledState;
        
		public override void Initialize()
		{
			AddState(_idleState);

			TrySwitchStateTo(_idleState);
		}
        
		protected override void ExitCurrentState()
		{
			base.ExitCurrentState();
			_animator.TryResetTriggers();
		}
	}
}