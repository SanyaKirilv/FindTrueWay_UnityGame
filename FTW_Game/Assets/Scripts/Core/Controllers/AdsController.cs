using System;
using UnityEngine;
using UnityEngine.Video;
using FTW.UI;
using System.Collections.Generic;
using System.Collections;


namespace FTW.Core
{
    public class AdsController : MonoBehaviour
    {
        public static Action<int> OnAudioPlay;
        public static Action<int> OnAudioStop;

        [Header("View")]
        [SerializeField] private AdsView adsView;
        [Header("Video's")]
        public List<VideoClip> ListVideoClips;

        private void OnEnable()
        {
            GameController.OnAdsLaunch += LaunchAds;
            ShopController.OnAdsLaunch += LaunchAds;
        }

        private void OnDisable()
        {
            GameController.OnAdsLaunch -= LaunchAds;
            ShopController.OnAdsLaunch -= LaunchAds;
        }

        public void LaunchAds()
        {
            VideoClip clip = ListVideoClips[UnityEngine.Random.Range(0, ListVideoClips.Count - 1)];
            adsView.Enable(clip);
            OnAudioStop?.Invoke(3);
            StartCoroutine(Ads((int)clip.length));
        }

        private IEnumerator Ads(int time)
        {
            while (time > 0)
            {
                time--;
                yield return new WaitForSeconds(1f);
            }
            adsView.Disable();
            OnAudioPlay?.Invoke(3);
        }
    }
}
