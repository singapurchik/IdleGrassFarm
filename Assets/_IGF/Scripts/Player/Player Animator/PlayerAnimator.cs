using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF.Players.Animations
{
	public class PlayerAnimator : CharacterAnimator
	{
		[SerializeField] private float _changeHealthBehaviourTypeValueSpeed = 2f;
		[SerializeField] private float _increaseLocomotionSpeed = 2f;
		[SerializeField] private float _decreaseLocomotionSpeed = 2f;
		
		[InjectOptional] private List<ILayerWithTriggers> _layersWithTriggers;
		[Inject] private BaseLayer _baseLayer;
		
		private float _locomotionValueRequested;
		private float _requestedSpeed;
		
		private bool _isChangeLocomotionRequested;
		private bool _isChangeSpeedRequested;

		public IReadOnlyAnimatorLayer BaseLayer => _baseLayer;
		
		public Vector3 DeltaPosition => Animator.deltaPosition;
		
		public float GetLocomotionValue() => _baseLayer.LocomotionValue;
		
		public void RequestSetLocomotionValue(float value)
		{
			_isChangeLocomotionRequested = true;
			_locomotionValueRequested = value;
		}
		
		public void ForceSetLocomotionValue(float value) => _baseLayer.SetLocomotionValue(value);
		
		private void SmoothChangeLocomotionValue(float value, float speed)
		{
			var targetValue = Mathf.MoveTowards(_baseLayer.LocomotionValue,
				value, speed * Time.deltaTime);
			_baseLayer.SetLocomotionValue(targetValue);
		}
		
		public void ForceChangeLocomotionValue(float value)
		{
			_locomotionValueRequested = value;
			_baseLayer.SetLocomotionValue(_locomotionValueRequested);
		}

		private void UpdateLocomotionValue()
		{
			if (_isChangeLocomotionRequested)
			{
				if (!Mathf.Approximately(_baseLayer.LocomotionValue, _locomotionValueRequested))
				{
					if (_baseLayer.LocomotionValue < _locomotionValueRequested)
						SmoothChangeLocomotionValue(_locomotionValueRequested, _increaseLocomotionSpeed);
					else
						SmoothChangeLocomotionValue(_locomotionValueRequested, _decreaseLocomotionSpeed);
				}

				_isChangeLocomotionRequested = false;
			}
			else if (_baseLayer.LocomotionValue > 0)
			{
				SmoothChangeLocomotionValue(_locomotionValueRequested, _decreaseLocomotionSpeed);
			}
		}

		private void AutoLayerWeightControl(Layer layer)
		{
			if (layer.IsActive && !(layer.IsInTransition && layer.NextAnimHash == Layer.EmptyAnimHash))
			{
				if (!layer.IsEnabled)
					layer.EnableWeightSmooth();
			}
			else if (!layer.IsDisabled)
			{
				layer.DisableWeightSmooth();
			}
		}
		
		public void RequestChangeSpeed(float speed)
		{
			_requestedSpeed = speed;
			_isChangeSpeedRequested = true;
		}

		private void Update()
		{
			if (_isChangeSpeedRequested)
				Animator.speed = _requestedSpeed;
			else if (Animator.speed != 1)
				Animator.speed = 1;
				
			UpdateLocomotionValue();
			
			_isChangeSpeedRequested = false;
		}
	}
}