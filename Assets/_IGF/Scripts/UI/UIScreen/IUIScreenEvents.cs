using System;
using UnityEngine.Events;

namespace IGF.UI
{
	public interface IUIScreenEvents
	{
		public void AddOnStartShowListener(UnityAction listener);
		public void RemoveOnStartShowListener(UnityAction listener);
		
		public void AddOnStartHideListener(UnityAction listener);
		public void RemoveOnStartHideListener(UnityAction listener);
		
		public void AddOnShownListener(UnityAction listener);
		public void RemoveOnShownListener(UnityAction listener);
		
		public void AddOnHiddenListener(UnityAction listener);
		public void RemoveOnHiddenListener(UnityAction listener);

		public event Action OnCloseButtonClicked;
	}
}