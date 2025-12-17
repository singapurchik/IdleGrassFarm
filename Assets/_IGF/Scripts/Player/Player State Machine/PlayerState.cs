using IGF.Players.Animations;
using IGF.Players.AnimRig;
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
		[Inject] protected PlayerAnimationRigging AnimationRigging;
		[Inject] protected IPlayerStateMachineInfo StateMachine;
		[Inject] protected PlayerAnimEventsReceiver AnimEvents;
		[Inject] protected IPlayerStateReturner StateReturner;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected PlayerMover Mover;
		[Inject] protected IPlayerInputInfo Input;

		public abstract bool IsPlayerControlledState { get; }

		protected Transform Parent { get; private set; }

		protected virtual void Awake() => Parent = transform.parent;
	}
}