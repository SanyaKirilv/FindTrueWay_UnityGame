using System.Collections.Generic;
using UnityEngine;

namespace FTW.UI
{
    public class CompleteView : MonoBehaviour
    {
        public List<GameObject> Stars;

        private MoveElement MoveElement => GetComponent<MoveElement>();

        public void Enable()
        {
            MoveElement.Enable();
        }

        public void Disable()
        {
            MoveElement.Disable();
        }
    }
}
