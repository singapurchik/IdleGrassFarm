using IGF.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF
{
	public class HayBalesInstaller : MonoInstaller
	{
		[SerializeField] private HayBalesDistributor _distributor;
		[SerializeField] private HayBaleSaleZone _saleZone;
		[SerializeField] private HayBalePool _yellowPool;
		[SerializeField] private HayBalePool _greenPool;

		private readonly HayBaleHolders _holders = new ();
		private readonly HayBaleSpawner _spawner = new ();

		public override void InstallBindings()
		{
			Container.BindInstance(_yellowPool).WithId(HayBaleType.Yellow).WhenInjectedIntoInstance(_spawner);
			Container.BindInstance(_greenPool).WithId(HayBaleType.Green).WhenInjectedIntoInstance(_spawner);
			Container.Bind<IHayBaleSpawnEvents>().FromInstance(_spawner).WhenInjectedIntoInstance(_distributor);
			Container.Bind<IHayBaleHoldersInfo>().FromInstance(_holders).WhenInjectedInto<PlayerState>();
			Container.Bind<IHayBaleHolders>().FromInstance(_holders).WhenInjectedInto<HayBaleHolder>();
			Container.Bind<IHayBaleSpawner>().FromInstance(_spawner).WhenInjectedInto<Grass>();
			Container.Bind<IHayBalesDistributor>().FromInstance(_distributor).AsSingle();
			Container.BindInstance(_holders).WhenInjectedIntoInstance(_distributor);
			Container.BindInstance(_holders).WhenInjectedIntoInstance(_saleZone);
			Container.QueueForInject(_holders);
			Container.QueueForInject(_spawner);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_distributor = GetComponentInChildren<HayBalesDistributor>(true);
			_saleZone = GetComponentInChildren<HayBaleSaleZone>(true);
		}
#endif
	}
}