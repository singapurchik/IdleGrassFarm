using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace IGF
{
	public class ChainConstraintWrapper : WeightChanger
	{
		private ChainIKConstraint _rig;
		private Transform _target;
		
		public void SetData(ChainConstraintData data)
		{
			_target = data.Target;
			_rig = data.Rig;
			base.SetData(data.DefaultChangeWeightSpeed, data.MaxWeight);
		}

		public void ForceUpdateWeight()
		{
			_rig.weight = Weight;
		}

		public void Update(Transform leftHandTarget = null)
		{
			if (IsWeightChanged(out var weight))
				_rig.weight = weight;
			
			if (weight > 0 && leftHandTarget != null)
				_target.SetPositionAndRotation(leftHandTarget.position, leftHandTarget.rotation);
		}
	}
}