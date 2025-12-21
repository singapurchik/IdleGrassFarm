using IGF.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Players
{
	public class PlayerStateMachineInstaller : MonoInstaller
	{
		[SerializeField] private PlayerStateMachine _stateMachine;
		[SerializeField] private FreeRun _freeRunState;
		[SerializeField] private Attack _attackState;

		public override void InstallBindings()
		{
			BindInstanceToStateMachine(_freeRunState);
			BindInstanceToStateMachine(_attackState);

			Container.Bind<IPlayerStateMachineInfo>().FromInstance(_stateMachine).AsSingle();
			Container.BindInstance(_stateMachine).WhenInjectedInto<Player>();
		}
		
		private void BindInstanceToStateMachine<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<PlayerStateMachine>();
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_stateMachine = GetComponentInChildren<PlayerStateMachine>(true);
			_attackState = GetComponentInChildren<Attack>(true);
			_freeRunState = GetComponentInChildren<FreeRun>(true);
		}
#endif
	}
}