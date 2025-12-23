using UnityEngine;
using Zenject;

namespace IGF.Buyers.Animations
{
	public class BuyerAnimator : CharacterAnimator
	{
		[SerializeField] private float _idleAnimSpeedMultiplier = 1f;
		[SerializeField] private float _walkAnimSpeedMultiplier = 1f;
		[SerializeField] private float _runAnimSpeedMultiplier = 1f;
		
		[Inject] private UpperBodyLayer _upperBodyLayer;
		[Inject] private BaseLayer _baseLayer;
		
		protected float RequestedChangeLocomotionLerpSpeed;
		protected float CurrentChangeLocomotionLerpSpeed;
		protected float CurrentLocomotionSpeedMultiplier;
		protected float RequestedSetLocomotionValue;

		protected bool IsChangeLocomotionLerpSpeedRequested;
		protected bool IsChangeLocomotionValueRequested;
		
		public Quaternion DeltaRotation => Animator.deltaRotation;
		public Vector3 DeltaPosition => Animator.deltaPosition;
		public Vector3 Velocity => Animator.velocity;
		
		private readonly int _locomotionSpeedMultiplier = Animator.StringToHash("locomotionSpeedMultiplier");

		protected const float DEFAULT_LOCOMOTION_LERP_SPEED = 1f;
		protected const float IDLE_LOCOMOTION_VALUE = 0f;
		protected const float WALK_LOCOMOTION_VALUE = 0.5f;
		protected const float RUN_LOCOMOTION_VALUE = 1f;

		protected override void Awake()
		{
			base.Awake();
			CurrentLocomotionSpeedMultiplier = Animator.GetFloat(_locomotionSpeedMultiplier);
		}

		private void OnEnable()
		{
			_baseLayer.SyncLocomotionValue();
		}

		public void RequestChangeLocomotionLerpSpeed(float speed)
		{
			RequestedChangeLocomotionLerpSpeed = speed;
			IsChangeLocomotionLerpSpeedRequested = true;
		}
		
		public void PlayHoldingHayBaleAnim() => _upperBodyLayer.PlayHoldingHayBaleAnim();
		
		public void StopHoldingHayBaleAnim() => _upperBodyLayer.StopHoldingHayBaleAnim();

		public void RequestWalkAnim() => RequestSetLocomotion(WALK_LOCOMOTION_VALUE);

		public void RequestRunAnim() => RequestSetLocomotion(RUN_LOCOMOTION_VALUE);
		
		public void Restart() => Animator.Rebind();

		private void RequestSetLocomotion(float normalizedValue)
		{
			RequestedSetLocomotionValue = normalizedValue;
			IsChangeLocomotionValueRequested = true;
		}

		protected void UpdateLocomotionSpeedMultiplier(float oldLocomotionValue, float targetLocomotionValue,
			float currentLocomotionValue)
		{
			var targetMultiplier = targetLocomotionValue switch
			{
				IDLE_LOCOMOTION_VALUE => _idleAnimSpeedMultiplier,
				WALK_LOCOMOTION_VALUE => _walkAnimSpeedMultiplier,
				RUN_LOCOMOTION_VALUE => _runAnimSpeedMultiplier,
				_ => 1f
			};

			CurrentLocomotionSpeedMultiplier = Mathf.Lerp(CurrentLocomotionSpeedMultiplier, targetMultiplier,
				Mathf.InverseLerp(oldLocomotionValue, targetLocomotionValue, currentLocomotionValue));
			
			Animator.SetFloat(_locomotionSpeedMultiplier, CurrentLocomotionSpeedMultiplier);	
		}

		protected void AutoLayerWeightControl(Layer layer) => AutoLayerWeightControl(layer, layer.IsActive);
		
		protected void AutoLayerWeightControl(Layer layer, bool isActive)
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
					CurrentChangeLocomotionLerpSpeed * Time.deltaTime);
			
				UpdateLocomotionSpeedMultiplier(_baseLayer.LocomotionValue, normalizedValue, targetLayerValue);
				_baseLayer.SetLocomotionValue(targetLayerValue);
			}
		}

		private void Update()
		{
			if (IsChangeLocomotionLerpSpeedRequested)
			{
				CurrentChangeLocomotionLerpSpeed = RequestedChangeLocomotionLerpSpeed;
				IsChangeLocomotionLerpSpeedRequested = false;
			}

			if (IsChangeLocomotionValueRequested)
			{
				TryChangeLocomotionValueSmooth(RequestedSetLocomotionValue);
				IsChangeLocomotionValueRequested = false;
			}
			else if (_baseLayer.LocomotionValue > 0)
			{
				TryChangeLocomotionValueSmooth(IDLE_LOCOMOTION_VALUE);
			}

			AutoLayerWeightControl(_upperBodyLayer);
			
			CurrentChangeLocomotionLerpSpeed = DEFAULT_LOCOMOTION_LERP_SPEED;
		}
	}
}