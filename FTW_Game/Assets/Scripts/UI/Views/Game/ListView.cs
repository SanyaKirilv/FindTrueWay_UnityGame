using UnityEngine;

namespace FTW.UI
{
    public class ListView : MonoBehaviour
    {
        [Header("View's")]
        public MoveElement Label;
        public MoveElement LabelResources;
        public MoveElement Object;

        private MoveElement Move => GetComponent<MoveElement>();

        public void Enable()
        {
            Move.Enable();
            Label.Enable();
            LabelResources.Enable();
            Object.Enable();
        }

        public void Disable()
        {
            Move.Disable();
            Label.Disable();
            LabelResources.Disable();
            Object.Disable();
        }
    }
}
