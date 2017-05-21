using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SCG
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private Button ReturnCards;
        [SerializeField] private Button GetCard;
        [SerializeField] private Button DoDamage;

        public void Init(UnityAction returnCards, UnityAction getCard, UnityAction doDamage)
        {
            Debug.Log("[MainUI][Init]");

            if (!CheckUnityBindings()) return;

            ReturnCards.onClick.AddListener(returnCards);
            GetCard.onClick.AddListener(getCard);
            DoDamage.onClick.AddListener(doDamage);
        }

        private bool CheckUnityBindings()
        {
            if (ReturnCards == null) Debug.LogError("[MainUI][CheckUnityBindings] ReturnCards button not assigned!");
            if (GetCard == null) Debug.LogError("[MainUI][CheckUnityBindings] GetCard button not assigned!");
            if (DoDamage == null) Debug.LogError("[MainUI][CheckUnityBindings] DoDamage button not assigned!");

            return ReturnCards != null && GetCard != null && DoDamage != null;
        }
    }
}