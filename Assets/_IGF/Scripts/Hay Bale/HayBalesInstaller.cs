using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF
{
	public class HayBalesInstaller : MonoInstaller
	{
		[SerializeField] private HayBalesDistributor _distributor;
		[SerializeField] private HayBalePool _hayBaleYellowPool;
		[SerializeField] private HayBalePool _hayBaleGreenPool;
		[SerializeField] private List<HayBaleHolder> _hayBaleHolder = new ();

		private readonly HaleBaleHolders _holders = new ();
		private readonly HayBaleSpawner _spawner = new ();

		public override void InstallBindings()
		{
			Container.BindInstance(_hayBaleYellowPool).WithId(HayBaleType.Yellow).WhenInjectedIntoInstance(_spawner);
			Container.BindInstance(_hayBaleGreenPool).WithId(HayBaleType.Green).WhenInjectedIntoInstance(_spawner);
			Container.Bind<IReadOnlyList<HayBaleHolder>>().FromInstance(_hayBaleHolder).WhenInjectedIntoInstance(_holders);
			Container.Bind<IHayBaleSpawnEvents>().FromInstance(_spawner).WhenInjectedIntoInstance(_distributor);
			Container.Bind<IHayBaleSpawner>().FromInstance(_spawner).WhenInjectedInto<Grass>();
			Container.BindInstance(_holders).AsSingle();
			Container.QueueForInject(_holders);
			Container.QueueForInject(_spawner);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_hayBaleHolder.Clear();
			_hayBaleHolder.AddRange(GetComponentsInChildren<HayBaleHolder>(true));
			
			_distributor = GetComponent<HayBalesDistributor>();
		}
#endif
	}
}