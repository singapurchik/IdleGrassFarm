using UnityEngine;

namespace IGF.Buyers
{
	public class BuyerMovementTarget : IBuyerMovementTarget
	{
		public BuyerMovementTargetType Type { get; private set; }
		
		public Vector3 Position { get; private set; }
		
		public bool IsHasTarget { get; private set; }

		public void Set(Vector3 position, BuyerMovementTargetType type)
		{
			Position = position;
			Type = type;
			IsHasTarget = true;
		}

		public void Clear() => IsHasTarget = false;
	}
}