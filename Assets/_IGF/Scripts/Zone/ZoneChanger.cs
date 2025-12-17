using IGF.Players;
using UnityEngine;
using VInspector;

namespace IGF
{
	public class ZoneChanger : MonoBehaviour
	{
		[SerializeField] private ZoneType _type;

		private IPlayerZoneChanger _zoneChanger;
		
		private bool _isActive;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayerZoneChanger zoneChanger))
			{
				_zoneChanger = zoneChanger;
				_isActive = true;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayerZoneChanger zoneChanger))
			{
				_zoneChanger = null;
				_isActive = false;
			}
		}

		private void Update()
		{
			if (_isActive)
				_zoneChanger.RequestChangeZone(_type);
		}

#if UNITY_EDITOR
		[Header("DEBUG")]
		[SerializeField] private bool _isDrawGizmos = true;
		[ReadOnly][SerializeField] private BoxCollider _collider;
		
		private void OnDrawGizmos()
		{
			if (_isDrawGizmos)
			{
				if (_collider == null)
					_collider = GetComponent<BoxCollider>();
				
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(_collider.center, _collider.size);
			}
		}
#endif
	}
}