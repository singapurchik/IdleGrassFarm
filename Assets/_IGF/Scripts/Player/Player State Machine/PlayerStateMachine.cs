using IGF.Fatality;
using IGF.Players.Animations;
using IGF.Players.States;
using IGF.Players.States.Interaction;
using IGF.Players.States.Triggers;
using IGF.Players.States.Zone;
using IGF.Transitions;
using UnityEngine;
using Zenject;

namespace IGF.Players
{
    public class PlayerStateMachine : StateMachine<PlayerStates, PlayerState>, IPlayerStateMachineInfo,
	    IInteractionStatesSwitcher, ITriggerStatesSwitcher, IWeaponStatesSwitcher
    {
	    [Inject] private MoveFromExitToStartCorridor _moveFromExitToStartCorridorState;
	    [Inject] private InteractionWithLootHolder _interactionWithLootHolderState;
	    [Inject] private InteractionWithDeadBody _interactionWithDeadBodyState;
	    [Inject] private RangeWeaponBoltAction _rangeWeaponBoltActionState;
	    [Inject] private RangeWeaponShooting _rangeWeaponShootingState;
	    [Inject] private RotateToInteraction _rotateToInteractionState;
	    [Inject] private TeleportTransition _teleportTransitionState;
	    [Inject] private MoveToInteraction _moveToInteractionState;
	    [Inject] private RangeWeaponAiming _rangeWeaponAimingState;
	    [Inject] private RangeWeaponReady _rangeWeaponReadyState;
	    [Inject] private RangeWeaponStow _rangeWeaponStowState;
	    [Inject] private FatalityReceive _fatalityReceiveState;
	    [Inject] private LookTransition _lookTransitionState;
	    [Inject] private KnockedDown _knockedDownState;
	    [Inject] private LookAtZone _lookAtZoneState;
	    [Inject] private Dialogue _dialogueState;
	    [Inject] private PlayerAnimator _animator;
	    [Inject] private Respawn _respawnState;
	    [Inject] private Death _deathState;
	    [Inject] private Free _freeState;
	    
        [Inject] private IReadOnlyFatalityTarget _fatalityTarget;
        
        private PlayerState _lastControlledState;
        private PlayerStates _lastControlledKey;
        
        public IPrepareInteractionState CurrentPrepareInteractionState { get; private set; }

        private void OnEnable()
        {
            _fatalityTarget.OnReceivedFatality += OnFatality;
        }

        protected override void OnDisable()
        {
            _fatalityTarget.OnReceivedFatality -= OnFatality;
        }

        public override void Initialize()
        {
	        AddState(_moveFromExitToStartCorridorState);
	        AddState(_interactionWithLootHolderState);
	        AddState(_rangeWeaponBoltActionState);
	        AddState(_rangeWeaponShootingState);
	        AddState(_rotateToInteractionState);
	        AddState(_teleportTransitionState);
	        AddState(_moveToInteractionState);
	        AddState(_rangeWeaponAimingState);
	        AddState(_rangeWeaponReadyState);
	        AddState(_rangeWeaponStowState);
	        AddState(_fatalityReceiveState);
	        AddState(_lookTransitionState);
	        AddState(_knockedDownState);
	        AddState(_lookAtZoneState);
	        AddState(_dialogueState);
	        AddState(_respawnState);
	        AddState(_deathState);
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

        void IInteractionStatesSwitcher.SwitchToInteractionWithLootHolder(LootHolder lootHolder)
        {
            _interactionWithLootHolderState.SetLootHolder(lootHolder);
            TrySwitchStateTo(_interactionWithLootHolderState);
        }

        void IInteractionStatesSwitcher.SwitchToInteractionWithApartmentWindow(ITransitionZone transition)
        {
            _lookTransitionState.UpdateData(transition);
            TrySwitchStateTo(_lookTransitionState);
        }

        void IInteractionStatesSwitcher.SwitchToTeleportWithCameraEffectState(ITransitionZone transition)
        {
	        _teleportTransitionState.UpdateData(transition);
            TrySwitchStateTo(_teleportTransitionState);
        }

        void IInteractionStatesSwitcher.SwitchToMonologueState(DialogueSequence sequence)
        {
            _dialogueState.Setup(sequence);
            TrySwitchStateTo(_dialogueState);
        }

        void IInteractionStatesSwitcher.SwitchToInteractionWithDeadBody(DeadBody deadBody)
        {
	        _interactionWithDeadBodyState.SetDeadBody(deadBody);
            TrySwitchStateTo(_interactionWithDeadBodyState);
        }

        void IInteractionStatesSwitcher.SwitchToRotateToInteraction()
        {
	        TrySwitchStateTo(_rotateToInteractionState);
	        CurrentPrepareInteractionState = _rotateToInteractionState;
        }

        void IInteractionStatesSwitcher.SwitchToMoveToInteraction()
        {
	        TrySwitchStateTo(_moveToInteractionState);
	        CurrentPrepareInteractionState = _moveToInteractionState;
        }
        
        void IWeaponStatesSwitcher.SwitchToRangeWeaponReadyState() =>  TrySwitchStateTo(_rangeWeaponReadyState);

        void IInteractionStatesSwitcher.SwitchToNonInteractionState() => TryReturnLastControlledState();
        
        void ITriggerStatesSwitcher.SwitchToMoveFromExitToStartCorridor(Transform startPoint, Transform finishPoint)
        {
	        _moveFromExitToStartCorridorState.UpdateData(startPoint, finishPoint);
	        TrySwitchStateTo(_moveFromExitToStartCorridorState);
        }

        private void OnFatality() => TrySwitchStateTo(_fatalityReceiveState);

        public void TryReturnLastControlledState()
        {
            if (_lastControlledState != null && !CurrentState.IsPlayerControlledState)
                TrySwitchStateTo(_lastControlledState);
        }
    }
}