using UnityEngine;

namespace IGF
{
	public interface IHayBaleHolder
	{
		public bool TryGetPoint(out Transform point);
	}
}