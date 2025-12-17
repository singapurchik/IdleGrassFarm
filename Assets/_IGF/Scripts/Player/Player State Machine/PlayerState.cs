using IGF.Players.Animations;
using IGF.Players.AnimRig;
using IGF.UI;
using IGF.UI.ScreensGroups;
using UnityEngine;
using Zenject;

namespace IGF.Players.States
{
	public enum PlayerStates
	{
		Undefined = 0,
		Free = 1,
	}

	public abstract class PlayerState : State<PlayerStates, PlayerStates>
	{
		[Inject (Id = CharacterTransformType.Root)] protected Transform Transform;
		[Inject (Id = CharacterTransformType.Body)] protected Transform Body;
		[Inject] protected IInteractableFinderUpdater InteractableFinderUpdater;
		[Inject] protected IUIScreensGroupSwitcherInfo UIScreensGroupInfo;
		[Inject] protected PlayerCharacterController CharacterController;
		[Inject] protected IUIScreensGroupSwitcher UIScreensSwitcher;
		[Inject] protected PlayerAnimationRigging AnimationRigging;
		[Inject] protected IPlayerStateMachineInfo StateMachine;
		[Inject] protected IReadOnlyPlayerInteractor Interactor;
		[Inject] protected PlayerAnimEventsReceiver AnimEvents;
		[Inject] protected IPlayerStateReturner StateReturner;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected PlayerCameraShaker CameraShaker;
		[Inject] protected InputEventsInfo InputEvents;
		[Inject] protected IInputBlocker InputBlocker;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected PlayerMover Mover;
		[Inject] protected IInputInfo Input;

		public abstract bool IsPlayerControlledState { get; }

		protected Transform Parent { get; private set; }

		protected virtual void Awake() => Parent = transform.parent;
	}
}