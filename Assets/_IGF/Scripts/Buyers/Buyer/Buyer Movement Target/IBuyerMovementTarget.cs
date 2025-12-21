using UnityEngine;

namespace IGF.Buyers
{
	public interface IBuyerMovementTarget
	{
		public BuyerMovementTargetType Type { get; }
		
		public Vector3 Position { get; }
		
		public bool IsHasTarget { get; }
		
		public void Set(Vector3 position, BuyerMovementTargetType type);
		
		public void Clear();
	}
}