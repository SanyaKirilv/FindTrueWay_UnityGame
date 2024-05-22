using System;
using UnityEngine;

namespace FTW.Game
{
    public class BaseGame : MonoBehaviour
    {
        public static Action OnLevelStarted;
        public static Action OnLevelCompleted;
        public static Action<int> CoinAdd;
        public static Action<int> OnAudioPlay;

        public virtual void SetLevel(ScriptableObject level) { }
    }
}
