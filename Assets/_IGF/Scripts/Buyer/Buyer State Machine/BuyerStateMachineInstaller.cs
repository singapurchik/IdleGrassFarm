using IGF.Buyers.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Buyers
{
	public class BuyerStateMachineInstaller : MonoInstaller
	{
		[SerializeField] private BuyerStateMachine _stateMachine;
		[SerializeField] private Idle _idleState;
		
		public override void InstallBindings()
		{
			BindInstanceToStateMachine(_idleState);
			
			Container.Bind<IBuyerStateMachineInfo>().FromInstance(_stateMachine).AsSingle();
			Container.BindInstance(_stateMachine).WhenInjectedInto<Buyer>();
		}
		
		private void BindInstanceToStateMachine<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<BuyerStateMachine>();

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_stateMachine = GetComponent<BuyerStateMachine>();
			_idleState = GetComponent<Idle>();
		}
#endif
	}
}