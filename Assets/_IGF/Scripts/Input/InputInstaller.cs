using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

namespace IGF
{
	public class InputInstaller : MonoInstaller
	{
		[SerializeField] private InputActionReference _joystickMoveAction;

		public override void InstallBindings()
		{
			Container.BindInstance(_joystickMoveAction).WhenInjectedInto<Input>();
		}
	}
}