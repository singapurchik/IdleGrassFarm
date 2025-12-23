using System.Collections.Generic;

namespace IGF
{
	public sealed class HayBaleHolders : IHayBaleHolders
	{
		private readonly List<HayBaleHolder> _orderedHolders = new(16);
		private readonly HashSet<HayBaleHolder> _allHolders = new(16);
		
		void IHayBaleHolders.Add(HayBaleHolder holder)
		{
			if (_allHolders.Add(holder))
				_orderedHolders.Add(holder);
		}

		public bool IsHasAnySpace()
		{
			for (int i = 0; i < _orderedHolders.Count; i++)
			{
				var holder = _orderedHolders[i];
				if (holder.IsAvailable && !holder.IsFull)
					return true;
			}
			return false;
		}

		public bool IsAllEmpty()
		{
			for (int i = 0; i < _orderedHolders.Count; i++)
			{
				var holder = _orderedHolders[i];
				if (holder.IsAvailable && holder.IsHasAny)
					return false;
			}
			return true;
		}

		public bool TryPlace(HayBale bale)
		{
			for (int i = 0; i < _orderedHolders.Count; i++)
				if (_orderedHolders[i].TryPlace(bale))
					return true;

			return false;
		}

		public bool TryPopLast(out HayBale bale)
		{
			for (int i = _orderedHolders.Count - 1; i >= 0; i--)
				if (_orderedHolders[i].TryPopLast(out bale))
					return true;

			bale = null;
			return false;
		}
	}
}