using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public sealed class BuyersQueue : MonoBehaviour, IBuyersQueue
	{
		[SerializeField] private Transform _exitPoint;

		[Inject] private IReadOnlyList<Transform> _points;

		private readonly Queue<Buyer> _buyers = new(6);

		public bool IsHasBuyers => _buyers.Count > 0;
		
		private void OnDisable()
		{
			foreach (var buyer in _buyers)
				buyer.OnPurchaseCompleted -= OnBuyerPurchaseCompleted;
		}

		public Vector3 GetFirstPointPosition() => _points[0].position;
		
		public void AddBuyer(Buyer buyer)
		{
			_buyers.Enqueue(buyer);
			buyer.OnPurchaseCompleted += OnBuyerPurchaseCompleted;
		}

		public bool TryGetEmptyPoint(out Transform point)
		{
			var index = _buyers.Count;

			if (index >= _points.Count)
			{
				point = null;
				return false;
			}

			point = _points[index];
			return true;
		}

		private void OnBuyerPurchaseCompleted(Buyer buyer)
		{
			buyer.OnPurchaseCompleted -= OnBuyerPurchaseCompleted;
			_buyers.Dequeue();

			if (_exitPoint != null)
				buyer.MovementTarget.Set(_exitPoint.position, BuyerMovementTargetType.Exit);

			ReassignTargets();
		}

		private void ReassignTargets()
		{
			var max = _points.Count;
			var i = 0;

			foreach (var queuedBuyer in _buyers)
			{
				if (i < max)
				{
					queuedBuyer.MovementTarget.Set(_points[i].position, BuyerMovementTargetType.QueuePoint);
					i++;	
				}
			}
		}
		
		public bool TryGetCurrentBuyer(out IBuyer buyer)
		{
			if (_buyers.TryPeek(out var current))
			{
				buyer = current;
				return true;
			}

			buyer = null;
			return false;
		}
	}
}