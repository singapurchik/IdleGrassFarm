using UnityEngine.InputSystem;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Players
{
	public class PlayerInputInstaller : MonoInstaller
	{
		[SerializeField] private InputActionReference _joystickMoveAction;
		[SerializeField] private PlayerInput _input;

		public override void InstallBindings()
		{
			Container.BindInstance(_joystickMoveAction).WhenInjectedIntoInstance(_input);
			Container.Bind<IPlayerInputInfo>().FromInstance(_input).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_input = GetComponentInChildren<PlayerInput>(true);
		}
#endif
	}
}