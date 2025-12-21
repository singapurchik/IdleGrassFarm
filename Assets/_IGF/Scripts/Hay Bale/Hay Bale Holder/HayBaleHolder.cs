using System.Collections.Generic;
using UnityEngine;

namespace IGF
{
	public sealed class HayBaleHolder : MonoBehaviour, IHayBaleHolder
	{
		[SerializeField] private List<Transform> _points = new();
		[SerializeField] private bool _isAvailable = true;

		private bool[] _occupied;

		public bool IsAvailable => _isAvailable;
		public bool IsFull
		{
			get
			{
				for (int i = 0; i < _occupied.Length; i++)
					if (!_occupied[i]) return false;
				return true;
			}
		}

		private void Awake()
		{
			_occupied = new bool[_points.Count];
		}

		public bool TryReserve(out HayBaleSlot slot)
		{
			for (int i = 0; i < _points.Count; i++)
			{
				if (_occupied[i]) continue;

				_occupied[i] = true;
				slot = new HayBaleSlot(this, i, _points[i]);
				return true;
			}

			slot = default;
			return false;
		}

		public void Release(in HayBaleSlot slot)
		{
			// Защита: освобождаем только “свой” слот
			if (!ReferenceEquals(slot.Holder, this))
				return;

			if ((uint)slot.Index >= (uint)_occupied.Length)
				return;

			_occupied[slot.Index] = false;
		}
	}
}