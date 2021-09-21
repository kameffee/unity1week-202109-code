using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace kameffee.unity1week202109.Entity
{
    [CreateAssetMenu(fileName = "SePreset", menuName = "Audio/SePreset")]
    public class SePreset : ScriptableObject
    {
        public List<AudioClip> AudioClips => audioClips;

        [SerializeField]
        private List<AudioClip> audioClips;

        public AudioClip GetAudioClip(int id)
        {
            Assert.IsTrue(id < audioClips.Count);
            Assert.IsTrue(0 <= id);
            return audioClips[id];
        }
    }
}