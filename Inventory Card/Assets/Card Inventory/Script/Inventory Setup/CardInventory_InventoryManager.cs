using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardInventory
{
    public class CardInventory_InventoryManager : MonoBehaviour
    {
        public GameObject LocalCards, LocalBuy;
        [SerializeField]
        int _cardsActive;
        public Button BtCardsDeck, BtAllCards, BtShop, BtBuy, BtOrder;
        void Start()
        {
            BtCardsDeck.interactable = false;
            BtBuy.gameObject.SetActive(false);
            LocalBuy.gameObject.SetActive(false);
        }
        //Calls each card in the deck to be instantiated following the list
        public void InstantiateCardsPlayer()
        {
            BtAllCards.interactable = true;
            BtCardsDeck.interactable = false;
            BtShop.interactable = true;
            BtBuy.gameObject.SetActive(false);
            LocalBuy.gameObject.SetActive(false);

            DeleteChildren();

            int amountDeck = CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial.Count;
            int amountInventory = CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory.Count;

            if (amountDeck > 0 && amountDeck < amountInventory)
            {
                for (int i = 0; i < amountDeck; i++)
                {
                    SpawnCardsDeck(CardInventory_ControlOfTheCards.Instance.DeckPlayerOfficial[i]);
                }
            }
            else if (amountDeck == amountInventory)
            {
                for (int i = 0; i < amountInventory; i++)
                {
                    SpawnCardsDeck(i);
                }
            }
            StartCoroutine(TimeToCountCards());
        }
        //Calls each card to be instantiated in the sequence of the list
        public void InstantiateAllCards()
        {
            BtCardsDeck.interactable = true;
            BtAllCards.interactable = false;
            BtShop.interactable = true;
            BtOrder.gameObject.SetActive(true);
            BtBuy.gameObject.SetActive(false);
            LocalBuy.gameObject.SetActive(false);
            //Destroys the cards that were on the screen
            DeleteChildren();

            int amountObtained = CardInventory_ControlOfTheCards.Instance.CardsObtained.Count;
            int amountInventory = CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory.Count;

            if (amountObtained > 0 && amountObtained < amountInventory)
            {
                for (int i = 0; i < amountObtained; i++)
                {
                    SpawnCardsAll(CardInventory_ControlOfTheCards.Instance.CardsObtained[i]);
                }
            }
            else if (amountObtained == amountInventory)
            {
                for (int i = 0; i < amountInventory; i++)
                {
                    SpawnCardsAll(i);
                }
            }
            StartCoroutine(TimeToCountCards());
        }
        //Instantiate cards that have been purchased
        void SpawnCardsAll(int value)
        {
            var spawn = Instantiate(CardInventory_ControlOfTheCards.Instance.CardGamePrefab, LocalCards.transform);

            CardConfiguration(spawn, value);
            spawn.transform.GetChild(1).gameObject.SetActive(false);
            spawn.transform.GetChild(2).gameObject.SetActive(false);

            spawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory[value].ToString();
            spawn.transform.GetChild(3).GetComponent<Image>().color = new Color(1f, 0.96f, 0.1745283f, 1f);
            spawn.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ADD";
        }
        //Instantiate cards belonging to the deck
        void SpawnCardsDeck(int value)
        {
            var spawn = Instantiate(CardInventory_ControlOfTheCards.Instance.CardGamePrefab, LocalCards.transform);

            CardConfiguration(spawn, value);
            spawn.transform.GetChild(1).gameObject.SetActive(true);
            spawn.transform.GetChild(2).gameObject.SetActive(true);

            spawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[value].ToString();
            spawn.transform.GetChild(3).GetComponent<Image>().color = new Color(1f, 0.28f, 0.28f, 1f);
            spawn.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "X";
        }
        //Adds the card number and its quantity.
        void CardConfiguration(GameObject spawn, int value)
        {
            spawn.GetComponent<Image>().sprite = CardInventory_ControlOfTheCards.Instance.CardsAllGame[value];
            spawn.transform.GetChild(0).gameObject.SetActive(true);
            spawn.GetComponent<CardInventory_InventoryCard>().NumberCard = value;
            spawn.GetComponent<CardInventory_InventoryCard>().RarityCard = CardInventory_ControlOfTheCards.Instance.RarityOfCards[value];
        }
        //Disables the options to add/remove cards and card information
        public void CheckCards()
        {
            _cardsActive = LocalCards.transform.childCount;
            for (int i = 0; i < _cardsActive; i++)
            {
                if (LocalCards.transform.GetChild(i).GetChild(4).gameObject.activeSelf)
                {
                    LocalCards.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                    LocalCards.transform.GetChild(i).GetChild(4).gameObject.SetActive(false);
                }
            }
        }
        //Activates the mini store with the ability to receive the card
        public void BuyCardsInventory()
        {
            BtCardsDeck.interactable = true;
            BtAllCards.interactable = true;
            BtShop.interactable = false;
            BtOrder.gameObject.SetActive(false);
            BtBuy.gameObject.SetActive(true);
            LocalBuy.gameObject.SetActive(true);
            //Shows how many cards you've got
            CardInventory_ControlOfTheCards.Instance.BuyManager.TotalNumberOfCards();
            DeleteChildren();
        }
        //Deletes all cards (child objects from the location of the cards).
        void DeleteChildren()
        {
            _cardsActive = LocalCards.transform.childCount;
            if (_cardsActive > 0)
            {
                for (int i = 0; i < _cardsActive; i++)
                {
                    Destroy(LocalCards.transform.GetChild(_cardsActive - i - 1).gameObject);
                }
            }
        }
        //Int method that returns the value of the amount of active cards
        int TotalCards()
        {
            int addUpCards = 0;
            _cardsActive = LocalCards.transform.childCount;
            //Picks up the amount of cards on the screen
            for (int i = 0; i < _cardsActive; i++)
            {
                addUpCards += Convert.ToInt32(LocalCards.transform.GetChild(i).GetChild(0).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text);
            }
            return addUpCards;
        }
        //Timer to start counting the active cards
        IEnumerator TimeToCountCards()
        {
            yield return new WaitForSeconds(0.1f);
            CardInventory_ControlOfTheCards.Instance.NumberCardTMP.text = TotalCards().ToString();
        }
    }
}
