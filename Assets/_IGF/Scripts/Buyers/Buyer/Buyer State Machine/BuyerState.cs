using IGF.Buyers.Animations;
using Zenject;

namespace IGF.Buyers
{
	public enum BuyerStates
	{
		Undefined = 0,
		Idle = 1,
		Move = 2
	}
	
	public abstract class BuyerState : State<BuyerStates, BuyerStates>
	{
		[Inject] protected IMovementTargetHolder MovementTargetHolder;
		[Inject] protected BuyerAnimator Animator;
		[Inject] protected BuyerRotator Rotator;
		[Inject] protected BuyerMover Mover;
	}
}