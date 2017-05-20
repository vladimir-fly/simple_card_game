using System.Collections.Generic;

namespace SCG
{
    public class Mocks
    {
        public static List<CardModel> CardModelCollection1
        {
            get
            {
                return new List<CardModel>
                {
                    new CardModel("card1", 1, 10, "desc_1", "img_1"),
                    new CardModel("card2", 2, 10, "desc_2", "img_2"),
                    new CardModel("card3", 3, 10, "desc_3", "img_3"),
                    new CardModel("card4", 4, 10, "desc_4", "img_4"),
                    new CardModel("card5", 5, 10, "desc_5", "img_5"),
                    new CardModel("card6", 6, 10, "desc_6", "img_6"),
                    new CardModel("card7", 7, 10, "desc_7", "img_7")
                };
            }
        }

    }
}
