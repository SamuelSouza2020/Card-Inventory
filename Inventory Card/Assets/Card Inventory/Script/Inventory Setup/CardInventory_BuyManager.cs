using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardInventory
{
    public class CardInventory_BuyManager : MonoBehaviour
    {
        [SerializeField]
        Image _localResult;
        [SerializeField]
        private List<float> arrangePercentage;
        List<float> percentageOfWinning = new List<float>();
        [SerializeField]
        int cardPrice = 20;
        public GameObject Alert;

        void Start()
        {
            for (int i = 0; i < CardInventory_ControlOfTheCards.Instance.CardsAllGame.Count; i++)
            {
                percentageOfWinning.Add(0);
            }
        }
        //adds money to purchase cards
        public void MoreMoney(int value)
        {
            int yourMoney = Convert.ToInt32(CardInventory_ControlOfTheCards.Instance.YourMoneyTMP.text) + value;
            CardInventory_ControlOfTheCards.Instance.YourMoneyTMP.text = yourMoney.ToString();
        }
        public void BuyCard()
        {
            /*
             * Checks the quantity of elements in the purchase.
             * If you exceed the number of elements, the last indexes are removed.
             */
            if (CardInventory_ControlOfTheCards.Instance.CardsAllGame.Count != arrangePercentage.Count)
            {
                int missingValue = CardInventory_ControlOfTheCards.Instance.CardsAllGame.Count - arrangePercentage.Count;
                if (missingValue > 0)
                {
                    for (int i = 0; i < missingValue; i++)
                    {
                        arrangePercentage.Add(1);
                    }
                }
                else
                {
                    for (int i = 0; i > missingValue; i--)
                    {
                        arrangePercentage.RemoveAt(arrangePercentage.Count - 1);
                    }
                }
            }

            //Check if you have the value to buy the card
            int yourMoney = Convert.ToInt32(CardInventory_ControlOfTheCards.Instance.YourMoneyTMP.text);
            if (cardPrice <= yourMoney)
            {
                //To disable the hold for the animation, simply place the code from line 40 up to line 47 as comment or remove
                //Disables the object to show the card you won
                if (_localResult.transform.GetChild(0).gameObject.activeSelf)
                {
                    _localResult.transform.GetChild(0).gameObject.SetActive(false);
                    _localResult.GetComponent<Image>().enabled = false;
                }
                CardInventory_ControlOfTheCards.Instance.InventoryManager.BtBuy.interactable = false;
                //Active Button Buy
                StartCoroutine(CardAnimation());

                //reduce money
                MoreMoney(-20);
                //Active animation the card
                //Commenting on the line below disables the animation
                _localResult.transform.GetChild(0).gameObject.SetActive(true);
                _localResult.transform.GetChild(0).GetComponent<Animator>().Play("ShoppingAnimation");


                //Arranges the percentage of the list totaling 100 percent
                float percentageSummed = 0;
                for (int i = 0; i < arrangePercentage.Count; i++)
                {
                    percentageSummed += arrangePercentage[i];

                    //If the value is above 100 it will recalculate to leave 100% accurate
                    if (percentageSummed > 100)
                    {
                        /*
                         * If the value exceeds 100%, the last value is removed. The missing value
                         * is calculated to reach 100% and divided to add to all elements in the list.
                         */
                        int remainder = arrangePercentage.Count - i;
                        float valueMore = arrangePercentage[i];
                        percentageSummed -= valueMore;
                        float endValue = (100 - percentageSummed) / remainder;


                        //This "FOR" adds to the list the value of the split percentage
                        for (int j = 0; j < remainder; j++)
                        {
                            arrangePercentage[i + j] = endValue;
                            percentageSummed += endValue;
                        }
                        Debug.Log(endValue);
                        //For the "For" not to cause loop
                        break;
                    }
                }
                //Calculates the missing value to 100 %
                float remainingValue = 100 - percentageSummed;
                if (remainingValue > 1)
                {
                    //if more than 1% is missing, the remainder is split and passed to all elements on the list
                    float endValue = remainingValue / arrangePercentage.Count;

                    for (int i = 0; i < arrangePercentage.Count; i++)
                        arrangePercentage[i] += endValue;
                }

                //Quantity of elements to draw
                for (int i = 0; i < arrangePercentage.Count; i++)
                {
                    //Here simulates the amount of items to draw according to the percentage
                    percentageOfWinning[i] = (((float)CardInventory_ControlOfTheCards.Instance.
                        CardsAllGame.Count * 10) / 100) * arrangePercentage[i];
                    /*
                     *To know the chosen element it is drawn by number, the number of it increases according to the percentage.
                     *The value of the element is summed with the previous one, so we know what the numbers of element "X" are.
                     *Example: element[1] = 50 and element[2] = 80. Then from the number 51 to the 80
                     *belongs to the element[2], from the bottom 50 belongs to the element[1]
                    */

                    if (i > 0)
                        percentageOfWinning[i] += percentageOfWinning[i - 1];
                }
                //"Random" is used to draw a number of the elements of the list
                int randomCard = UnityEngine.Random.Range(0, Convert.ToInt32
                    (CardInventory_ControlOfTheCards.Instance.CardsAllGame.Count * 10));
                Debug.Log("Number Random: " + randomCard);
                //After drawn is passed by the command "FOR" to check which element belongs and show on the screen
                for (int i = 0; i < percentageOfWinning.Count; i++)
                {
                    if (i == 0)
                    {
                        if (randomCard <= percentageOfWinning[i])
                        {
                            _localResult.sprite = CardInventory_ControlOfTheCards.Instance.CardsAllGame[i];
                            WonCard(i);
                            break;
                        }
                    }
                    else if (randomCard <= percentageOfWinning[i] && randomCard > percentageOfWinning[i - 1])
                    {
                        _localResult.sprite = CardInventory_ControlOfTheCards.Instance.CardsAllGame[i];
                        WonCard(i);
                        break;
                    }
                }
                TotalNumberOfCards();
            }
            else
            {
                Alert.SetActive(true);
            }
        }
        /*
         * In this method is added the number of the card drawn in the list and
         * positioned in the same position as the list of sprites of the elements.
         */
        void WonCard(int value)
        {
            CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory[value]++;

            //To verify that the "CardsObtained" list already has the card drawn
            bool alreadyHave = false;

            for (int i = 0; i < CardInventory_ControlOfTheCards.Instance.CardsObtained.Count; i++)
            {
                if (CardInventory_ControlOfTheCards.Instance.CardsObtained[i] == value)
                    alreadyHave = true;
            }
            //If not, it adds the card in the same order as the "CardsAllGame" list
            if (!alreadyHave)
            {
                if (CardInventory_ControlOfTheCards.Instance.CardsObtained.Count > 0)
                {
                    for (int i = 0; i < CardInventory_ControlOfTheCards.Instance.CardsObtained.Count; i++)
                    {
                        if (value < CardInventory_ControlOfTheCards.Instance.CardsObtained[i])
                        {
                            CardInventory_ControlOfTheCards.Instance.CardsObtained.Insert(i, value);
                            break;
                        }
                        else if (i == CardInventory_ControlOfTheCards.Instance.CardsObtained.Count - 1)
                        {
                            CardInventory_ControlOfTheCards.Instance.CardsObtained.Add(value);
                            break;
                        }
                    }
                }//If it has, it adds as the first element
                else
                    CardInventory_ControlOfTheCards.Instance.CardsObtained.Add(value);
            }
        }
        //Checks the total amount of cards obtained
        public void TotalNumberOfCards()
        {
            int addUpCards = 0;
            //picks up the amount of cards
            for (int i = 0; i < CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory.Count; i++)
            {
                addUpCards += CardInventory_ControlOfTheCards.Instance.AmountOfCardInInventory[i];
            }
            CardInventory_ControlOfTheCards.Instance.NumberCardTMP.text = addUpCards.ToString();
        }
        //Timer to activate the purchase button and activate the obtained card.
        IEnumerator CardAnimation()
        {
            yield return new WaitForSeconds(1f);
            _localResult.GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            CardInventory_ControlOfTheCards.Instance.InventoryManager.BtBuy.interactable = true;
        }
    }
}
