using System;
using UnityEngine;
using FTW.UI;
using FTW.Data;


namespace FTW.Core
{
    public class ShopController : MonoBehaviour
    {
        public static Action<int> OnEnergyAdd;
        public static Action<int> OnCubesAdd;
        public static Action<int> OnCubesDecrease;
        public static Action<bool> OnOffAds;
        public static Action OnAdsLaunch;

        [Header("View")]
        [SerializeField] private ShopView shopView;

        private GameManager GameManager => FindAnyObjectByType<GameManager>();
        private Settings Settings => GameManager.Save.Settings;
        private Data.Resources Resources => GameManager.Save.Resources;

        private void OnEnable()
        {
            PageController.OnShopCalled += UpdateView;
            PageController.OnGameListCalled += shopView.Disable;
            PageController.OnSettingsCalled += shopView.Disable;
        }

        private void OnDisable()
        {
            PageController.OnShopCalled -= UpdateView;
            PageController.OnGameListCalled -= shopView.Disable;
            PageController.OnSettingsCalled -= shopView.Disable;
        }

        public void UpdateView()
        {
            shopView.Enable();
            shopView.UpdateView(Resources.Energy < 10, Resources.Cubes >= 100, Settings.Ads);
        }

        public void EnergyByCubes()
        {
            OnEnergyAdd?.Invoke(1);
            OnCubesDecrease?.Invoke(100);
            UpdateView();
        }

        public void EnergyByAds()
        {
            OnEnergyAdd?.Invoke(1);
            OnAdsLaunch?.Invoke();
            UpdateView();
        }

        public void EnergyByRub()
        {
            OnEnergyAdd?.Invoke(1);
            UpdateView();
        }

        public void CubesByRubSmall()
        {
            OnCubesAdd?.Invoke(100);
            UpdateView();
        }

        public void CubesByRubBig()
        {
            OnCubesAdd?.Invoke(300);
            UpdateView();
        }

        public void CubesByAds()
        {
            OnCubesAdd?.Invoke(100);
            OnAdsLaunch?.Invoke();
            UpdateView();
        }

        public void AdsByRub()
        {
            OnOffAds?.Invoke(false);
            UpdateView();
        }
    }
}
