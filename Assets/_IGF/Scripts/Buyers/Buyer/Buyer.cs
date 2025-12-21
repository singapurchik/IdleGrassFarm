using UnityEngine;
using Zenject;
using System;
using VInspector;

namespace IGF.Buyers
{
	public class Buyer : MonoBehaviour, IMovementTargetHolder
	{
		[Inject] private BuyerStateMachine _stateMachine;
		[Inject] private BuyerMover _mover;

		public Vector3 TargetPosition { get; private set; }

		public bool IsHasTarget { get; private set; }

		public event Action<Buyer> OnExitFromVisibleZone;
		public event Action<Buyer> OnPurchaseCompleted;

		private void Start()
		{
			_stateMachine.Initialize();
		}
		
		public void RequestTeleport(Vector3 position) => _mover.RequestTeleport(position);

		public void SetMovementTarget(Vector3 target)
		{
			TargetPosition = target;
			IsHasTarget = true;
		}

		public void ClearTarget() => IsHasTarget = false;

		[Button]
		public void InvokeOnExitFromVisibleZone() => OnExitFromVisibleZone?.Invoke(this);
		
		[Button]
		public void InvokeOnPurchaseCompleted() => OnPurchaseCompleted?.Invoke(this);
	}
}