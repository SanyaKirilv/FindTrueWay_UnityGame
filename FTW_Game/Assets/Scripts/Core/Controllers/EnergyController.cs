using System.Collections.Generic;
using UnityEngine;
using FTW.Data;
using FTW.UI;
using System.Collections;


namespace FTW.Core
{
    public class EnergyController : MonoBehaviour
    {
        [Header("View's")]
        public List<EnergyView> EnergyViews;

        private int maxEnergy = 10;
        private int timePerEnergy = 90;

        private GameManager GameManager => FindAnyObjectByType<GameManager>();
        private Data.Resources Resources { get => GameManager.Save.Resources; set => Resources = value; }
        private Data.Time ExitTime => GameManager.Save.ExitTime;

        private int EnergyDifference => maxEnergy - Resources.Energy;
        private int TimeDifference => (EnergyDifference * timePerEnergy) - (Resources.ElapsedTime + ExitTime.DifferenceInSeconds);

        private void OnEnable()
        {
            GameManager.EnergyCheck += StartEnergy;
            LevelView.OnEnergyGain += DecreaseEnergy;
            ShopController.OnEnergyAdd += AddEnergy;
        }

        private void OnDisable()
        {
            GameManager.EnergyCheck -= StartEnergy;
            LevelView.OnEnergyGain -= DecreaseEnergy;
            ShopController.OnEnergyAdd -= AddEnergy;
        }

        private void StartEnergy()
        {
            UpdateEnergyViews();
            CheckData();
        }

        private void CheckData()
        {
            UpdateEnergyViews();
            if (EnergyDifference == 0)
            {
                Resources.ElapsedTime = 0;
                StopCoroutine("AddEnergyCoroutine");
                return;
            }

            if (TimeDifference <= 0)
            {
                AddEnergy(EnergyDifference);
                return;
            }

            int energy = TimeDifference / timePerEnergy;
            AddEnergy(energy);
            StartCoroutine(AddEnergyCoroutine(TimeDifference % timePerEnergy));
        }

        private void AddEnergy(int value)
        {
            if (EnergyDifference > 0)
            {
                Resources.Energy += value;
            }
            if (EnergyDifference == 0)
            {
                Resources.ElapsedTime = 0;
                StopCoroutine("AddEnergyCoroutine");
            }
            UpdateEnergyViews();
        }

        private void DecreaseEnergy(int value)
        {
            Resources.Energy -= value;
            if (EnergyDifference == 1)
            {
                StartCoroutine(AddEnergyCoroutine(timePerEnergy));
            }
            UpdateEnergyViews();
        }

        private IEnumerator AddEnergyCoroutine(int time)
        {
            Resources.ElapsedTime = time;
            while (Resources.ElapsedTime > 0)
            {
                Resources.ElapsedTime--;
                UpdateEnergyViews();
                yield return new WaitForSeconds(1f);
            }
            AddEnergy(1);
            if (EnergyDifference > 0)
            {
                StartCoroutine(AddEnergyCoroutine(timePerEnergy));
            }
            UpdateEnergyViews();
        }

        private void UpdateEnergyViews()
        {
            foreach (var energyView in EnergyViews)
            {
                energyView.UpdateView(Resources.ElapsedTime, Resources.Energy, maxEnergy);
            }
        }
    }
}
