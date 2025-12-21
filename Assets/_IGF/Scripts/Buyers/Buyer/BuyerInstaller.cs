using UnityEngine.AI;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Buyers
{
	public class BuyerInstaller : MonoInstaller
	{
		[SerializeField] private Animator _animatorController;
		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private BuyerAnimator _animator;
		[SerializeField] private BuyerMover _mover;

		public override void InstallBindings()
		{
			Container.BindInstance(_animatorController).WhenInjectedIntoInstance(_animator);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_mover);
			Container.BindInstance(_animator).AsSingle();
			Container.BindInstance(_mover).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_animatorController = GetComponentInChildren<Animator>(true);
			_navMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			_animator = GetComponentInChildren<BuyerAnimator>(true);
			_mover = GetComponentInChildren<BuyerMover>(true);
		}
#endif
	}
}