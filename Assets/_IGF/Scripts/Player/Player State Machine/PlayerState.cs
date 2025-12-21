using IGF.Players.Animations;
using IGF.Players.AnimRig;
using UnityEngine;
using Zenject;

namespace IGF.Players.States
{
	public enum PlayerStates
	{
		Undefined = 0,
		FreeRun = 1,
		Attack = 2
	}

	public abstract class PlayerState : State<PlayerStates, PlayerStates>
	{
		[Inject (Id = CharacterTransformType.Root)] protected Transform Transform;
		[Inject (Id = CharacterTransformType.Body)] protected Transform Body;
		[Inject] protected IPlayerDamageablesFinderResult DamageablesFinderResult;
		[Inject] protected PlayerAnimationRigging AnimationRigging;
		[Inject] protected IPlayerStateMachineInfo StateMachine;
		[Inject] protected PlayerAnimEventsReceiver AnimEvents;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected HaleBaleHolders HaleBaleHolders;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected IPlayerInputInfo Input;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected PlayerMover Mover;

		protected Transform Parent { get; private set; }

		protected virtual void Awake() => Parent = transform.parent;
	}
}