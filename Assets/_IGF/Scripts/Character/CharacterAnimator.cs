using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF
{
	public abstract class CharacterAnimator : MonoBehaviour
	{
		[InjectOptional] private List<ILayerWithTriggers> _layersWithTriggers;
		[Inject] protected Animator Animator;
		[Inject] private List<Layer> _layers;

		private readonly Dictionary<int, Layer> _layersByIndex = new(10);
		private readonly HashSet<Layer> _layersIgnoringPause = new();

		protected virtual void Awake()
		{
			foreach (var layer in _layers)
			{
				_layersByIndex.Add(layer.Index, layer);
				layer.OnPlayIgnoringPauseAnim += AddLayerIgnoringPause;
			}
		}

		protected virtual void OnDestroy()
		{
			foreach (var layer in _layers)
				layer.OnPlayIgnoringPauseAnim -= AddLayerIgnoringPause;
		}

		private void AddLayerIgnoringPause(int layerIndex) => _layersIgnoringPause.Add(_layersByIndex[layerIndex]);

		public void TryResetTriggers()
		{
			if (_layersWithTriggers != null)
				foreach (var layer in _layersWithTriggers)
					layer.ResetTriggers();
		}
	}
}