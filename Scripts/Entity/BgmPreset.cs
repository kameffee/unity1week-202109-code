using System.Collections.Generic;
using UnityEngine;

namespace kameffee.unity1week202109.Entity
{
    [CreateAssetMenu(fileName = "BgmPreset", menuName = "Audio/BgmPreset")]
    public class BgmPreset : ScriptableObject
    {
        public List<AudioClip> AudioClips => audioClips;

        [SerializeField]
        private List<AudioClip> audioClips;

        public AudioClip GetAudioClip(int id) => audioClips[id];
    }
}
