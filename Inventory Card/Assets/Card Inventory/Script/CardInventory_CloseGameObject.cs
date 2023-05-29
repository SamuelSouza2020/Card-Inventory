using UnityEngine;

namespace CardInventory
{
    public class CardInventory_CloseGameObject : MonoBehaviour
    {
        public void CloseObject()
        {
            gameObject.SetActive(false);
        }
    }
}
