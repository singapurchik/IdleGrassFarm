using System.Collections.Generic;
using UnityEngine;

namespace IGF
{
	public class HayBaleHolder : MonoBehaviour, IHayBaleHolder
	{
		[SerializeField] private List<Transform> _points = new ();
		
		private int _currentIndex;

		public bool IsAvailable => gameObject.activeInHierarchy;
		public bool IsFull => _currentIndex >= _points.Count;

		private void Awake() => enabled = false;

		public bool TryGetPoint(out Transform point)
		{
			if (_points.Count > _currentIndex)
			{
				point = _points[_currentIndex];
				_currentIndex++;
				return true;
			}

			point = null;
			return false;
		}
	}
}