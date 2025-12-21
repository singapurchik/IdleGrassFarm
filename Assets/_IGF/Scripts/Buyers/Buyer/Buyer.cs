using UnityEngine;
using Zenject;
using System;

namespace IGF.Buyers
{
	public class Buyer : MonoBehaviour, IMovementTargetHolder
	{
		[Inject] private BuyerStateMachine _stateMachine;
		[Inject] private BuyerMover _mover;

		public Transform CurrentMovementTarget { get; private set; }

		public bool IsHasTarget => CurrentMovementTarget != null;

		public event Action<Buyer> OnExitFromVisibleZone;
		public event Action<Buyer> OnPurchaseCompleted;

		private void Start()
		{
			_stateMachine.Initialize();
		}
		
		public void RequestTeleport(Vector3 position) => _mover.RequestTeleport(position);

		public void SetMovementTarget(Transform target) => CurrentMovementTarget = target;

		public void ClearTarget() => CurrentMovementTarget = null;
	}
}