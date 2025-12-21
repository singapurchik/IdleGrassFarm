using UnityEngine;

namespace IGF.Buyers
{
	public interface IMovementTargetHolder
	{
		public bool IsHasTarget { get; }
		
		public Vector3 TargetPosition { get; }

		public void ClearTarget();
	}
}