namespace PlanesRemastered.Runtime.Core
{
    using UnityEngine;

    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
            GameManager gameManager = new GameManager();
        }
    }
}