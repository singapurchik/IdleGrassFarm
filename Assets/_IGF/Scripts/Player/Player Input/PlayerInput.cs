using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

namespace IGF.Players
{
	public class PlayerInput : MonoBehaviour, IPlayerInputInfo
	{
		[Inject] private InputActionReference _joystickMoveAction;
		
		private void OnEnable()
		{
			_joystickMoveAction.action.Enable();
		}

		private void OnDisable()
		{
			_joystickMoveAction.action.Enable();
		}

		public Vector2 GetJoystickDirection2D() => _joystickMoveAction.action.ReadValue<Vector2>();
		
		public Vector3 GetJoystickDirection3D()
		{
			var vector = _joystickMoveAction.action.ReadValue<Vector2>();
			return new Vector3(vector.x, 0f, vector.y).normalized;
		}
	}
}