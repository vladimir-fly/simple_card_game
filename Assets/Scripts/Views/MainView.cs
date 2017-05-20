using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCG
{
	public class MainView : MonoBehaviour
	{
		[SerializeField] private MainUI _mainUi;
		[SerializeField] private Queue<CardView> _cardDeckView;
		[SerializeField] private List<HandSlotView> _handSlotViews;
		[SerializeField] private List<TableSlotView> _tableSlotViews;

		public GameObject _cardDeckAchor;
		private List<CardView> _cardViews;

		public void Init(List<CardView> cardViews, int handSlotViewCount = 3, int tableSlotViewsCount = 5)
		{
			_cardViews = cardViews;
			_cardDeckView = new Queue<CardView>(_cardViews);
			_handSlotViews = _handSlotViews ?? new List<HandSlotView>(handSlotViewCount);
			_tableSlotViews = _tableSlotViews ?? new List<TableSlotView>(tableSlotViewsCount);

			InitCardDeckAnchor();
		}

		private void InitCardDeckAnchor()
		{
			_cardDeckAchor = new GameObject("CardDeckAnchor");
			_cardDeckAchor.transform.SetParent(transform);
			_cardDeckAchor.transform.localPosition = new Vector3(3f, 0.3f, -3f);

			var i = 0f;
			_cardViews.ForEach(cv =>
			{
				cv.gameObject.transform.SetParent(_cardDeckAchor.transform);
				cv.gameObject.transform.localPosition = new Vector3(i, 0.5f, 0f);
				cv.gameObject.SetActive(true);
				i += 0.15f;
			});
		}

		public void InitUI(Action returnCard, Func<CardModel> getCard, Action<int> doDamage)
		{
			_mainUi.Init(
				() => returnCard(),
				() =>
				{
					Debug.Log("get card");
					getCard();
				},
				() => doDamage(1));
		}

		public void ReturnCardViews()
		{
			Debug.Log("[MainView][ReturnCardViews]");
			var handSlots =
				_handSlotViews.Where(hs => hs.CardView != null).ToList();

			handSlots.ForEach(hs =>
			{
				var cv = hs.CardView;
				if (cv != null)
				{
					cv.transform.SetParent(_cardDeckAchor.transform);
					cv.transform.localPosition = new Vector3(0.5f, 0.5f, 0f);
					_cardDeckView.Enqueue(cv);
				}
				hs.CardView = null;
			});
		}

		public void GetCardView(CardModel cardModel)
		{
			Debug.Log("[MainView][GetCardView]");

			CardView cardView;

			var emptyHandSlots = _handSlotViews.Where(hs => hs.CardView == null);
			if (emptyHandSlots.Any() && _cardDeckView.Any())
			{
				if (_cardDeckView.Peek().CardModel != cardModel)
					Debug.LogError("[MainView][GetCardView] Not equal card models!");

				cardView = _cardDeckView.Dequeue();

				var freeHandSlot = _handSlotViews.FirstOrDefault(hs => hs.CardView == null);
				if (freeHandSlot != null)
				{
					freeHandSlot.CardView = cardView;
					cardView.gameObject.transform.SetParent(freeHandSlot.transform);
					cardView.gameObject.transform.localPosition = new Vector3(0, 1, 0);
					cardView.gameObject.transform.Rotate(Vector3.up *(- 90), Space.Self);
				}


			}
		}

		public void DoDamageAll(int damage)
		{
			_tableSlotViews.ForEach(slot => slot.CardView.CardModel.DoDamage(damage));
		}
	}
}