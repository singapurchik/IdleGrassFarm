using System.Collections.Generic;
using UnityEngine;

namespace IGF.Players.Animations
{
	public class WeaponLayer : Layer
	{
		private readonly int _isAttackBoolHash = Animator.StringToHash("isAttack");
		
        protected override float MaxLayerWeight => 0.85f;
		
		public WeaponLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}

		public void StopAttackAnim() => Animator.SetBool(_isAttackBoolHash, false);
		
		public void PlayAttackAnim() => Animator.SetBool(_isAttackBoolHash, true);
	}
}