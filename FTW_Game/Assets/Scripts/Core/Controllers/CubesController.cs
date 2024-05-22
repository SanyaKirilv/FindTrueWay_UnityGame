using System.Collections.Generic;
using UnityEngine;
using FTW.Data;
using FTW.Game;
using FTW.UI;


namespace FTW.Core
{
    public class CubesController : MonoBehaviour
    {
        [Header("View's")]
        public List<CubeView> CubeViews;

        private GameManager GameManager => FindAnyObjectByType<GameManager>();
        private Data.Resources Resources { get => GameManager.Save.Resources; set => Resources = value; }

        private void OnEnable()
        {
            GameManager.CoinCheck += UpdateCubeViews;
            BaseGame.CoinAdd += AddCubes;
            ShopController.OnCubesAdd += AddCubes;
            ShopController.OnCubesDecrease += DecreaseCubes;
        }

        private void OnDisable()
        {
            GameManager.CoinCheck -= UpdateCubeViews;
            BaseGame.CoinAdd -= AddCubes;
            ShopController.OnCubesAdd -= AddCubes;
            ShopController.OnCubesDecrease += DecreaseCubes;
        }

        private void AddCubes(int value)
        {
            Resources.Cubes += value;
            UpdateCubeViews();
        }

        private void DecreaseCubes(int value)
        {
            Resources.Cubes -= value;
            UpdateCubeViews();
        }

        private void UpdateCubeViews()
        {
            foreach (var cubesView in CubeViews)
            {
                cubesView.UpdateView(Resources.Cubes);
            }
        }
    }
}
