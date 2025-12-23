using UnityEngine;

namespace IGF.Buyers
{
	public interface IBuyer : IHayBaleBuyer
	{
		public Vector3 Position { get; }
	}
}