using UnityEngine;
using VInspector;
using Zenject;
using System;

namespace IGF.Buyers
{
	public sealed class Buyer : MonoBehaviour, IDestroyable
	{
		[Inject] private IBuyerMovementTarget _movementTarget;
		[Inject] private BuyerStateMachine _stateMachine;
		[Inject] private BuyerMover _mover;
		
		public IBuyerMovementTarget MovementTarget => _movementTarget;

		public event Action<Buyer> OnPurchaseCompleted;
		public event Action<Buyer> OnDestroyed;

		private void Start() => _stateMachine.Initialize();

		public void RequestTeleport(Vector3 position) => _mover.RequestTeleport(position);

		[Button]
		public void NotifyPurchaseCompleted() => OnPurchaseCompleted?.Invoke(this);

		public void Destroy() => OnDestroyed?.Invoke(this);
	}
}