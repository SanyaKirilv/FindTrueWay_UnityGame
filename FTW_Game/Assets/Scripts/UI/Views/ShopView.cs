using UnityEngine;
using UnityEngine.UI;

namespace FTW.UI
{
    public class ShopView : MonoBehaviour
    {
        [Header("View's")]
        public MoveElement Label;
        public MoveElement LabelResources;
        public MoveElement Energy;
        public MoveElement Cube;
        public MoveElement Ads;
        [Header("Buttons's")]
        public Button energyByCubes;
        public Button energyByAds;
        public Button energyByRub;
        public Button adsByRub;


        private MoveElement Move => GetComponent<MoveElement>();

        public void Enable()
        {
            Move.Enable();
            Label.Enable();
            LabelResources.Enable();
            Energy.Enable();
            Cube.Enable();
            Ads.Enable();
        }

        public void Disable()
        {
            Move.Disable();
            Label.Disable();
            LabelResources.Disable();
            Energy.Disable();
            Cube.Disable();
            Ads.Disable();
        }

        public void UpdateView(bool energy, bool cubes, bool ads)
        {
            energyByCubes.interactable = energy && cubes;
            energyByAds.interactable = energy;
            energyByRub.interactable = energy;

            adsByRub.interactable = ads;
        }
    }
}
