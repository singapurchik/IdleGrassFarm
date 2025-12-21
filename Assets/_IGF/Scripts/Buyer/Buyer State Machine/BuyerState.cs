using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public enum BuyerStates
	{
		Undefined = 0,
		Idle = 1
	}
	
	public abstract class BuyerState : State<BuyerStates, BuyerStates>
	{
		[Inject] protected BuyerMover Mover;
	}
}