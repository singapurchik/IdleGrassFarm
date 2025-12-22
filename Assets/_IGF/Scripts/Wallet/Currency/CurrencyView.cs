using UnityEngine;
using TMPro;

namespace IGF.Wallet
{
	public class CurrencyView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _amountText;
		[SerializeField] private CurrencyType _type;
		
		public CurrencyType Type => _type;
		
		public void UpdateView(string amount) => _amountText.text = amount;
	}
}