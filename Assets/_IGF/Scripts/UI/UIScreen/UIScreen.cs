using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine;
using VInspector;
using System;

namespace IGF.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class UIScreen : MonoBehaviour, IUIScreen
	{
		[Serializable]
		private class PunchedObject
		{
			[SerializeField] private RectTransform _rectTransform;
			[SerializeField] private Vector3 _punchScaleFactor = new (0.05f, 0.05f, 0.05f);
			[SerializeField] private float _duration = 0.5f;
			[SerializeField] private int _vibrato;
			[SerializeField] private int _elasticity;
			[SerializeField] private Ease _ease = Ease.OutBack;
			[SerializeField] private float _delay;
			
			private Vector3 _defaultScale;
			private Tween _currentTween;
			
			public float Duration => _duration;
			public float Delay => _delay;

			public void Initialize()
			{
				_defaultScale = _rectTransform.localScale;
				_rectTransform.localScale = _defaultScale;
			}

			public void ReadyToPlay()
			{
				_rectTransform.gameObject.SetActive(false);
			}
			
			public void Play()
			{
				Reset();
				
				_currentTween = _rectTransform
					.DOPunchScale(_punchScaleFactor, _duration, _vibrato, _elasticity)
					.SetEase(_ease)
					.SetDelay(_delay)
					.OnStart(() =>
					{
						_rectTransform.gameObject.SetActive(true);
					});
			}

			public void Reset()
			{
				_currentTween?.Kill();
				_rectTransform.localScale = _defaultScale;
			}
		}

		[SerializeField] private bool _isFullyDisabled = true;
		[ShowIf(nameof(_isFullyDisabled))]
		[SerializeField] private List<GameObject> _children = new();
		[EndIf]
		[SerializeField] private bool _isAnimated;
		[SerializeField] private bool _isHasFadeEffect;
		[ShowIf(nameof(_isHasFadeEffect))]
		[SerializeField] private float _showFadeDuration = 0.2f;
		[SerializeField] private float _hideFadeDuration;
		[EndIf]
		[ShowIf(nameof(_isAnimated))]
		[SerializeField] private List<PunchedObject> _animatedOjbectsOnShow;
		[EndIf]

		protected CanvasGroup CanvasGroup;

		private Coroutine _currentShowRoutine;

		private bool _isEventsShowInEditor;

		public float ShowFadeDuration => _showFadeDuration;
		public float HideFadeDuration => _hideFadeDuration;
		
		public virtual bool IsHasCloseButton { get; } = false;
		public bool IsProcessShow { get; private set; }
		public bool IsProcessHide { get; private set; }
		public bool IsVisible { get; private set; }
		
		public event Action OnCloseButtonClicked;

		[ShowIf(nameof(_isEventsShowInEditor))]
		[SerializeField] private UnityEvent _onStartShow;
		[SerializeField] private UnityEvent _onStartHide;
		[SerializeField] private UnityEvent _onHidden;
		[SerializeField] private UnityEvent _onShown;
		[EndIf]

		protected virtual void Awake()
		{
			CanvasGroup = GetComponent<CanvasGroup>();
			IsVisible = CanvasGroup.alpha != 0;

			foreach (var animatedObject in _animatedOjbectsOnShow)
				animatedObject.Initialize();
		}

		public virtual void Hide()
		{
			if (IsVisible && !IsProcessHide)
				StartHide();
		}

		protected virtual void StartHide()
		{
			IsProcessHide = true;
			IsProcessShow = false;

			_onStartHide?.Invoke();

			if (_isHasFadeEffect)
			{
				CanvasGroup.DOFade(0, _hideFadeDuration)
					.OnComplete(HideComplete);
			}
			else
			{
				if (_isAnimated)
				{
					if (_currentShowRoutine != null)
						StopCoroutine(_currentShowRoutine);

					foreach (var animatedObject in _animatedOjbectsOnShow)
						animatedObject.Reset();
				}

				CanvasGroup.alpha = 0;
				HideComplete();
			}
		}

		public virtual void Show()
		{
			if (!IsVisible && !IsProcessShow)
				StartShow();
		}

		protected virtual void StartShow()
		{
			IsProcessShow = true;
			IsProcessHide = false;
			
			_onStartShow?.Invoke();

			if (_isFullyDisabled && _children.Count > 0)
				foreach (var child in _children)
					child.gameObject.SetActive(true);
			
			if (_isAnimated && _animatedOjbectsOnShow.Count > 0)
				foreach (var animatedObject in _animatedOjbectsOnShow)
					animatedObject.ReadyToPlay();

			if (_isHasFadeEffect)
			{
				CanvasGroup.DOFade(1, _showFadeDuration)
					.OnComplete(OnCanvasAlphaEnabled);
			}
			else
			{
				CanvasGroup.alpha = 1;
				OnCanvasAlphaEnabled();
			}
		}

		private void OnCanvasAlphaEnabled()
		{
			if (_isAnimated)
				_currentShowRoutine = StartCoroutine(ShowRoutine());
			else
				ShowComplete();
		}

		private IEnumerator ShowRoutine()
		{
			float showDuration = 0;

			foreach (var animatedObject in _animatedOjbectsOnShow)
			{
				var animDuration = animatedObject.Delay + animatedObject.Duration;
				
				if (animDuration > showDuration)
					showDuration = animDuration;
				
				animatedObject.Play();
			}

			yield return new WaitForSeconds(showDuration);
			ShowComplete();
			_currentShowRoutine = null;
		}

		protected virtual void ShowComplete()
		{
			IsProcessShow = false;
			IsVisible = true;
			_onShown?.Invoke();
		}

		protected virtual void HideComplete()
		{
			IsProcessHide = false;
			IsVisible = false;

			if (_isFullyDisabled && _children.Count > 0)
				foreach (var child in _children)
					child.gameObject.SetActive(false);

			_onHidden?.Invoke();
		}
		
		public void RemoveOnStartShowListener(UnityAction listener) => _onStartShow?.RemoveListener(listener);
		public void AddOnStartShowListener(UnityAction listener) => _onStartShow?.AddListener(listener);

		public void RemoveOnStartHideListener(UnityAction listener) => _onStartHide?.RemoveListener(listener);
		public void AddOnStartHideListener(UnityAction listener) => _onStartHide?.AddListener(listener);

		public void RemoveOnShownListener(UnityAction listener) => _onShown?.RemoveListener(listener);
		public void AddOnShownListener(UnityAction listener) => _onShown?.AddListener(listener);

		public void RemoveOnHiddenListener(UnityAction listener) => _onHidden?.RemoveListener(listener);
		public void AddOnHiddenListener(UnityAction listener) => _onHidden?.AddListener(listener);

		void IUIScreenForceVisibility.ForceHide() => OnForceHide();
		
		void IUIScreenForceVisibility.ForceShow() => OnForceShow();

		protected void InvokeOnCloseButtonClicked() => OnCloseButtonClicked?.Invoke();
		
		protected virtual void OnForceHide()
		{
			CanvasGroup.alpha = 0;
			IsVisible = false;
		}

		protected virtual void OnForceShow()
		{
			CanvasGroup.alpha = 1;
			IsVisible = true;
		}

#if UNITY_EDITOR
		[ShowIf(nameof(_isFullyDisabled))]
		[Button]
		private void FindChildren()
		{
			_children.Clear();

			foreach (Transform child in transform)
				_children.Add(child.gameObject);
		}
		[EndIf]

		[Button]
		private void EventsShowInEditor() => _isEventsShowInEditor = !_isEventsShowInEditor;

		[Button]
		private void TestShow() => Show();

		[Button]
		private void TestHide() => Hide();
#endif
	}
}