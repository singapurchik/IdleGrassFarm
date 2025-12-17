using UnityEngine;
using System;

namespace IGF.Players.Animations
{
	public class PlayerAnimEventsReceiver : MonoBehaviour
	{
		public event Action OnUnequipWeapon;
		public event Action OnEquipWeapon;
		public event Action OnAttack;
		
		private void AE_UnequipWeapon() => OnUnequipWeapon?.Invoke();
		
		private void AE_EquipWeapon() => OnEquipWeapon?.Invoke();
		
		private void AE_Attack() => OnAttack?.Invoke();
	}
}