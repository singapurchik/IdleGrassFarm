using System.Collections.Generic;
using IGF.Buyers.Animations;
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
		[SerializeField] private BuyerRotator _rotator;
		[SerializeField] private BuyerMover _mover;
		[SerializeField] private Buyer _buyer;

		public override void InstallBindings()
		{
			Container.BindInstance(_animatorController).WhenInjectedIntoInstance(_animator);
			Container.Bind<IMovementTargetHolder>().FromInstance(_buyer).AsSingle();
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_rotator);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_mover);
			Container.BindInstance(_rotator).AsSingle();
			Container.BindInstance(_animator).AsSingle();
			Container.BindInstance(_mover).AsSingle();

			CreateAndBindAnimationLayers();
		}

		private void CreateAndBindAnimationLayers()
		{
			var triggersList = new List<ILayerWithTriggers>(10);
			var layersList = new List<Layer>(10);

			BindInstanceToAnimator(new BaseLayer(_animatorController, 0, layersList));
			BindInstanceToAnimator(triggersList);
			BindInstanceToAnimator(layersList);
		}
		
		private void BindInstanceToAnimator<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<BuyerAnimator>();

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_animatorController = GetComponentInChildren<Animator>(true);
			_navMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			_animator = GetComponentInChildren<BuyerAnimator>(true);
			_rotator = GetComponentInChildren<BuyerRotator>(true);
			_mover = GetComponentInChildren<BuyerMover>(true);
			_buyer = GetComponentInChildren<Buyer>(true);
		}
#endif
	}
}