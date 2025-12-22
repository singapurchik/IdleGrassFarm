using System.Collections.Generic;
using UnityEngine;

namespace IGF.Upgrades
{
	[CreateAssetMenu(fileName = "AttackRangeProgression", menuName = "IGF/Upgrades/Attack Range Progression", order = 0)]
	public sealed class AttackRangeProgression : ScriptableObject
	{
		[SerializeField] private List<float> _rangeByLevel;

		public IReadOnlyList<float> RangeByLevel => _rangeByLevel;
	}
}