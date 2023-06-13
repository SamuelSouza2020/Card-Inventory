using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardInventory
{
    public class CardInventory_OrganizeCards : MonoBehaviour
    {
        List<int> cardsAmount = new List<int>();
        List<int> orderOld = new List<int>();
        List<int> orderRarity = new List<int>();
        int countValue = 0;

        // Toggles the visibility of options for organizing cards, such as by Quantity or Rarity.
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
        /*
        * Sorts the cards based on the selected value, which determines the sorting sequence.
        * Value 0: Sorts by quantity, from smallest to largest.
        * Value 1: Sorts by rarity, from ordinary to rarest.
        */
        public void SortCard(int value)
        {
            //Clears the lists that organize the cards to fill again.
            cardsAmount.Clear();
            orderOld.Clear();
            orderRarity.Clear();

            int numberCount = CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards.transform.childCount;
            GameObject localCard = CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards;
            int _cardsActive = 0;
            countValue = 0;
            /*
            * Determines the sequence in which the cards will be sorted.
            * Additional sequences can be added by increasing the "switch" cases.
            */
            switch (value)
            {
                case 0:
                    // Sequence 0: Sort by quantity, from smallest to largest.
                    for (int i = 0; i < numberCount; i++)
                    {
                        cardsAmount.Add(Convert.ToInt32(localCard.transform.GetChild(i).GetChild(0).
                            GetChild(0).GetComponent<TextMeshProUGUI>().text));
                        orderOld.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().NumberCard);
                        orderRarity.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().RarityCard);
                    }
                    for (int i = 0; i < cardsAmount.Count - 1; i++)
                    {
                        for (int j = 0; j < cardsAmount.Count - i - 1; j++)
                        {
                            if (cardsAmount[j] > cardsAmount[j + 1])
                                swap(j, j + 1);
                        }
                    }
                    _cardsActive = cardsAmount.Count;
                    for (int i = 0; i < cardsAmount.Count; i++)
                    {
                        Destroy(CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards
                            .transform.GetChild(_cardsActive - i - 1).gameObject);
                    }
                    for (int i = 0; i < cardsAmount.Count; i++)
                    {
                        GenerateCardSequence(orderOld[i]);
                    }
                    break;
                case 1:
                    // Sequence 1: Sort by rarity, from ordinary to rarest.
                    for (int i = 0; i < numberCount; i++)
                    {
                        cardsAmount.Add(Convert.ToInt32(localCard.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text));
                        orderOld.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().NumberCard);
                        orderRarity.Add(localCard.transform.GetChild(i).GetComponent<CardInventory_InventoryCard>().RarityCard);
                    }
                    for (int i = 0; i < orderRarity.Count - 1; i++)
                    {
                        for (int j = 0; j < orderRarity.Count - i - 1; j++)
                        {
                            if (orderRarity[j] > orderRarity[j + 1])
                                swap(j, j + 1);
                        }
                    }
                    _cardsActive = orderRarity.Count;
                    for (int i = 0; i < orderRarity.Count; i++)
                    {
                        Destroy(CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards.transform.GetChild(_cardsActive - i - 1).gameObject);
                    }
                    for (int i = 0; i < orderOld.Count; i++)
                    {
                        GenerateCardSequence(orderOld[i]);
                    }
                    break;
            }
        }
        /*
         * Swaps the values at the given indices in the internal lists.
         * The firstValue and secondValue represent the indices of the values to be exchanged.
         */
        void swap(int firstValue, int secondValue)
        {
            int guardValue = cardsAmount[firstValue];
            cardsAmount[firstValue] = cardsAmount[secondValue];
            cardsAmount[secondValue] = guardValue;

            int guardValue2 = orderRarity[firstValue];
            orderRarity[firstValue] = orderRarity[secondValue];
            orderRarity[secondValue] = guardValue2;

            int guardExchangeValue = orderOld[firstValue];
            orderOld.RemoveAt(firstValue);
            orderOld.Insert(secondValue, guardExchangeValue);
        }
        /*
        * Instantiates a card with the new defined sequence.
        * The value parameter represents the index of the card in the new sequence.
        */
        void GenerateCardSequence(int value)
        {
            var spawn = Instantiate(CardInventory_ControlOfTheCards.Instance.CardGamePrefab, CardInventory_ControlOfTheCards.Instance.InventoryManager.LocalCards.transform);
            spawn.GetComponent<Image>().sprite = CardInventory_ControlOfTheCards.Instance.CardsAllGame[value];
            spawn.transform.GetChild(0).gameObject.SetActive(true);

            spawn.GetComponent<CardInventory_InventoryCard>().NumberCard = value;

            if (!CardInventory_ControlOfTheCards.Instance.InventoryManager.BtAllCards.interactable)
            {
                spawn.transform.GetChild(1).gameObject.SetActive(false);
                spawn.transform.GetChild(2).gameObject.SetActive(false);
                spawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory[value].ToString();
                spawn.transform.GetChild(3).GetComponent<Image>().color = new Color(1f, 0.96f, 0.1745283f, 1f);
                spawn.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ADD";
            }
            else
            {
                spawn.transform.GetChild(1).gameObject.SetActive(true);
                spawn.transform.GetChild(2).gameObject.SetActive(true);
                spawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = CardInventory_ControlOfTheCards.Instance.AmountOfCardInTheDeck[value].ToString();
                spawn.transform.GetChild(3).GetComponent<Image>().color = new Color(1f, 0.28f, 0.28f, 1f);
                spawn.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "X";
            }
            spawn.GetComponent<CardInventory_InventoryCard>().RarityCard = orderRarity[countValue];
            spawn.GetComponent<CardInventory_InventoryCard>().TypeCard();
            countValue++;
        }
    }
}