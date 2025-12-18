using System;
using UnityEngine;

namespace IGF
{
	public sealed class Cart : MonoBehaviour
	{
		[SerializeField] private Transform _targetPoint;

		[Header("Follow Offset (Behind Target)")]
		[SerializeField, Min(0f)] private float _maxDistance = 1.25f;
		[SerializeField, Min(0f)] private float _stopDistance = 0.15f;

		[Header("Car-like Movement")]
		[SerializeField, Min(0f)] private float _maxForwardSpeed = 6f;
		[SerializeField, Min(0f)] private float _acceleration = 18f;
		[SerializeField, Min(0f)] private float _brake = 28f;

		[Header("Steering")]
		[SerializeField, Min(0f)] private float _rotationSpeedDeg = 360f;

		// Если ошибка угла больше этого — стоим и доворачиваемся
		[SerializeField, Range(0f, 180f)] private float _stopMoveAngleDeg = 65f;
		// Начинаем плавно замедляться при приближении к этому углу
		[SerializeField, Range(0f, 180f)] private float _slowMoveAngleDeg = 25f;

		public event Action<Cart> OnDestroyed;

		private float _currentSpeed;

		private void Update()
		{
			if (!_targetPoint)
				return;

			var dt = Time.deltaTime;

			var desiredPos = GetDesiredPosBehindTarget();
			UpdateSteeringAndMove(desiredPos, dt);
		}

		private Vector3 GetDesiredPosBehindTarget()
		{
			var targetPos = _targetPoint.position;

			var behindDir = -_targetPoint.forward;
			behindDir.y = 0f;
			if (behindDir.sqrMagnitude < 0.0001f)
				behindDir = -transform.forward;

			behindDir.Normalize();

			return targetPos + behindDir * _maxDistance;
		}

		private void UpdateSteeringAndMove(Vector3 desiredPos, float dt)
		{
			// направление, куда хотим ехать (к desiredPos)
			var toDesired = desiredPos - transform.position;
			toDesired.y = 0f;

			// если уже “достаточно на месте” — стоп
			if (toDesired.magnitude <= _stopDistance)
			{
				_currentSpeed = MoveTowards(_currentSpeed, 0f, _brake * dt);
				return;
			}

			var desiredDir = toDesired.normalized;

			// Поворачиваемся к desiredDir
			var forward = transform.forward; forward.y = 0f; forward.Normalize();

			var signedAngle = Vector3.SignedAngle(forward, desiredDir, Vector3.up);
			var absAngle = Mathf.Abs(signedAngle);

			var desiredRot = Quaternion.LookRotation(desiredDir, Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, _rotationSpeedDeg * dt);

			// Управление скоростью: чем больше ошибка угла — тем медленнее
			var targetSpeed = _maxForwardSpeed;

			if (absAngle >= _stopMoveAngleDeg)
			{
				targetSpeed = 0f; // стоим и доворачиваемся
			}
			else if (absAngle > _slowMoveAngleDeg)
			{
				// линейно режем скорость между slow..stop
				var t = Mathf.InverseLerp(_stopMoveAngleDeg, _slowMoveAngleDeg, absAngle);
				targetSpeed *= t; // t: 0..1
			}

			// Плавно разгон/тормоз
			if (_currentSpeed < targetSpeed)
				_currentSpeed = MoveTowards(_currentSpeed, targetSpeed, _acceleration * dt);
			else
				_currentSpeed = MoveTowards(_currentSpeed, targetSpeed, _brake * dt);

			// Движение ТОЛЬКО вперёд — никакого “страфа”
			transform.position += transform.forward * (_currentSpeed * dt);
		}

		private static float MoveTowards(float current, float target, float maxDelta)
		{
			if (current < target) return Mathf.Min(current + maxDelta, target);
			return Mathf.Max(current - maxDelta, target);
		}
	}
}
