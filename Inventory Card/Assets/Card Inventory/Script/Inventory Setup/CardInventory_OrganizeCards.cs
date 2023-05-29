using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardInventory
{
    public class CardInventory_OrganizeCards : MonoBehaviour
    {
        List<int> _cardsAmount = new List<int>(), _orderOld = new List<int>(), _orderRarity = new List<int>();
        int _countValue = 0;

        //Shows options for organizing cards, such as: Quantity or Rarity.
        public void OpenOrganizationButtons()
        {
            if (!gameObject.transform.GetChild(1).gameObject.activeSelf)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        public void SortCard(int value)
        {
            //Clears the lists that organize the cards to fill again.
            _cardsAmount.Clear();
            _orderOld.Clear();
            _orderRarity.Clear();

            int numberCount = CardInventory_ControlOfTheCards.Instance.InventoryManager.
                LocalCards.transform.childCount;
            GameObject localCard = CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards;
            int _cardsActive = 0;
            _countValue = 0;
            /*
             * Here it is defined which sequence the cards will follow, and another type of sequence can
             * be added only needing to increase the "Switch".
             */
            switch (value)
            {
                case 0:
                    //Sequence 0, by quantity. From the smallest to the biggest.
                    for (int i = 0; i < numberCount; i++)
                    {
                        _cardsAmount.Add(Convert.ToInt32(localCard.transform.GetChild(i).GetChild(0).
                            GetChild(0).GetComponent<TextMeshProUGUI>().text));
                        _orderOld.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().NumberCard);
                        _orderRarity.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().RarityCard);
                    }
                    for (int i = 0; i < _cardsAmount.Count - 1; i++)
                    {
                        for (int j = 0; j < _cardsAmount.Count - i - 1; j++)
                        {
                            if (_cardsAmount[j] > _cardsAmount[j + 1])
                                swap(j, j + 1);
                        }
                    }
                    _cardsActive = _cardsAmount.Count;
                    for (int i = 0; i < _cardsAmount.Count; i++)
                    {
                        Destroy(CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards
                            .transform.GetChild(_cardsActive - i - 1).gameObject);
                    }
                    for (int i = 0; i < _cardsAmount.Count; i++)
                    {
                        GenerateCardSequence(_orderOld[i]);
                    }
                    break;
                case 1:
                    //Sequence 1, by rarity. From the ordinary to the rarest.
                    for (int i = 0; i < numberCount; i++)
                    {
                        _cardsAmount.Add(Convert.ToInt32(localCard.transform.GetChild(i).GetChild(0).
                            GetChild(0).GetComponent<TextMeshProUGUI>().text));
                        _orderOld.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().NumberCard);
                        _orderRarity.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().RarityCard);
                    }
                    for (int i = 0; i < _orderRarity.Count - 1; i++)
                    {
                        for (int j = 0; j < _orderRarity.Count - i - 1; j++)
                        {
                            if (_orderRarity[j] > _orderRarity[j + 1])
                                swap(j, j + 1);
                        }
                    }
                    _cardsActive = _orderRarity.Count;
                    for (int i = 0; i < _orderRarity.Count; i++)
                    {
                        Destroy(CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards
                            .transform.GetChild(_cardsActive - i - 1).gameObject);
                    }
                    for (int i = 0; i < _orderOld.Count; i++)
                    {
                        GenerateCardSequence(_orderOld[i]);
                    }
                    break;
            }
        }
        //Adds in the internal lists the values of the index exchanged
        void swap(int firstValue, int secondValue)
        {
            int guardValue = _cardsAmount[firstValue];
            _cardsAmount[firstValue] = _cardsAmount[secondValue];
            _cardsAmount[secondValue] = guardValue;
            int guardValue2 = _orderRarity[firstValue];
            _orderRarity[firstValue] = _orderRarity[secondValue];
            _orderRarity[secondValue] = guardValue2;
            int guardExchangeValue = _orderOld[firstValue];
            _orderOld.RemoveAt(firstValue);
            _orderOld.Insert(secondValue, guardExchangeValue);
        }
        //instantiation cards by the new sequence defined.
        void GenerateCardSequence(int value)
        {
            var spawn = Instantiate(CardInventory_ControlOfTheCards.Instance.CardGamePrefab, CardInventory_ControlOfTheCards.
                Instance.InventoryManager.LocalCards.transform);
            spawn.GetComponent<Image>().sprite = CardInventory_ControlOfTheCards.Instance.CardsAllGame[value];
            spawn.transform.GetChild(0).gameObject.SetActive(true);

            spawn.GetComponent<CardInventory_InventoryCard>().NumberCard = value;

            if (!CardInventory_ControlOfTheCards.Instance.InventoryManager.BtAllCards.interactable)
            {
                spawn.transform.GetChild(1).gameObject.SetActive(false);
                spawn.transform.GetChild(2).gameObject.SetActive(false);
                spawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                    = CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory[value].ToString();
                spawn.transform.GetChild(3).GetComponent<Image>().color = new Color(1f, 0.96f, 0.1745283f, 1f);
                spawn.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ADD";
            }
            else
            {
                spawn.transform.GetChild(1).gameObject.SetActive(true);
                spawn.transform.GetChild(2).gameObject.SetActive(true);
                spawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                    = CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[value].ToString();
                spawn.transform.GetChild(3).GetComponent<Image>().color = new Color(1f, 0.28f, 0.28f, 1f);
                spawn.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "X";
            }
            spawn.GetComponent<CardInventory_InventoryCard>().RarityCard = _orderRarity[_countValue];
            spawn.GetComponent<CardInventory_InventoryCard>().TypeCard();
            _countValue++;
        }
    }
}