using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardInventory
{
    public class CardInventory_ControlOfTheCards : MonoBehaviour
    {
        //Cards you will use in the game
        public List<Sprite> CardsAllGame;

        /// <summary>
        /// The cards are identified by the list number.
        /// The initial quantity is generated automatically, but can be changed in the 
        /// inventory itself or manually.
        /// </summary>
        public List<int> DeckPlayerOfficial, CardsObtained, RarityOfCards, AmountOfCardInTheDeck, AmountOfCardInInventory;

        public CardInventory_InventoryManager InventoryManager;
        public CardInventory_BuyManager BuyManager;
        public TextMeshProUGUI NumberCardTMP, YourMoneyTMP;
        public GameObject CardGamePrefab, InfoCard;

        public static CardInventory_ControlOfTheCards Instance;

        /// <summary>
        /// SINGLETON method to connect the scripts and maintain during scenario switching. 
        /// Can be changed or removed. If removed, another method will be required to link 
        /// the scripts or pass the information.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            SceneManager.sceneLoaded += LoadObjects;
        }
        /*
         * Each time you change the scenario, this method is called.
         * In this method you can place commands to be loaded in specific scenes
         */
        void LoadObjects(Scene scena, LoadSceneMode mode)
        {
            InventoryManager = GameObject.Find("InventoryManager").GetComponent<CardInventory_InventoryManager>();
            BuyManager = GameObject.Find("BuyManager").GetComponent<CardInventory_BuyManager>();
            NumberCardTMP = GameObject.Find("NumberCardsInTheDeck").GetComponent<TextMeshProUGUI>();
            YourMoneyTMP = GameObject.Find("YourMoneyTMP").GetComponent<TextMeshProUGUI>();


            //This method will place the two lists with the same amount of index from the list "CardsAllGame"
            FillList();

            //To instantiate the cards
            InventoryManager.InstantiateCardsPlayer();

            //The script called within the IF will identify the scenario number through Build Setting
            //if (WhatScenario.instance.Phase == 0)
            //{
            //}
        }
        void FillList()
        {
            int cardDivision = CardsAllGame.Count / 3;
            for (int count = 0; count < CardsAllGame.Count; count++)
            {
                //Creating indexes and placing 0 elements
                AmountOfCardInTheDeck.Add(0);
                AmountOfCardInInventory.Add(0);

                //I simply defined the rarity of the card. Having 3 points: Common(2), rare(1) and epic(0)
                if (count < cardDivision)
                    RarityOfCards.Add(2);
                else if (count >= cardDivision && count < (cardDivision * 2))
                    RarityOfCards.Add(1);
                else
                    RarityOfCards.Add(0);
            }
            /*
             * Space manually place the rarity of each element of the list "CardsAllGame".
             * Example
             * RarityOfCards.Add(0);
             * RarityOfCards.Add(0);
             * RarityOfCards.Add(1);
             * In the sequence you put, will be added to the top-down list.
             * Delete these comments and put it here. Delete line 82 to 90
             */

            //Checks whether the lists have the same amount of elements
            if (RarityOfCards.Count != CardsAllGame.Count)
            {
                int missingValue = CardsAllGame.Count - RarityOfCards.Count;
                if (missingValue > 0)
                {
                    for (int i = 0; i < missingValue; i++)
                    {
                        RarityOfCards.Add(0);
                    }
                }
                else
                {
                    for (int i = 0; i > missingValue; i--)
                    {
                        RarityOfCards.RemoveAt(RarityOfCards.Count - 1);
                    }
                }
            }
        }
    }
}
