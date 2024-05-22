using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace FTW.UI
{
    public class AdsView : MonoBehaviour
    {
        [Header("Video Player")]
        public VideoPlayer Player;

        private MoveElement Move => GetComponent<MoveElement>();

        public void Enable(VideoClip clip)
        {
            Move.Enable();
            Player.clip = clip;
            Player.Play();
        }

        public void Disable()
        {
            Move.Disable();
            Player.Stop();
            Player.clip = null;
        }
    }
}
