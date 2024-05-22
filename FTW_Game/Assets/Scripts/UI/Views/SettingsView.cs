using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class SettingsView : MonoBehaviour
    {
        [Header("Player")]
        public Image Photo;
        public Text Name;

        [Header("Main")]
        public Text Registration;
        public Text Level;
        public Text Star;

        [Header("Settings")]
        public MoveElement MoveTheme;
        public MoveElement MoveLanguage;
        public MoveElement MoveAudio;

        [Header("Views")]
        public MoveElement MovePlayer;
        public MoveElement MoveMain;
        public MoveElement MoveSettings;

        private MoveElement Move => GetComponent<MoveElement>();

        public void Enable()
        {
            Move.Enable();
            MovePlayer.Enable();
            MoveMain.Enable();
            MoveSettings.Enable();
        }

        public void Disable()
        {
            Move.Disable();
            MovePlayer.Disable();
            MoveMain.Disable();
            MoveSettings.Disable();
        }
    }
}
