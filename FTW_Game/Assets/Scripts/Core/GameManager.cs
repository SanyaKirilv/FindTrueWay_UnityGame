using System;
using UnityEngine;
using FTW.Data;

namespace FTW.Core
{
    public class GameManager : MonoBehaviour
    {
        public static Action OnAuthCalled;
        public static Action EnergyCheck;
        public static Action CoinCheck;

        [Header("Saved data")]
        public Save Save;

        private SaveController SaveController => GetComponent<SaveController>();

        private void Start()
        {
            LoadData();
            OnAuthCalled?.Invoke();
            EnergyCheck?.Invoke();
            CoinCheck?.Invoke();
        }

        private void SaveData()
        {
            Save.ExitTime.SetTime(DateTime.Now);
            SaveController.SaveData(Save);
        }

        private void LoadData()
        {
            Save = SaveController.LoadData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}
