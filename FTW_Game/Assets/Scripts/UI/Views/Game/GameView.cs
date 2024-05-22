using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class GameView : MonoBehaviour
    {
        [Header("Views")]
        public MoveElement Label;
        public List<ReqTimeView> Reqs;
        public MoveElement ClearButton;
        [Header("Timer")]
        public Text Timer;

        private MoveElement Move => GetComponent<MoveElement>();

        public void Enable()
        {
            Move.Enable();
            Label.Enable();
            foreach (ReqTimeView Req in Reqs)
            {
                Req.Enable();
            }
            ClearButton.Enable();
        }

        public void Disable()
        {
            Move.Disable();
            Label.Disable();
            foreach (ReqTimeView Req in Reqs)
            {
                Req.Disable();
            }
            ClearButton.Disable();
        }

        public void UpdateView(int time)
        {
            Timer.text = $"{time / 60}:{string.Format("{0:d2}", time % 60)}";
        }
    }
}
