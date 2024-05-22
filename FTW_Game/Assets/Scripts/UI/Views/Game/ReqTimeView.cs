using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class ReqTimeView : MonoBehaviour
    {
        public Text Time;

        private MoveElement Move => GetComponent<MoveElement>();

        public void UpdateView(int time)
        {
            Time.text = $"< {time / 60}:{string.Format("{0:d2}", time % 60)}";
        }

        public void Enable()
        {
            Move.Enable();
        }

        public void Disable()
        {
            Move.Disable();
        }
    }
}
