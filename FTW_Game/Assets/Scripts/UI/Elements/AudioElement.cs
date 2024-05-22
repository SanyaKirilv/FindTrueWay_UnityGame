using UnityEngine;
using FTW.Game;
using FTW.Core;

namespace FTW.UI
{
    public class AudioElement : MonoBehaviour
    {
        private void OnEnable()
        {
            SettingsController.OnAudioChanged += Change;
            BaseGame.OnAudioPlay += Play;
            GameController.OnAudioPlay += Play;
            AdsController.OnAudioPlay += Play;
            AdsController.OnAudioStop += Stop;
        }

        private void OnDisable()
        {
            SettingsController.OnAudioChanged -= Change;
            BaseGame.OnAudioPlay -= Play;
            GameController.OnAudioPlay -= Play;
            AdsController.OnAudioPlay -= Play;
            AdsController.OnAudioStop -= Stop;
        }

        private void Change(bool state)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(state);
            }
        }

        public void Play(int index)
        {
            transform.GetChild(index).GetComponentInChildren<AudioSource>().Play();
        }

        public void Stop(int index)
        {
            transform.GetChild(index).GetComponentInChildren<AudioSource>().Play();
        }
    }
}
