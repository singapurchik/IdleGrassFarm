using IGF.Buyers.Animations;
using IGF.Buyers.States;
using Zenject;

namespace IGF.Buyers
{
	public class BuyerStateMachine : StateMachine<BuyerStates, BuyerState>, IBuyerStateMachineInfo
	{
		[Inject] private BuyerAnimator _animator;
		[Inject] private Idle _idleState;
		[Inject] private Move _moveState;
		
		private BuyerState _lastControlledState;
        
		public override void Initialize()
		{
			AddState(_idleState);
			AddState(_moveState);

			TrySwitchStateTo(_idleState);
		}
        
		protected override void ExitCurrentState()
		{
			base.ExitCurrentState();
			_animator.TryResetTriggers();
		}
	}
}