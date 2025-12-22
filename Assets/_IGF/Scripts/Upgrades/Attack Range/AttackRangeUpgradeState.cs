using System;
using UnityEngine;

namespace IGF.Upgrades
{
	[Serializable]
	public sealed class AttackRangeUpgradeState
	{
		[SerializeField] private AttackRangeProgression _progression;
		[SerializeField, Min(0)] private int _currentLevel;

		public float GetCurrentRange()
		{
			var list = _progression.RangeByLevel;
			var index = Mathf.Clamp(_currentLevel, 0, list.Count - 1);
			return list[index];
		}

		public void Upgrade() => _currentLevel = Mathf.Min(_currentLevel + 1, _progression.RangeByLevel.Count - 1);

		public void Reset() => _currentLevel = 0;
	}
}