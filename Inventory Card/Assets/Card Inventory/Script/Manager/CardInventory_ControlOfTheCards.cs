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
         * This method is called each time the scenario changes.
         * You can add commands here to be loaded in specific scenes.
         */
        void LoadObjects(Scene scena, LoadSceneMode mode)
        {
            // Find and assign the necessary components and objects
            InventoryManager = GameObject.Find("InventoryManager").GetComponent<CardInventory_InventoryManager>();
            BuyManager = GameObject.Find("BuyManager").GetComponent<CardInventory_BuyManager>();
            NumberCardTMP = GameObject.Find("NumberCardsInTheDeck").GetComponent<TextMeshProUGUI>();
            YourMoneyTMP = GameObject.Find("YourMoneyTMP").GetComponent<TextMeshProUGUI>();

            // Populate the lists with the same number of elements as the "CardsAllGame" list
            FillList();

            // Instantiate the player's cards
            InventoryManager.InstantiateCardsPlayer();

            // The following code is commented out as it appears to be incomplete
            // and requires further implementation
            // It seems to be related to identifying the scenario number
            // based on the current build settings
            /*
            if (WhatScenario.instance.Phase == 0)
            {
                // Implementation for a specific scenario
            }
            */
        }
        void FillList()//Fills the lists with card data, including rarity and amount information.
        {
            const int CommonRarity = 2;
            const int RareRarity = 1;
            const int EpicRarity = 0;

            int cardDivision = CardsAllGame.Count / 3;

            FillAmountOfCards();
            FillRarityOfCards(cardDivision, CommonRarity, RareRarity, EpicRarity);
            ValidateListSizes();
        }
        void FillAmountOfCards()//Fills the amount lists with initial values for each card.
        {
            for (int i = 0; i < CardsAllGame.Count; i++)
            {
                // Creating indexes and placing 0 elements
                AmountOfCardInTheDeck.Add(0);
                AmountOfCardInInventory.Add(0);
            }
        }
        //Assigns rarity values to the cards based on the provided divisions.
        void FillRarityOfCards(int cardDivision, int commonRarity, int rareRarity, int epicRarity)
        {
            for (int i = 0; i < CardsAllGame.Count; i++)
            {
                if (i < cardDivision)
                    RarityOfCards.Add(commonRarity);
                else if (i < (cardDivision * 2))
                    RarityOfCards.Add(rareRarity);
                else
                    RarityOfCards.Add(epicRarity);
            }
        }
        //Ensures that the size of the rarity list matches the size of the card list and adjusts it if necessary.
        void ValidateListSizes()
        {
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
                else if (missingValue < 0)
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
