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

		public void Init(List<CardModel> cardModels, int handSlotsCount = 3, int tableSlotsCount = 5)
		{
			Debug.Log(string.Format("[MainController][Init] Card models = {0}.", cardModels.Count));

			_cardModels = cardModels;
			_cardDeck = new Queue<CardModel>(_cardModels);
			_handSlots = new List<CardModel>(handSlotsCount);
			_tableSlots = new List<CardModel>(tableSlotsCount);
		}

		public void ReturnCards()
		{
			Debug.Log("[MainController][ReturnCards]");

			var cardModels = new List<CardModel>();

			cardModels.AddRange(_handSlots.Where(cardModel => cardModel != null).ToList());
			cardModels.AddRange(_tableSlots.Where(cardModel => cardModel != null).ToList());
			_handSlots.Clear();
			_tableSlots.Clear();

			cardModels.ForEach(_cardDeck.Enqueue);

			//_tableSlots.Clear();

			if (ReturnCardsCallback != null)
				ReturnCardsCallback();
		}

		public CardModel GetCard()
		{
			Debug.Log(string.Format("[MainCotroller][GetCard] Cards = {0}", _cardDeck.Count));

			CardModel cardModel = null;
			if (_handSlots.Count < _handSlots.Capacity && _cardDeck.Any())
			{
				cardModel = _cardDeck.Dequeue();
				_handSlots.Add(cardModel);

				if (GetCardModelCallback != null)
					GetCardModelCallback(cardModel);
			}

			return cardModel;
		}

		private void DoDamage(CardModel cardModel, int damage)
		{
			Debug.Log(string.Format("[MainController][DoDamage] Card name: {0}; damage: {1}.", cardModel.Name, damage));

			var firstOrDefault = _tableSlots.FirstOrDefault(cm => cm == cardModel);
			if (firstOrDefault != null)
				firstOrDefault.DoDamage(damage);
		}

		public void DoDamageAll(int damage)
		{
			Debug.Log(string.Format("[MainController][DoDamageAll] Damage = {0}", damage));

			_tableSlots.ForEach(cardModel => DoDamage(cardModel, damage));

			if (DoDamageCallback != null)
				DoDamageCallback(damage);
		}

		public bool MoveCard(CardModel cardModel, int handSlotIndex, int tableSlotIndex)
		{
			Debug.Log(string.Format("[MainController][MoveCard]Hand slot index = {0}, table slot index = {1}",
				handSlotIndex, tableSlotIndex));

			if (handSlotIndex > _handSlots.Count) return false;

			if (tableSlotIndex > _tableSlots.Count)
				if (tableSlotIndex <= _tableSlots.Capacity)
					_tableSlots.Add(cardModel);
				else return false;
			//else _tableSlots[tableSlotIndex] = cardModel;

			_handSlots.RemoveAt(handSlotIndex);
				 //_tableSlots[tableSlotIndex] != null) return false;

			//; [tableSlotIndex] = _handSlots[handSlotIndex];


			return true;
		}
	}
}