using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public sealed class BuyersQueue : MonoBehaviour
	{
		[SerializeField] private Transform _exitPoint;

		[Inject] private IReadOnlyList<Transform> _points;

		private readonly List<Buyer> _buyers = new(6);

		private void OnDisable()
		{
			for (int i = 0; i < _buyers.Count; i++)
			{
				var buyer = _buyers[i];
				if (buyer == null) continue;

				buyer.OnPurchaseCompleted -= OnBuyerPurchaseCompleted;
			}
		}

		public void AddBuyer(Buyer buyer)
		{
			_buyers.Add(buyer);
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
			if (_buyers.Count == 0 || _buyers[0] != buyer)
				return;

			buyer.OnPurchaseCompleted -= OnBuyerPurchaseCompleted;
			_buyers.RemoveAt(0);

			buyer.MovementTarget.Set(_exitPoint.position, BuyerMovementTargetType.Exit);
			ReassignTargets();
		}

		private void ReassignTargets()
		{
			var count = Mathf.Min(_buyers.Count, _points.Count);

			for (int i = 0; i < count; i++)
				_buyers[i].MovementTarget.Set(_points[i].position, BuyerMovementTargetType.QueuePoint);
		}
	}
}