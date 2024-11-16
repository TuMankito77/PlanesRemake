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

        [JsonProperty]
        public float musicVolumeSet = 1;

        [JsonProperty]
        public float vfxVolumeSet = 1;

        [JsonProperty]
        public string aircraftSelected = AircraftIds.PLANE;

        public PlayerInformation(int sourceCoinsCollected, int sourceWallsEvaded, float sourceMusicVolumeSet, float sourceVfxVolumeSet)
        {
            coinsCollected = sourceCoinsCollected;
            wallsEvaded = sourceWallsEvaded;
            musicVolumeSet = sourceMusicVolumeSet;
            vfxVolumeSet = sourceVfxVolumeSet;
        }
    }
}

