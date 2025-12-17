using IGF.Players.Animations;
using IGF.Players.States;
using Zenject;

namespace IGF.Players
{
    public class PlayerStateMachine : StateMachine<PlayerStates, PlayerState>, IPlayerStateMachineInfo
    {
	    [Inject] private PlayerAnimator _animator;
	    [Inject] private Attack _attackState;
	    [Inject] private Free _freeState;
	    
        private PlayerState _lastControlledState;
        
        public override void Initialize()
        {
	        AddState(_attackState);
	        AddState(_freeState);
	        
	        TrySwitchStateTo(_freeState);
        }
        
        protected override void ExitCurrentState()
        {
	        base.ExitCurrentState();
	        _animator.TryResetTriggers();
        }

        public void TrySwitchToAttackState() => TrySwitchStateTo(_attackState);
        
        public void TrySwitchToDefaultState() => TrySwitchStateTo(_freeState);
    }
}