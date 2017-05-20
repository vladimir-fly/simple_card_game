using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCG
{
	public class App : MonoBehaviour
	{
		[SerializeField] private MainView _mainView;
		[SerializeField] private CardView CardViewTemplate;

		//private MainController _mainController;

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
			var cardModels = GetCardModels();
			var cardViews = GenerateCardViews(cardModels);

			//_mainController = .Instance.Init(cardModels);
			MainController.Instance.Init(cardModels);
			InitControllerCallbacks();

			_mainView.Init(cardViews);
			InitViewCallbacks();

			BindCallbacks();
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
			if (_mainView == null) Debug.LogError("[App][CheckUnitBindings] MainView not assigned!");
			return _mainView != null;
		}

		public List<CardView> GenerateCardViews(List<CardModel> cardModels)
		{
			var cardViews = new List<CardView>();

			var i = 0;
			foreach (var model in cardModels)
			{
				var cardView = Instantiate(CardViewTemplate);
				//cardView.transform.position = new Vector3(0, 0, i++);
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