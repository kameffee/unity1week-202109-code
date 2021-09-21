using kameffee.unity1week202109.Domain;
using kameffee.unity1week202109.Entity;
using UnityEngine;

namespace kameffee.unity1week202109.UseCase
{
    public class SeController
    {
        private readonly ISeModel seModel;
        private readonly SePreset sePreset;

        public SeController(ISeModel seModel, SePreset sePreset)
        {
            this.seModel = seModel;
            this.sePreset = sePreset;
        }

        public void Play(int id)
        {
            AudioClip clip = sePreset.GetAudioClip(id);
            seModel.Play(clip);
        }
    }
}