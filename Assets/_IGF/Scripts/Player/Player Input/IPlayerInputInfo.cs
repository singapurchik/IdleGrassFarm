using UnityEngine;

namespace IGF.Players
{
	public interface IPlayerInputInfo
	{
		public Vector2 GetJoystickDirection2D();
		
		public Vector3 GetJoystickDirection3D();
	}
}