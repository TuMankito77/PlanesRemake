namespace PlanesRemake.Runtime.Sound
{
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.Views;

    [CreateAssetMenu(fileName = "ClipsContainer", menuName = "Sounds/ClipsContainer")]
    public class ClipsContanier : ScriptableObject
    {
        [SerializeField]
        private AudioClip[] clips = new AudioClip[0];

        private Dictionary<string, AudioClip> clipsById = new Dictionary<string, AudioClip>();

        public IReadOnlyDictionary<string, AudioClip> ClipsById => clipsById;

        public void Initialize()
        {
            if(clips.Length == 0)
            {
                return;
            }

            clipsById = new Dictionary<string, AudioClip>()
            {
                { ClipIds.COIN_CLIP, clips[0] } 
            };
        }
    }
}
