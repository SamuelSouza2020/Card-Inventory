using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardInventory
{
    public class CardInventory_WhatScenario : MonoBehaviour
    {
        public int Phase = -1;
        public string PhaseN;

        public static CardInventory_WhatScenario instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            SceneManager.sceneLoaded += CheckPhase;
        }
        void CheckPhase(Scene scena, LoadSceneMode mode)
        {
            Phase = SceneManager.GetActiveScene().buildIndex;
            PhaseN = SceneManager.GetActiveScene().name;
        }
    }
}
