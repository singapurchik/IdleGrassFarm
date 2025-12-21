using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Buyers
{
	public class BuyersInstaller : MonoInstaller
	{
		[SerializeField] private BuyersSpawner _spawner;
		[SerializeField] private BuyersPool _pool;
		[SerializeField] private List<Transform> _queuePoints = new ();
		
		private readonly BuyersQueue _buyersQueue = new ();

		public override void InstallBindings()
		{
			Container.Bind<IReadOnlyList<Transform>>().FromInstance(_queuePoints).WhenInjectedIntoInstance(_buyersQueue);
			Container.BindInstance(_buyersQueue).WhenInjectedIntoInstance(_spawner);
			Container.BindInstance(_pool).WhenInjectedIntoInstance(_spawner);
			Container.QueueForInject(_buyersQueue);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_spawner = GetComponentInChildren<BuyersSpawner>(true);
			_pool = GetComponentInChildren<BuyersPool>(true);
		}
#endif
	}
}