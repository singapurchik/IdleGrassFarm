using IGF.Players.Animations;
using IGF.Players.States;
using Zenject;

namespace IGF.Players
{
    public class PlayerStateMachine : StateMachine<PlayerStates, PlayerState>, IPlayerStateMachineInfo, IPlayerStateReturner
    {
	    [Inject] private PlayerAnimator _animator;
	    [Inject] private Free _freeState;
	    
        private PlayerState _lastControlledState;
        private PlayerStates _lastControlledKey;
        public override void Initialize()
        {
	        AddState(_freeState);
	        
	        TrySwitchStateTo(_freeState);
        }
        
        protected override void ExitCurrentState()
        {
	        base.ExitCurrentState();
	        _animator.TryResetTriggers();
        }
		
        protected override void TrySwitchStateTo(PlayerState state)
        {
            if (CurrentState != null && CurrentState.IsPlayerControlledState)
	            _lastControlledState = CurrentState;

            base.TrySwitchStateTo(state);
	        //print(CurrentState);
        }

        public void TryReturnLastControlledState()
        {
            if (_lastControlledState != null && !CurrentState.IsPlayerControlledState)
                TrySwitchStateTo(_lastControlledState);
        }
    }
}