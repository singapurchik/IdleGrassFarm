using UnityEngine;

namespace IGF
{
	public interface ICartTarget
	{
		public Vector3 Position { get; }
		public Vector3 Forward { get; }
	}
}