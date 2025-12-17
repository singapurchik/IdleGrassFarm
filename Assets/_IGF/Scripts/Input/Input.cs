using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

namespace IGF
{
	public class Input : MonoBehaviour
	{
		[Inject] private InputActionReference _joystickMoveAction;
		
		private bool _isBlockMovementInputRequested;
		private bool _isBlockButtonsInputRequested;
		
		public bool IsMovementJoystickActive { get; private set; } = true;
		public bool IsButtonsInputActive { get; private set; } = true;

		private void OnEnable()
		{
			_joystickMoveAction.action.Enable();
		}

		private void OnDisable()
		{
			_joystickMoveAction.action.Enable();
		}
		
		public Vector2 GetJoystickDirection2D()
		{
			var moveVector = Vector2.zero;
			
			if (IsMovementJoystickActive)
				moveVector = _joystickMoveAction.action.ReadValue<Vector2>();
			
			return moveVector;
		}
		
		private void Update()
		{
			IsMovementJoystickActive = !_isBlockMovementInputRequested;
			_isBlockMovementInputRequested = false;
			
			IsButtonsInputActive = !_isBlockButtonsInputRequested;
			_isBlockButtonsInputRequested = false;
		}
	}
}