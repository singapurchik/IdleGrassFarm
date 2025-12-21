using IGF.Buyers.Animations;
using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public class BuyerMover : MonoBehaviour
	{
		[SerializeField] private float _targetRepathThreshold = 0.25f;
		[SerializeField] private float _selfRepathThreshold = 0.25f;

		[Inject] protected BuyerAnimator Animator;
		[Inject] protected NavMeshAgent Agent;
		
		private NavMeshPath _navMeshPath;

		private Vector3 _lastSetDestination = new (float.PositiveInfinity, 0, 0);
		private Vector3 _transformMoveTargetPosition;
		private Vector3 _teleportTargetPosition;
		private Vector3 _navMeshTargetPosition;
		
		private float _transformMoveSpeed;
		private float _nextPathUpdateTime;
		
		private bool _isAgentPositionUpdatingInRootMotionRequested;
		private bool _isDisableNavMeshAgentRequested;
		private bool _isRootMotionMovedLastFrame;
		private bool _isTransformMoveThisFrame;
		private bool _isTransformMoveRequested;
		private bool _isRootMotionRequested;
		private bool _isTeleportRequested;
		private bool _isDisableRequested;
		private bool _isMovedLastFrame;
		
		private const float MIN_REMAINING_DISTANCE = 0.1f;
		private const float PATH_UPDATE_INTERVAL = 0.2f;

		public bool IsProcessMovement => Agent.enabled && (_isTransformMoveThisFrame || _isRootMotionMovedLastFrame
		                                 || Agent.pathPending
		                                 || Agent.remainingDistance > MIN_REMAINING_DISTANCE + Agent.stoppingDistance);
		public bool IsFinishMoveThisFrame => !IsProcessMovement && IsMovedLastFrame;
		public bool IsMovingToTarget { get; private set; }
		public bool IsMovedLastFrame { get; private set; }
		
		public float StoppingSqrMagnitude => Agent.stoppingDistance * Agent.stoppingDistance;
		
		private void Awake()
		{
			_navMeshPath = new NavMeshPath();
			Agent.autoRepath = false;
		}

		public void RequestDisableNavMeshAgent() => _isDisableNavMeshAgentRequested = true;
		
		public void RequestDisable() => _isDisableRequested = true;
		
		public void SetStoppingDistance(float stoppingDistance) => Agent.stoppingDistance = stoppingDistance;

		public void RequestEnableRootMotion(bool isAgentPositionUpdatingInRootMotionRequested = true)
		{
			_isAgentPositionUpdatingInRootMotionRequested = isAgentPositionUpdatingInRootMotionRequested;
			_isRootMotionRequested = true;
		}

		private bool NeedRepath(Vector3 target)
		{
			if (Agent.pathPending) return false;
			if (!Agent.hasPath || Agent.pathStatus != NavMeshPathStatus.PathComplete) return true;
			if ((_lastSetDestination - target).sqrMagnitude >= _targetRepathThreshold * _targetRepathThreshold) return true;
			return false;
		}

		private bool CalculatePath(Vector3 target)
		{
			IsMovingToTarget = false;

			if (_navMeshTargetPosition != target)
			{
				_navMeshTargetPosition = target;

				if (!Agent.CalculatePath(target, _navMeshPath))
					return IsMovingToTarget;
			}

			IsMovingToTarget = _navMeshPath.status == NavMeshPathStatus.PathComplete && _navMeshPath.corners.Length > 0;
			return IsMovingToTarget;
		}

		public void NavMeshMove(Vector3 target)
		{
			Agent.enabled = true;
			Agent.updatePosition = true;
			_isDisableRequested = false;

			if (Time.timeSinceLevelLoad > _nextPathUpdateTime)
			{
				_nextPathUpdateTime = Time.timeSinceLevelLoad + PATH_UPDATE_INTERVAL;

				if (NeedRepath(target))
				{
					Agent.isStopped = false;

					if (CalculatePath(target))
					{
						Agent.SetDestination(_navMeshTargetPosition);
						_lastSetDestination = _navMeshTargetPosition;
					}
					else
					{
						if (_navMeshPath.corners != null && _navMeshPath.corners.Length > 0)
						{
							var fallback = _navMeshPath.corners[^1];
							Agent.SetDestination(fallback);
							_lastSetDestination = fallback;
						}
						else
						{
							Agent.ResetPath();
							_lastSetDestination = new Vector3(float.PositiveInfinity, 0f, 0f);
						}
					}
				}
			}
		}
		
		public void TryStopMove()
		{
			if (Agent.enabled && !_isDisableRequested)
			{
				Agent.isStopped = true;
				Agent.ResetPath();
			}
		}

		public void RequestTeleport(Vector3 position)
		{
			_isTeleportRequested = true;
			_teleportTargetPosition = position;
		}

		public void RequestTransformMove(Vector3 target, float speed)
		{
			_isTransformMoveRequested = true;
			_transformMoveTargetPosition = target;
			_transformMoveSpeed = speed;
			
			_isTransformMoveThisFrame =
				Vector3.SqrMagnitude(transform.position - _transformMoveTargetPosition) > 0.1f;
		}
		
		private void Teleport()
		{
			Agent.Warp(_teleportTargetPosition);
		}

		private void TransformMove()
		{
			var newPos = Vector3.MoveTowards(
				transform.position, _transformMoveTargetPosition, 
				_transformMoveSpeed * Time.deltaTime);
			
			transform.position = newPos;
			Agent.updatePosition = false;
			Agent.nextPosition = newPos;
		}
		
		private void RootMotionMove()
		{
			Agent.speed = 0;
			var newPos = transform.position + Animator.DeltaPosition;
			transform.position = newPos;
			transform.rotation *= Animator.DeltaRotation;
			Agent.updatePosition = _isAgentPositionUpdatingInRootMotionRequested;
			Agent.nextPosition = newPos;
			_isRootMotionMovedLastFrame = Animator.DeltaPosition != Vector3.zero;
		}
		
		protected virtual void Update()
		{
			if (_isDisableRequested)
			{
				Agent.enabled = false;
				_isDisableRequested = false;
			}
			else
			{
				if (_isDisableNavMeshAgentRequested)
				{
					if (Agent.enabled)
						Agent.enabled = false;
				}
				else if (Agent.enabled == false)
				{
					Agent.enabled = true;
				}

				if (_isTeleportRequested)
					Teleport();
				else if (_isTransformMoveRequested)
					TransformMove();
				else if (Agent.enabled)
					Agent.updatePosition = true;
			}

			_isDisableNavMeshAgentRequested = false;
			_isTransformMoveRequested = false;
			_isTeleportRequested = false;
		}
		
		private void OnAnimatorMove()
		{
			_isRootMotionMovedLastFrame = false;
			
			if (_isRootMotionRequested)
				RootMotionMove();
			else if (Agent.enabled)
				Agent.speed = Animator.Velocity.magnitude;	
			
			_isAgentPositionUpdatingInRootMotionRequested = false;
			_isRootMotionRequested = false;
		}
		
		private void LateUpdate()
		{
			_isTransformMoveThisFrame = false;
			_isMovedLastFrame = IsMovedLastFrame;
			IsMovedLastFrame = _isMovedLastFrame || IsProcessMovement;
		}
	}
}