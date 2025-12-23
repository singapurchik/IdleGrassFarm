using UnityEngine;

namespace IGF
{
	public sealed class CartMover : MonoBehaviour
	{
		[Header("Follow Offset (Behind Target)")]
		[SerializeField, Min(0f)] private float _maxDistance = 1.25f;
		[SerializeField, Min(0f)] private float _stopDistance = 0.15f;

		[Header("Car-like Movement")]
		[SerializeField, Min(0f)] private float _maxForwardSpeed = 6f;
		[SerializeField, Min(0f)] private float _acceleration = 18f;
		[SerializeField, Min(0f)] private float _brake = 28f;

		[Header("Steering")]
		[SerializeField, Min(0f)] private float _rotationSpeedDeg = 360f;
		[SerializeField, Range(0f, 180f)] private float _stopMoveAngleDeg = 65f;
		[SerializeField, Range(0f, 180f)] private float _slowMoveAngleDeg = 25f;
		[SerializeField, Range(0f, 180f)] private float _finishFacingAngleDeg = 5f;

		[Header("Lag Limit")]
		[SerializeField, Min(0f)] private float _maxLagDistance = 3f;
		[SerializeField, Min(0f)] private float _catchUpSpeed = 10f;
		[SerializeField, Min(1f)] private float _catchUpRotMultiplier = 1.5f;
		
		private ICartTarget _currentTarget;

		private float _currentSpeed;
		
		public void SetTarget(ICartTarget target) => _currentTarget = target;

		private Vector3 GetDesiredPosBehindTarget()
		{
			var targetPos = _currentTarget.Position;

			var behindDir = -_currentTarget.Forward;
			behindDir.y = 0f;

			if (behindDir.sqrMagnitude < 0.0001f)
			{
				behindDir = -transform.forward;
				behindDir.y = 0f;
			}

			behindDir = behindDir.sqrMagnitude > 0.0001f ? behindDir.normalized : Vector3.back;

			return targetPos + behindDir * _maxDistance;
		}

		private void UpdateSteeringAndMove(Vector3 desiredPos)
		{
			var toDesired = desiredPos - transform.position;
			toDesired.y = 0f;

			var dist = toDesired.magnitude;

			var finishFacing = _currentTarget.Forward;
			finishFacing.y = 0f;
			finishFacing = finishFacing.sqrMagnitude > 0.0001f ? finishFacing.normalized : GetPlanarForward();

			if (dist <= _stopDistance)
			{
				_currentSpeed = MoveTowards(_currentSpeed, 0f, _brake * Time.deltaTime);

				var planarForward = GetPlanarForward();
				var finishAngle = Vector3.Angle(planarForward, finishFacing);

				var finishRot = Quaternion.LookRotation(finishFacing, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, finishRot, _rotationSpeedDeg * Time.deltaTime);

				if (finishAngle > _finishFacingAngleDeg)
					return;

				_currentSpeed = 0f;
				return;
			}

			var steerDir = toDesired / dist;

			var lagEnabled = _maxLagDistance > 0f;
			var isLagging = lagEnabled && dist >= _maxLagDistance;

			var rotSpeed = _rotationSpeedDeg * (isLagging ? _catchUpRotMultiplier : 1f);
			var moveRot = Quaternion.LookRotation(steerDir, Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, moveRot, rotSpeed * Time.deltaTime);

			var targetSpeed = isLagging
				? Mathf.Max(_maxForwardSpeed, _catchUpSpeed)
				: _maxForwardSpeed;

			if (!isLagging)
			{
				var planarForward = GetPlanarForward();
				var steerAngle = Vector3.Angle(planarForward, steerDir);

				if (steerAngle >= _stopMoveAngleDeg)
				{
					targetSpeed = 0f;
				}
				else if (steerAngle > _slowMoveAngleDeg)
				{
					var t = Mathf.InverseLerp(_stopMoveAngleDeg, _slowMoveAngleDeg, steerAngle); // 0..1
					targetSpeed *= t;
				}
			}

			if (_currentSpeed < targetSpeed)
				_currentSpeed = MoveTowards(_currentSpeed, targetSpeed, _acceleration * Time.deltaTime);
			else
				_currentSpeed = MoveTowards(_currentSpeed, targetSpeed, _brake * Time.deltaTime);

			transform.position += transform.forward * (_currentSpeed * Time.deltaTime);

			if (lagEnabled)
			{
				var after = desiredPos - transform.position;
				after.y = 0f;

				var afterSqr = after.sqrMagnitude;
				var maxLagSqr = _maxLagDistance * _maxLagDistance;

				if (afterSqr > maxLagSqr && afterSqr > 0.0001f)
				{
					var dir = after.normalized;
					transform.position = desiredPos - dir * _maxLagDistance;
				}
			}
		}

		private Vector3 GetPlanarForward()
		{
			var forward = transform.forward;
			forward.y = 0f;
			return forward.sqrMagnitude > 0.0001f ? forward.normalized : Vector3.forward;
		}

		private static float MoveTowards(float current, float target, float maxDelta)
		{
			if (current < target)
				return Mathf.Min(current + maxDelta, target);
			
			return Mathf.Max(current - maxDelta, target);
		}
		
		private void Update()
		{
			if (_currentTarget != null)
			{
				var desiredPos = GetDesiredPosBehindTarget();
				UpdateSteeringAndMove(desiredPos);	
			}
		}
	}
}
