using UnityEngine;

namespace IGF.Buyers
{
	public interface IMovementTargetHolder
	{
		public bool IsHasTarget { get; }
		
		public Transform CurrentMovementTarget { get; }

		public void ClearTarget();
	}
}