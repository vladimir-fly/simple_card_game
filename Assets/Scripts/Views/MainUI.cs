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
            ReturnCards.onClick.AddListener(returnCards);
            GetCard.onClick.AddListener(getCard);
            DoDamage.onClick.AddListener(doDamage);
        }
    }
}