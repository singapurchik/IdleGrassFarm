using UnityEngine;
using VInspector;
using Zenject;

namespace IGF
{
	public class SceneInstaller : MonoInstaller
	{
		[SerializeField] private Camera _mainCamera;

		public override void InstallBindings()
		{
			Container.BindInstance(_mainCamera).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_mainCamera = FindFirstObjectByType<Camera>(FindObjectsInactive.Include);
		}
#endif
	}
}