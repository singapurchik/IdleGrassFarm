using UnityEngine;
using Zenject;
using System;

namespace IGF.Buyers
{
	public sealed class Buyer : MonoBehaviour, IDestroyable, IBuyer
	{
		[Inject] private IBuyerMovementTarget _movementTarget;
		[Inject] private BuyerStateMachine _stateMachine;
		[Inject] private HayBaleHolder _hayBaleHolder;
		[Inject] private BuyerMover _mover;
		
		public IBuyerMovementTarget MovementTarget => _movementTarget;

		public event Action<Buyer> OnPurchaseCompleted;
		public event Action<Buyer> OnDestroyed;

		private void Start() => _stateMachine.Initialize();

		public void RequestTeleport(Vector3 position) => _mover.RequestTeleport(position);
		
		public HayBaleHolder CompletePurchase()
		{
			OnPurchaseCompleted?.Invoke(this);
			return _hayBaleHolder;
		}
		
		public void Destroy()
		{
			_hayBaleHolder.SetEmpty();
			OnDestroyed?.Invoke(this);
		}
	}
}