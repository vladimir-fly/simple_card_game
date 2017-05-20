using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCG
{
	public class MainController
	{
		private List<CardModel> _cardModels;
		private Queue<CardModel> _cardDeck;
		private List<CardModel> _handSlots;
		private List<CardModel> _tableSlots;

		public Action ReturnCardsCallback;
		public Action<CardModel> GetCardModelCallback;
		public Action<int> DoDamageCallback;

		private static MainController _instance;
		public static MainController Instance { get { return _instance ?? (_instance = new MainController()); } }
		private MainController() { }

		public void Init(List<CardModel> cardModels, int handSlotsCount = 3, int tableSlotsCount = 5)
		{
			_cardModels = cardModels;
			_cardDeck = new Queue<CardModel>(_cardModels);
			_handSlots = new List<CardModel>(handSlotsCount);
			_tableSlots = new List<CardModel>(tableSlotsCount);
		}

		public CardModel GetCard()
		{
			CardModel cardModel = null;

			Debug.Log("[MainController][GetCard]");

			if (_handSlots.Count < _handSlots.Capacity && _cardDeck.Any())
			{
				cardModel = _cardDeck.Dequeue();
				_handSlots.Add(cardModel);

				if (GetCardModelCallback != null)
					GetCardModelCallback(cardModel);
			}

			return cardModel;
		}

		public void ReturnCards()
		{
			var cardModels = _handSlots.Where(cardModel => cardModel != null).ToList();
			_handSlots.Clear();
			_cardModels.AddRange(cardModels);

			if (ReturnCardsCallback != null)
				ReturnCardsCallback();
		}

		private void DoDamage(CardModel cardModel, int damage)
		{
			var firstOrDefault = _tableSlots.FirstOrDefault(cm => cm == cardModel);
			if (firstOrDefault != null)
				firstOrDefault.DoDamage(damage);
		}

		public void DoDamageAll(int damage)
		{
			_tableSlots.ForEach(cardModel => DoDamage(cardModel, damage));

			if (DoDamageCallback != null)
				DoDamageCallback(damage);
		}
	}
}