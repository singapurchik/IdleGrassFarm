using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public class BuyersQueue : MonoBehaviour
	{
		[Inject] private IReadOnlyList<Transform> _points;

		private int _emptyPointIndex;

		public int PointsCount => _points.Count;
		public bool IsHasEmptyPoint => _emptyPointIndex < _points.Count;

		public Transform GetPoint(int index) => _points[index];

		public bool TryGetEmptyPoint(out Transform point)
		{
			if (IsHasEmptyPoint)
			{
				point = _points[_emptyPointIndex];
				_emptyPointIndex++;
				return true;
			}

			point = null;
			return false;
		}

		public void SetEmptyPoint() => _emptyPointIndex = Mathf.Max(0, _emptyPointIndex - 1);
	}
}