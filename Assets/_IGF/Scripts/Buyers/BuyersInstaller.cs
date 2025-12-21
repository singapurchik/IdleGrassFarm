using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Buyers
{
	public class BuyersInstaller : MonoInstaller
	{
		[SerializeField] private BuyersSpawnScheduler _spawnScheduler;
		[SerializeField] private BuyersQueue _buyersQueue;
		[SerializeField] private BuyersPool _pool;
		[SerializeField] private List<Transform> _queuePoints = new ();
		
		public override void InstallBindings()
		{
			Container.Bind<IReadOnlyList<Transform>>().FromInstance(_queuePoints).WhenInjectedIntoInstance(_buyersQueue);
			Container.BindInstance(_buyersQueue).WhenInjectedIntoInstance(_spawnScheduler);
			Container.BindInstance(_pool).WhenInjectedIntoInstance(_spawnScheduler);
			Container.QueueForInject(_buyersQueue);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_spawnScheduler = GetComponentInChildren<BuyersSpawnScheduler>(true);
			_buyersQueue = GetComponentInChildren<BuyersQueue>(true);
			_pool = GetComponentInChildren<BuyersPool>(true);
		}
#endif
	}
}