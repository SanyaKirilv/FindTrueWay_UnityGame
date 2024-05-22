using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class EnergyView : MonoBehaviour
    {
        [Header("Element")]
        public GameObject TimerObject;
        [Header("UI element's")]
        public Text TimerText;
        public Text CountText;

        private MoveElement Move => GetComponent<MoveElement>();

        public void Enable()
        {
            Move.Enable();
        }

        public void Disable()
        {
            Move.Disable();
        }

        public void UpdateView(int time, int count, int maxCount)
        {
            TimerObject.SetActive(count < maxCount);
            TimerText.text = $"{time / 60}:{string.Format("{0:d2}", time % 60)}";
            CountText.text = $"{count}/{maxCount}";
        }
    }
}
