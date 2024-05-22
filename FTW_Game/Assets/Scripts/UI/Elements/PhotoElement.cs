using System;
using UnityEngine;

namespace FTW.UI
{
    public class PhotoElement : MonoBehaviour
    {
        public static Action<int> OnPhotoSelected;
        public int Index;

        public void SelectPhoto()
        {
            OnPhotoSelected?.Invoke(Index);
        }
    }
}
