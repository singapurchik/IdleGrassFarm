using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace IGF
{
	public sealed class HaleBaleHolders
	{
		[Inject] private IReadOnlyList<IHayBaleHolder> _holders;

		private readonly Stack<HayBaleSlot> _occupied = new(64);

		public bool IsHasSpace => _holders.Any(h => h.IsAvailable && !h.IsFull);
		public bool IsHasAnyBales => _occupied.Count > 0;

		public bool TryReserveFreeSlot(out HayBaleSlot slot)
		{
			for (int i = 0; i < _holders.Count; i++)
			{
				var holder = _holders[i];
				
				if (!holder.IsAvailable || holder.IsFull) 
					continue;

				if (holder.TryReserve(out slot))
				{
					_occupied.Push(slot);
					return true;
				}
			}

			slot = default;
			return false;
		}

		public bool TryTakeLast(out HayBaleSlot slot)
		{
			while (_occupied.Count > 0)
			{
				slot = _occupied.Pop();

				if (slot.Holder == null)
					continue;

				slot.Holder.Release(slot);
				return true;
			}

			slot = default;
			return false;
		}
	}
}