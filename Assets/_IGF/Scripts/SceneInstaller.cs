using IGF.Upgrades.Carts;
using IGF.Upgrades;
using IGF.Players;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF
{
	public class SceneInstaller : MonoInstaller
	{
		[SerializeField] private CartSpawner _cartSpawner;
		[SerializeField] private Camera _mainCamera;
		[SerializeField] private Player _player;

		public override void InstallBindings()
		{
			Container.Bind<IAttackRangeUpgrader>().FromInstance(_player).WhenInjectedInto<AttackRangeDepositZone>();
			Container.Bind<ICartSpawner>().FromInstance(_cartSpawner).WhenInjectedInto<CartDepositZone>();
			Container.BindInstance(_mainCamera).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_cartSpawner = FindFirstObjectByType<CartSpawner>(FindObjectsInactive.Include);
			_mainCamera = FindFirstObjectByType<Camera>(FindObjectsInactive.Include);
			_player = FindFirstObjectByType<Player>(FindObjectsInactive.Include);
		}
#endif
	}
}