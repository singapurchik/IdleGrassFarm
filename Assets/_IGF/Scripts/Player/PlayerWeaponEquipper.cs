using IGF.Players.Animations;
using UnityEngine;
using Zenject;
using System;

namespace IGF.Players
{
	public class PlayerWeaponEquipper : MonoBehaviour
	{
		[Serializable]
		private struct WeaponPoints
		{
			[SerializeField] private Transform _rightWeaponPoint;
			[SerializeField] private Transform _leftWeaponPoint;
			
			public Transform RightWeaponPoint => _rightWeaponPoint;
			public Transform LeftWeaponPoint => _leftWeaponPoint;
		}

		[SerializeField] private WeaponPoints _spinePoints;
		[SerializeField] private WeaponPoints _handsPoints;
		[Space(5)]
		[SerializeField] private Transform _rightWeapon;
		[SerializeField] private Transform _leftWeapon;
		
		[Inject] private PlayerAnimEventsReceiver _animEventsReceiver;

		private void OnEnable()
		{
			_animEventsReceiver.OnUnequipWeapon += Unequip;
			_animEventsReceiver.OnEquipWeapon += Equip;
		}
		
		private void OnDisable()
		{
			_animEventsReceiver.OnUnequipWeapon -= Unequip;
			_animEventsReceiver.OnEquipWeapon -= Equip;
		}

		private void Start()
		{
			Unequip();
		}

		private void Unequip()
		{
			_rightWeapon.SetParent(_spinePoints.RightWeaponPoint, false);
			_leftWeapon.SetParent(_spinePoints.LeftWeaponPoint, false);
		}

		private void Equip()
		{
			_rightWeapon.SetParent(_handsPoints.RightWeaponPoint, false);
			_leftWeapon.SetParent(_handsPoints.LeftWeaponPoint, false);
		}
	}
}