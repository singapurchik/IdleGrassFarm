using IGF.Players.Animations;
using IGF.Players.States;
using Zenject;

namespace IGF.Players
{
    public class PlayerStateMachine : StateMachine<PlayerStates, PlayerState>, IPlayerStateMachineInfo
    {
	    [Inject] private PlayerAnimator _animator;
	    [Inject] private FreeRun _freeRunState;
	    [Inject] private Attack _attackState;
	    
        private PlayerState _lastControlledState;
        
        public override void Initialize()
        {
	        AddState(_attackState);
	        AddState(_freeRunState);
	        
	        TrySwitchStateTo(_freeRunState);
        }
        
        protected override void ExitCurrentState()
        {
	        base.ExitCurrentState();
	        _animator.TryResetTriggers();
        }
    }
}