using UnityEngine.Animations.Rigging;
using UnityEngine;
using System;

namespace IGF
{
	[Serializable]
	public class ChainConstraintData
	{
		[SerializeField] private ChainIKConstraint _rig;
		[SerializeField] private Transform _target;
		[Range(0, 1)] [SerializeField] private float _maxWeight = 1f;
		[SerializeField] private float _defaultChangeWeightSpeed = 5f;

		public ChainIKConstraint Rig => _rig;
		public Transform Target => _target;
		
		public float DefaultChangeWeightSpeed => _defaultChangeWeightSpeed;
		public float MaxWeight => _maxWeight;
	}
}