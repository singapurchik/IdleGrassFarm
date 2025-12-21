using UnityEngine;

namespace IGF
{
	public readonly struct HayBaleSlot
	{
		public readonly IHayBaleHolder Holder;
		public readonly Transform Point;
		
		public readonly int Index;

		public HayBaleSlot(IHayBaleHolder holder, int index, Transform point)
		{
			Holder = holder;
			Index = index;
			Point = point;
		}
	}
}