using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace IGF
{
	public class HaleBaleHolders
	{
		[Inject] private IReadOnlyList<IHayBaleHolder> _holders;
		
		public bool IsHasSpace => _holders.Any(holder => holder.IsAvailable && !holder.IsFull);

		public bool TryGetFreePoint(out Transform freePoint)
		{
			for (int i = 0; i < _holders.Count; i++)
			{
				var holder = _holders[i];
				
				if (!holder.IsAvailable) continue;
				
				if (holder.TryGetPoint(out var point))
				{
					freePoint = point;
					return true;
				}
			}
			
			freePoint = null;
			return false;
		}
	}
}