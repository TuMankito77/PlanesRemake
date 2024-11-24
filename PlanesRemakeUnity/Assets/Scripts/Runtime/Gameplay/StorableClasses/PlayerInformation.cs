namespace PlanesRemake.Runtime.Gameplay.StorableClasses
{
    using System.Collections.Generic;
    
    using UnityEngine;
    
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
        private int? coinsCollected = null;

        [JsonProperty]
        private int? wallsEvaded = null;

        [JsonProperty]
        private float? musicVolumeSet = null;

        [JsonProperty]
        private float? vfxVolumeSet = null;

        [JsonProperty]
        private string aircraftSelected = null;

        [JsonProperty]
        private List<string> aircraftsPurchased = null;

        [JsonProperty]
        private SystemLanguage? languageSelected = null;

        [JsonIgnore]
        public int CoinsCollected { get => coinsCollected.Value; set => coinsCollected = value; }

        [JsonIgnore]
        public int WallsEvaded { get => wallsEvaded.Value; set => wallsEvaded = value; }

        [JsonIgnore]
        public float MusicVolumeSet { get => musicVolumeSet.Value; set => musicVolumeSet = value; }
        
        [JsonIgnore]
        public float VfxVolumeSet { get => vfxVolumeSet.Value; set => vfxVolumeSet = value; }
        
        [JsonIgnore]
        public string AircraftSelected { get => aircraftSelected; set => aircraftSelected = value; }

        [JsonIgnore]
        public List<string> AircraftsPurchased { get => aircraftsPurchased; set => aircraftsPurchased = value; }

        [JsonIgnore]
        public SystemLanguage LanguageSelected { get => languageSelected.Value; set => languageSelected = value; }

        [JsonConstructor]
        public PlayerInformation(int? sourceCoinsCollected, int? sourceWallsEvaded, float? sourceMusicVolumeSet, float? sourceVfxVolumeSet, string sourceAircraftSelected, List<string> sourceAircraftsPurchased, SystemLanguage? sourceLanguageSelected)
        {
            coinsCollected = sourceCoinsCollected;
            wallsEvaded = sourceCoinsCollected;
            musicVolumeSet = sourceMusicVolumeSet;
            vfxVolumeSet = sourceVfxVolumeSet;
            aircraftSelected = sourceAircraftSelected;
            aircraftsPurchased = sourceAircraftsPurchased;
            languageSelected = sourceLanguageSelected;
        }

        //NOTE: This is to leave values that were not found in the class a their default values assigned at the moment of construction.
        //We are handling it in this way to prevent having the players' data erased completely because of a new value added on an upate.
        public void TrasferValidValues(ref PlayerInformation otherPlayerInformation)
        {
            if(coinsCollected != null)
            {
                otherPlayerInformation.CoinsCollected = coinsCollected.Value;
            }

            if(wallsEvaded != null)
            {
                otherPlayerInformation.WallsEvaded = wallsEvaded.Value;
            }

            if(musicVolumeSet != null)
            {
                otherPlayerInformation.MusicVolumeSet = musicVolumeSet.Value;
            }

            if(vfxVolumeSet != null)
            {
                otherPlayerInformation.VfxVolumeSet = vfxVolumeSet.Value;
            }

            if(aircraftSelected != null)
            {
                otherPlayerInformation.AircraftSelected = aircraftSelected;
            }

            if(aircraftsPurchased != null)
            {
                otherPlayerInformation.AircraftsPurchased = aircraftsPurchased;
            }

            if(languageSelected != null)
            {
                otherPlayerInformation.LanguageSelected = languageSelected.Value;
            }
        }
    }
}

