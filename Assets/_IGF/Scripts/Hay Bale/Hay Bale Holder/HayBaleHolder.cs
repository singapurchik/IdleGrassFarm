using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF
{
	public class HayBaleHolder : MonoBehaviour
	{
		[SerializeField] private List<Transform> _points = new();
		
		private readonly Stack<int> _occupied = new();
		private HayBale[] _bales;
		private int _nextFreeIndex;

		public bool IsAvailable => gameObject.activeInHierarchy;
		public bool IsFull => _nextFreeIndex >= _points.Count;
		public bool IsHasAny => _occupied.Count > 0;

		private void Awake()
		{
			_bales = new HayBale[_points.Count];
			_occupied.Clear();
			_nextFreeIndex = 0;
		}
		
		public bool TryPlace(HayBale bale)
		{
			if (!IsAvailable || IsFull)
				return false;

			var index = _nextFreeIndex++;

			_bales[index] = bale;
			_occupied.Push(index);

			bale.transform.SetParent(_points[index], false);
			bale.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			return true;
		}

		public bool TryPopLast(out HayBale bale)
		{
			while (_occupied.Count > 0)
			{
				var index = _occupied.Pop();

				bale = _bales[index];
				_bales[index] = null;

				_nextFreeIndex = Mathf.Max(0, _nextFreeIndex - 1);

				if (bale != null)
					return true;
			}

			bale = null;
			return false;
		}

		public void SetEmpty()
		{
			for (int i = 0; i < _bales.Length; i++)
			{
				var bale = _bales[i];
				
				if (bale != null)
					bale.Destroy();

				_bales[i] = null;
			}

			_occupied.Clear();
			_nextFreeIndex = 0;
		}

	}
}