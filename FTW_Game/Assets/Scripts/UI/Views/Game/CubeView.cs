using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class CubeView : MonoBehaviour
    {
        [Header("UI element")]
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

        public void UpdateView(int count)
        {
            CountText.text = $"{count}";
        }
    }
}
