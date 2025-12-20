using System;

namespace IGF
{
	public interface IDamageable
	{
		public event Action<IDamageable> OnDamageTaken;
		
		public void TryTakeDamage();
	}
}