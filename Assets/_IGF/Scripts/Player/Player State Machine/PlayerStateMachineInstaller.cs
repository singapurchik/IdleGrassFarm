using IGF.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Players
{
	public class PlayerStateMachineInstaller : MonoInstaller
	{
		[SerializeField] private PlayerStateMachine _stateMachine;
		[SerializeField] private Free _freeState;

		public override void InstallBindings()
		{
			BindInstanceToStateMachine(_freeState);

			Container.Bind<IPlayerStateReturner>().FromInstance(_stateMachine).WhenInjectedInto<PlayerState>();
			Container.Bind<IPlayerStateMachineInfo>().FromInstance(_stateMachine).AsSingle();
		}
		
		private void BindInstanceToStateMachine<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<PlayerStateMachine>();
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_stateMachine = GetComponentInChildren<PlayerStateMachine>(true);
			_freeState = GetComponentInChildren<Free>(true);
		}
#endif
	}
}