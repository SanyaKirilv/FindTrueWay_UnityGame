using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class AuthView : MonoBehaviour
    {
        public Image Photo;
        public Button Button;
        public Text Name;
        public InputField InputField;

        public MoveElement Move => GetComponent<MoveElement>();
    }
}
