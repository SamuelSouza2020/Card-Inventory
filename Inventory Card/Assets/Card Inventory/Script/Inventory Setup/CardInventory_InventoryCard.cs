using Newtonsoft.Json.Linq;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardInventory
{
    public class CardInventory_InventoryCard : MonoBehaviour
    {
        public int NumberCard, RarityCard;
        bool checkRepeat = false;
        void Start()
        {
            TypeCard();
        }
        public void OptionCards()
        {
            //calls a method from another script to verify that the card buttons are disabled.
            CardInventory_ControlOfTheCards.Instance.InventoryManager.CheckCards();
            //Activates the button that has been pressed.
            transform.GetChild(4).gameObject.SetActive(true);
            //Checks if the card is in the deck, if it is not, it enables an option to add.
            if (CardInventory_ControlOfTheCards.Instance.InventoryManager.BtCardsDeck.interactable)
            {
                for (int i = 0; i < CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Count; i++)
                {
                    if (CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial[i] == NumberCard)
                        checkRepeat = true;
                }
            }
            if (!checkRepeat)
                transform.GetChild(3).gameObject.SetActive(true);
        }
        //This method is to remove or add the card in the deck.
        public void RemoveAndAddCards()
        {
            if (!CardInventory_ControlOfTheCards.Instance.InventoryManager.BtCardsDeck.interactable)
            {
                //Removes the amount of cards from the Text
                int total = Convert.ToInt32(CardInventory_ControlOfTheCards.Instance.NumberCardTMP.text);
                total -= Convert.ToInt32(gameObject.transform.GetChild(0).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text);
                CardInventory_ControlOfTheCards.Instance.NumberCardTMP.text = total.ToString();
                //Removes the card content from the two lists and destroys the gameobject.
                CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Remove(NumberCard);
                CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[NumberCard] = 0;

                Destroy(gameObject);
            }
            else
            {
                /*
                 * Adds the gameobject and content in the same order as the "CardsAllGame" 
                 * list, the card numbers are used to adjust the position in the list.
                 */
                int countDeck = CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Count;
                if (countDeck > 0)
                {
                    for (int i = 0; i < countDeck; i++)
                    {
                        if (CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial[i] > NumberCard)
                        {
                            CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Insert(i, NumberCard);
                            CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[NumberCard]++;
                            break;
                        }
                        else if (i == (CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Count - 1))
                        {
                            CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Add(NumberCard);
                            CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[NumberCard]++;
                            break;
                        }
                    }
                }
                else
                {
                    CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Add(NumberCard);
                    CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[NumberCard]++;
                }
                transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        public void AddingCards(bool moreCard)
        {
            //Turn the text into integer by placing it in a variable, then return to the text.
            int numberOfCards = Convert.ToInt32(gameObject.transform.GetChild(0).
                GetChild(0).GetComponent<TextMeshProUGUI>().text);
            //This method serves to add and remove the number of repetitions of the card in the deck.
            //GetChild 1 and 2 are to change the amount that the card is repeated in the deck.
            if (moreCard)
            {
                if (numberOfCards < 5 && numberOfCards < CardInventory_ControlOfTheCards.
                    Instance.AmountOfCardInInventory[NumberCard])
                {
                    CardControl(+1);
                    gameObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                }
                else
                {
                    gameObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                if (numberOfCards > 1)
                {
                    CardControl(-1);
                    gameObject.transform.GetChild(2).GetComponent<Button>().interactable = true;
                }

                if (numberOfCards <= 1)
                    gameObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
            }
        }
        /*
         * By default it has been placed that the retry limit is 5 (Can be changed), if it reaches
         * 5 the increase button is disabled, if it reaches 1 the decrease button is disabled.
         */
        void CardControl(int value)
        {
            //How many of this card has.
            int numberOfCards = Convert.ToInt32(gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text);
            //How many cards the deck has.
            int cardsInTheDeck = Convert.ToInt32(CardInventory_ControlOfTheCards.Instance.NumberCardTMP.text);

            gameObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
            numberOfCards += value;
            cardsInTheDeck += value;
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().
                text = numberOfCards.ToString();
            //Here adds or removes the value from the list and TMP.
            CardInventory_ControlOfTheCards.Instance.NumberCardTMP.text = cardsInTheDeck.ToString();
            CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[NumberCard] += value;
        }
        public void TypeCard()
        {
            //Here is applied color of the rarity of the card
            switch (RarityCard)
            {
                case 0:
                    gameObject.transform.GetChild(5).GetComponent<Image>().color = new Color(0f, 0.82f, 1f, 1f);
                    break;
                case 1:
                    gameObject.transform.GetChild(5).GetComponent<Image>().color = new Color(1f, 0.58f, 0f, 1f);
                    break;
                case 2:
                    gameObject.transform.GetChild(5).GetComponent<Image>().color = new Color(0.72f, 0f, 1f, 1f);
                    break;
            }
        }
        //In this method is shown in the card information its rarity
        public void InfoTypeCard(int value)
        {
            switch (value)
            {
                case 0:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().
                        text = "Rarity: Common";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().
                        color = new Color(0f, 0.82f, 1f, 1f);
                    break;
                case 1:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().
                        text = "Rarity: Rare";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().
                        color = new Color(1f, 0.58f, 0f, 1f);
                    break;
                case 2:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().
                        text = "Rarity: Epic";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().
                        color = new Color(0.72f, 0f, 1f, 1f);
                    break;
            }
        }
        //In this method the card information is placed in the visual panel
        public void CardDefinition()
        {
            CardInventory_ControlOfTheCards.Instance.InfoCard.SetActive(true);
            CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(0).GetComponent<Image>().sprite =
                CardInventory_ControlOfTheCards.Instance.CardsAllGame[NumberCard];

            if (NumberCard >= 0 && NumberCard < 4)
            {
                InfoTypeCard(2);
            }
            else if (NumberCard >= 4 && NumberCard < 8)
            {
                InfoTypeCard(1);
            }
            else
            {
                InfoTypeCard(0);
            }

            switch (NumberCard)
            {
                case 0:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Sword";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: The sword is the best weapon to fight tactically.";
                    break;
                case 1:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Shild";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: a broad piece of metal, held by straps, used as a protection against blows.";
                    break;
                case 2:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: War hammer";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: war hammer, widely used in sturdy armor.";
                    break;
                case 3:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Bow and Arrow";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: The bow and arrow is a long-range weapon.";
                    break;
                case 4:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Battle axe";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: A battle axe delivers more damage than a sword, despite its negative points.";
                    break;
                case 5:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Combat Spear";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: Spears are considered the ultimate hand-to-hand weapon.";
                    break;
                case 6:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Healing Potion";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: The Healing Potion is a healing item that restores health when used";
                    break;
                case 7:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Spyglass";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: See what your opponent hides";
                    break;
                case 8:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Magic Book";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: Use the book to improve your magic";
                    break;
                case 9:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Mix the Cards";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: Mix all the cards and get new ones";
                    break;
                case 10:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Poison Pot";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: Poison your enemy and watch him lose his life slowly";
                    break;
                case 11:
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                        text = "Name: Reset";
                    CardInventory_ControlOfTheCards.Instance.InfoCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                        text = "Description: New chance, try again. Restart the attributes. With the exception of life, everything goes back to how it started.";
                    break;
            }
        }
    }
}
