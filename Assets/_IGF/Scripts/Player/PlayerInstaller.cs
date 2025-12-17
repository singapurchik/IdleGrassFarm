using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using IGF.Players.Animations;
using IGF.Players.AnimRig;
using IGF.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF.Players
{
	public class PlayerInstaller : MonoInstaller
	{
		[SerializeField] private PlayerAttackTargetsFinder _attackTargetsFinder;
		[SerializeField] private PlayerAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private PlayerAnimationRigging _animationRigging;
		[SerializeField] private PlayerVisualEffects _visualEffects;
		[SerializeField] private Animator _animatorController;
		[SerializeField] private PlayerAnimator _animator;
		[SerializeField] private RigBuilder _rigBuilder;
		[SerializeField] private PlayerRotator _rotator;
		[SerializeField] private PlayerMover _mover;
		[SerializeField] private PlayerZone _zone;
		[SerializeField] private Transform _body;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_rigBuilder).WhenInjectedIntoInstance(_animationRigging);

			Container.Bind<IPlayerZoneInfo>().FromInstance(_zone).AsSingle();
			
			Container.BindInstance(transform).WithId(CharacterTransformType.Root).AsCached();
			Container.BindInstance(_body).WithId(CharacterTransformType.Body).AsCached();
			Container.BindInstance(_characterController).AsSingle();
			Container.BindInstance(_animEventsReceiver).AsSingle();
			Container.BindInstance(_animationRigging).AsSingle();
			Container.BindInstance(_visualEffects).AsSingle();
			Container.BindInstance(_animator).AsSingle();
			Container.BindInstance(_rotator).AsSingle();
			Container.BindInstance(_mover).AsSingle();

			BindToStatesMachine();
			BindAnimator();
		}

		private void BindToStatesMachine()
		{
			Container.BindInstance(_attackTargetsFinder).WhenInjectedInto<Attack>();
		}

		private void BindAnimator()
		{
			var triggersList = new List<ILayerWithTriggers>(10);
			var layersList = new List<Layer>(10);
			
			BindToAnimator(new BaseLayer(_animatorController, 0, layersList));
			BindToAnimator(new WeaponLayer(_animatorController, 1, layersList));
			
			BindToAnimator(triggersList);
			BindToAnimator(layersList);
			BindToAnimator(_animatorController);
		}

		private void BindToAnimator<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_animator);
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_attackTargetsFinder = GetComponentInChildren<PlayerAttackTargetsFinder>(true);
			_animEventsReceiver = GetComponentInChildren<PlayerAnimEventsReceiver>(true);
			_animationRigging = GetComponentInChildren<PlayerAnimationRigging>(true);
			_characterController = GetComponentInChildren<CharacterController>(true);
			_visualEffects = GetComponentInChildren<PlayerVisualEffects>(true);
			_animatorController = GetComponentInChildren<Animator>(true);
			_animator = GetComponentInChildren<PlayerAnimator>(true);
			_rigBuilder = GetComponentInChildren<RigBuilder>(true);
			_rotator = GetComponentInChildren<PlayerRotator>(true);
			_mover = GetComponentInChildren<PlayerMover>(true);
			_zone = GetComponentInChildren<PlayerZone>(true);
		}
#endif
	}
}