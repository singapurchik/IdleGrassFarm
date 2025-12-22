using UnityEngine;
using IGF.Wallet;
using Zenject;

namespace IGF
{
	public abstract class TwoCurrencyDepositUpgradeZone : ProgressTriggerZone
	{
		[Header("Currencies")]
		[SerializeField] private CurrencyType _firstCurrency = CurrencyType.Green;
		[SerializeField] private CurrencyType _secondCurrency = CurrencyType.Yellow;

		[Header("Base Cost (Level 0)")]
		[SerializeField, Min(0)] private int _firstBaseCost = 10;
		[SerializeField, Min(0)] private int _secondBaseCost = 10;

		[Header("Cost Growth")]
		[SerializeField, Min(0f)] private float _costMultiplier = 1.25f;

		[Header("Deposit")]
		[SerializeField, Min(0f)] private float _depositUnitsPerSecond = 1f;
		[SerializeField, Min(1)] private int _depositChunk = 1;

		[Header("View")]
		[SerializeField] private TwoCurrencyDepositZoneView _view;

		[Inject] private IWalletCurrencyRemover _wallet;

		private bool _isDepositPhase;
		
		private float _depositAccumulator;

		private int _secondPaid;
		private int _secondCost;
		private int _firstCost;
		private int _firstPaid;
		private int _level;


		private void Awake()
		{
			RecalculateCosts();
			UpdateView();
		}

		protected override void OnPlayerInside()
		{
			if (_isDepositPhase)
			{
				TryDepositTick();

				if (IsDepositCompleted())
					CompleteLevel();
			}
			else
			{
				base.OnPlayerInside();

				if (Progress >= PROGRESS_TO_COMPLETE)
					StartDepositPhase();	
			}
		}

		protected override void OnProgressComplete()
		{
			if (!_isDepositPhase)
				StartDepositPhase();
		}

		protected override void OnPlayerExit()
		{
			_isDepositPhase = false;
			_depositAccumulator = 0f;
		}

		protected abstract void ApplyUpgrade();

		private void StartDepositPhase()
		{
			ClampProgressToComplete();
			_isDepositPhase = true;
			_depositAccumulator = 0f;
		}

		private void TryDepositTick()
		{
			if (IsDepositTickReady())
			{
				var firstDeposited  = TryDepositCurrency(_firstCurrency,  ref _firstPaid,  _firstCost);
				var secondDeposited = TryDepositCurrency(_secondCurrency, ref _secondPaid, _secondCost);

				if (firstDeposited || secondDeposited)
					UpdateView();	
			}
		}

		private bool IsDepositTickReady()
		{
			_depositAccumulator += Time.deltaTime * _depositUnitsPerSecond;

			if (_depositAccumulator >= 1)
			{
				_depositAccumulator -= 1f;
				return true;
			}
			
			return false;
		}

		private bool TryDepositCurrency(CurrencyType currencyType, ref int paid, int cost)
		{
			if (paid >= cost)
				return false;

			var remaining = cost - paid;
			var chunk = Mathf.Min(_depositChunk, remaining);

			if (!_wallet.TryRemoveCurrency(currencyType, chunk))
				return false;

			paid += chunk;
			return true;
		}

		private bool IsDepositCompleted() => _firstPaid >= _firstCost && _secondPaid >= _secondCost;

		private void CompleteLevel()
		{
			ApplyUpgrade();

			_level++;
			_firstPaid = 0;
			_secondPaid = 0;

			_isDepositPhase = false;
			_depositAccumulator = 0f;

			RecalculateCosts();
			UpdateView();
		}

		private void RecalculateCosts()
		{
			_firstCost = CalculateCost(_firstBaseCost, _level);
			_secondCost = CalculateCost(_secondBaseCost, _level);
		}

		private int CalculateCost(int baseCost, int level)
			=> Mathf.RoundToInt(baseCost * Mathf.Pow(_costMultiplier, level));

		private void UpdateView()
		{
			var firstRemaining = Mathf.Max(0, _firstCost - _firstPaid);
			var secondRemaining = Mathf.Max(0, _secondCost - _secondPaid);
			_view.UpdateView(secondRemaining.ToString(), firstRemaining.ToString());
		}
	}
}
