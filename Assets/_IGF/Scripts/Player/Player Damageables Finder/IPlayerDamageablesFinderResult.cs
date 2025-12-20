using System.Collections.Generic;

namespace IGF.Players
{
	public interface IPlayerDamageablesFinderResult
	{
		public IEnumerable<IDamageable> Damageables { get; }
		
		public bool IsHasTargets { get; }
	}
}