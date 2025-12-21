using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace IGF
{
	public sealed class HaleBaleHolders
	{
		[Inject] private IReadOnlyList<HayBaleHolder> _holders;

		public bool IsHasAnySpace => _holders.Any(holder => holder.IsAvailable && !holder.IsFull);

		public bool TryPlace(HayBale bale)
		{
			for (int i = 0; i < _holders.Count; i++)
				if (_holders[i].TryPlace(bale))
					return true;

			return false;
		}

		public bool TryPopLast(out HayBale bale)
		{
			for (int i = _holders.Count - 1; i >= 0; i--)
				if (_holders[i].TryPopLast(out bale))
					return true;

			bale = null;
			return false;
		}
	}
}