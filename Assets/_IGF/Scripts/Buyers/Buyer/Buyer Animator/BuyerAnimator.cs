using UnityEngine;
using Zenject;

namespace IGF.Buyers.Animations
{
	public sealed class BuyerAnimator : CharacterAnimator
	{
		[SerializeField] private float _idleAnimSpeedMultiplier = 1f;
		[SerializeField] private float _walkAnimSpeedMultiplier = 1f;
		[SerializeField] private float _runAnimSpeedMultiplier = 1f;
		
		[Inject] private UpperBodyLayer _upperBodyLayer;
		[Inject] private BaseLayer _baseLayer;
		
		private float _currentChangeLocomotionLerpSpeed;
		private float _requestedSetLocomotionValue;

		private bool _isChangeLocomotionValueRequested;
		
		public Quaternion DeltaRotation => Animator.deltaRotation;
		public Vector3 DeltaPosition => Animator.deltaPosition;
		public Vector3 Velocity => Animator.velocity;
		
		private const float DEFAULT_LOCOMOTION_LERP_SPEED = 1f;
		private const float IDLE_LOCOMOTION_VALUE = 0f;
		private const float WALK_LOCOMOTION_VALUE = 0.5f;
		private const float RUN_LOCOMOTION_VALUE = 1f;

		private void OnEnable()
		{
			_baseLayer.SyncLocomotionValue();
		}
		
		public void PlayHoldingHayBaleAnim() => _upperBodyLayer.PlayHoldingHayBaleAnim();
		
		public void StopHoldingHayBaleAnim() => _upperBodyLayer.StopHoldingHayBaleAnim();

		public void RequestWalkAnim() => RequestSetLocomotion(WALK_LOCOMOTION_VALUE);

		private void RequestSetLocomotion(float normalizedValue)
		{
			_requestedSetLocomotionValue = normalizedValue;
			_isChangeLocomotionValueRequested = true;
		}

		private void AutoLayerWeightControl(Layer layer) => AutoLayerWeightControl(layer, layer.IsActive);
		
		private void AutoLayerWeightControl(Layer layer, bool isActive)
		{
			if (isActive)
			{
				if (!layer.IsEnabled)
					layer.EnableWeightSmooth();
			}
			else if (!layer.IsDisabled)
			{
				layer.DisableWeightSmooth();
			}
		}
		
		private void TryChangeLocomotionValueSmooth(float normalizedValue)
		{
			if (_baseLayer.LocomotionValue != normalizedValue)
			{
				var targetLayerValue = Mathf.MoveTowards(_baseLayer.LocomotionValue, normalizedValue, 
					_currentChangeLocomotionLerpSpeed * Time.deltaTime);
			
				_baseLayer.SetLocomotionValue(targetLayerValue);
			}
		}

		private void Update()
		{
			if (_isChangeLocomotionValueRequested)
			{
				TryChangeLocomotionValueSmooth(_requestedSetLocomotionValue);
				_isChangeLocomotionValueRequested = false;
			}
			else if (_baseLayer.LocomotionValue > 0)
			{
				TryChangeLocomotionValueSmooth(IDLE_LOCOMOTION_VALUE);
			}

			AutoLayerWeightControl(_upperBodyLayer);
			
			_currentChangeLocomotionLerpSpeed = DEFAULT_LOCOMOTION_LERP_SPEED;
		}
	}
}