using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class LevelView : MonoBehaviour
    {
        public static Action<int> OnLevelSelected;
        public static Action<int> OnEnergyGain;

        [Header("Level index")]
        public int Index;

        [Header("UI elements")]
        [SerializeField] private GameObject block;
        [SerializeField] private List<GameObject> stars;
        [SerializeField] private Button button;
        [SerializeField] private Text index;

        public void SelectLevel()
        {
            OnLevelSelected?.Invoke(Index);
            OnEnergyGain?.Invoke(1);
        }

        public void SetUpLevel(int index, string state, int stars, bool isAvailable)
        {
            Index = index;
            this.index.text = (index + 1).ToString();
            block.SetActive(state == "Blocked");
            button.interactable = state != "Blocked" && isAvailable;
            for (int i = 0; i < this.stars.Count; i++)
            {
                this.stars[i].SetActive(stars - 1 == i);
            }
        }
    }
}
