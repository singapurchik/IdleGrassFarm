using UnityEngine;

namespace IGF
{
	public interface IHayBaleHolder
	{
		public bool IsAvailable { get; }
		public bool IsFull { get; }
		
		public bool TryGetPoint(out Transform point);
	}
}