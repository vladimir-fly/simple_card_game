using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCG
{
	public class App : MonoBehaviour
	{
		[SerializeField] private MainView _mainView;
		[SerializeField] private CardView _cardViewTemplate;

		//Callbacks for MainView
		private Action ReturnCardViewsCallback;
		private Action<CardModel> GetCardViewCallback;
		private Action<int> DoDamageCardViewCallback;

		//Callbacks for MainController
		private Action ReturnCardModelsCallback;
		private Func<CardModel> GetCardModelCallback;
		private Action<int> DoDamageCardModelCallback;

		private void Start()
		{
			if (!CheckUnityBindings()) return;
			Init();
		}

		private void Init()
		{
			Debug.Log("[App][Init]");

			var cardModels = GetCardModels();
			var cardViews = GenerateCardViews(cardModels);

			MainController.Instance.Init(cardModels);
			InitControllerCallbacks();

			_mainView.Init(cardViews);
			InitViewCallbacks();

			BindCallbacks();

			InitPlayerHand();
		}

		private void InitPlayerHand()
		{
			for (var i = 0; i < 3; i++) GetCardModelCallback();
		}

		private void InitControllerCallbacks()
		{
			ReturnCardModelsCallback = MainController.Instance.ReturnCards;
			GetCardModelCallback = MainController.Instance.GetCard;
			DoDamageCardModelCallback = MainController.Instance.DoDamageAll;
		}

		private void InitViewCallbacks()
		{
			ReturnCardViewsCallback = _mainView.ReturnCardViews;
			GetCardViewCallback = _mainView.GetCardView;
			DoDamageCardViewCallback = _mainView.DoDamageAll;
		}

		private void BindCallbacks()
		{
			MainController.Instance.ReturnCardsCallback = ReturnCardViewsCallback;
			MainController.Instance.GetCardModelCallback = GetCardViewCallback;
			MainController.Instance.DoDamageCallback = DoDamageCardViewCallback;

			_mainView.InitUI(
				ReturnCardModelsCallback,
				GetCardModelCallback,
				DoDamageCardModelCallback);
		}

		private bool CheckUnityBindings()
		{
			if (_mainView == null) Debug.LogError("[App][CheckUnityBindings] MainView not assigned!");
			if (_cardViewTemplate == null) Debug.LogError("[App][CheckUnityBindings] CardViewTemplate not assigned!");
			return _mainView != null && _cardViewTemplate != null;
		}

		public List<CardView> GenerateCardViews(List<CardModel> cardModels)
		{
			var cardViews = new List<CardView>();

			foreach (var model in cardModels)
			{
				var cardView = Instantiate(_cardViewTemplate);
				cardView.Init(model);
				cardViews.Add(cardView);
			}

			return cardViews;
		}

		private static List<CardModel> GetCardModels()
		{
			return Mocks.CardModelCollection1;
		}
	}
}