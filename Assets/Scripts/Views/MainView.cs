using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SCG
{
	public class MainView : MonoBehaviour
	{
		[SerializeField] private MainUI _mainUi;
		[SerializeField] private CardDeckView _cardDeckView;
		[SerializeField] private List<HandSlotView> _handSlotViews;
		[SerializeField] private List<TableSlotView> _tableSlotViews;

		public Func<CardModel, int, int, bool> MoveCardViewCallback;

		private List<CardView> _cardViews;

		public void Init(List<CardView> cardViews, int handSlotViewCount = 3, int tableSlotViewsCount = 5)
		{
			if (!CheckUnityBindings()) return;

			_cardViews = cardViews;
			_cardDeckView = new GameObject("CardDeckView").AddComponent<CardDeckView>();
			_cardDeckView.Init(_cardViews);
			_handSlotViews = _handSlotViews ?? new List<HandSlotView>(handSlotViewCount);
			_tableSlotViews = _tableSlotViews ?? new List<TableSlotView>(tableSlotViewsCount);
		}

		public void InitUI(Action returnCard, Func<CardModel> getCard, Action<int> doDamage)
		{
			_mainUi.Init(
				() => returnCard(),
				() => getCard(),
				() => doDamage(1));
		}

		public void ReturnCardViews()
		{
			Debug.Log(string.Format("[MainView][ReturnCardViews] Starts. CardViews = {0}", _cardDeckView.Count));

			var handSlots = _handSlotViews.Where(hs => hs.CardView != null).ToList();
			handSlots.ForEach(handSlot =>
			{
				var cv = handSlot.CardView;
				if (cv != null) _cardDeckView.Enqueue(cv);
				handSlot.SetCardView(null);
			});

			var tableSlots = _tableSlotViews.Where(ts => ts.CardView != null).ToList();
			tableSlots.ForEach(tableSlot =>
			{
				var cv = tableSlot.CardView;
				if (cv != null) _cardDeckView.Enqueue(cv);
				tableSlot.SetCardView(null);
			});

			Debug.Log(string.Format("[MainView][ReturnCardViews] Ends. CardViews = {0}", _cardDeckView.Count));
		}

		public void GetCardView(CardModel cardModel)
		{
			Debug.Log(string.Format("[MainView][GetCardView] Starts. CardViews = {0}", _cardDeckView.Count));

			var emptyHandSlots = _handSlotViews.Where(hs => hs.CardView == null);
			if (emptyHandSlots.Any() && _cardDeckView.Any())
			{
				var cardView = _cardDeckView.Dequeue();

				var freeHandSlot = _handSlotViews.FirstOrDefault(hs => hs.CardView == null);
				if (freeHandSlot == null) return;

				freeHandSlot.SetCardView(cardView);
			}

			Debug.Log(string.Format("[MainView][GetCardView] Ends. CardViews = {0}", _cardDeckView.Count));
		}

		public void DoDamageAll(int damage)
		{
			Debug.Log(string.Format("[MainView][DoDamageAll] Damage = {0}", damage));

			_tableSlotViews.ForEach(slot =>
			{
				if (slot.CardView != null)
					slot.CardView.GetDamage(damage);
			});
		}

		public void MoveCardView(CardView target, HandSlotView handSlot, TableSlotView tableSlot)
		{
			if (MoveCardViewCallback == null) return;

			var handSlotIndex = _handSlotViews.IndexOf(handSlot);
			var tableSlotIndex = _tableSlotViews.IndexOf(tableSlot);

			if (!MoveCardViewCallback(target.CardModel, handSlotIndex, tableSlotIndex)) return;
			tableSlot.SetCardView(target);
			handSlot.SetCardView(null);
		}

		private bool CheckUnityBindings()
		{
			if (_mainUi == null) Debug.LogError("[MainView][CheckUnityBindings] MainUI not assigned!");
			return _mainUi != null;
		}
	}
}