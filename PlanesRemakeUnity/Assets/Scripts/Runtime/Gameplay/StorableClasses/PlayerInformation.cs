namespace PlanesRemake.Runtime.Gameplay.StorableClasses
{
    using Newtonsoft.Json;

    using PlanesRemake.Runtime.SaveTool;

    public class PlayerInformation : IStorable
    {
        #region IStorable

        public string Key 
        { 
            get
            {
                return nameof(PlayerInformation);
            }
        }

        #endregion
        
        [JsonProperty]
        public int coinsCollected = 0;

        [JsonProperty]
        public int wallsEvaded = 0;

        public PlayerInformation(int sourceCoinsCollected, int sourceWallsEvaded)
        {
            coinsCollected = sourceCoinsCollected;
            wallsEvaded = sourceWallsEvaded;
        }
    }
}

