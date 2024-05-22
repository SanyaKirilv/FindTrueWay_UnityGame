using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTW.Data;
using FTW.UI;

namespace FTW.Core
{
    public class AuthController : MonoBehaviour
    {
        public static Action OnAuthCompleted;

        [Header("View's")]
        [SerializeField] private AuthView authView;
        [SerializeField] private MoveElement photosView;

        [Header("Photo's")]
        public List<Sprite> DefaultPhotos;

        private int currentPhotoIndex;
        private string currentState;
        
        private GameManager GameManager => FindAnyObjectByType<GameManager>();

        private Player Player { get => GameManager.Save.Player; set => Player = value; }

        private void OnEnable()
        {
            GameManager.OnAuthCalled += UpdateView;
            SettingsController.OnChangeCalled += SetupChange;
            PhotoElement.OnPhotoSelected += ChoosePhoto;
        }

        private void OnDisable()
        {
            GameManager.OnAuthCalled -= UpdateView;
            SettingsController.OnChangeCalled -= SetupChange;
            PhotoElement.OnPhotoSelected -= ChoosePhoto;
        }

        public void UpdateView()
        {
            if (!Player.Registred)
            {
                SetupSingUp();
            }
            else
            {
                SetupLogIn();
            }
        }

        public void ChangePhoto()
        {
            authView.Move.Disable();
            photosView.Enable();
        }

        public void Continue()
        {
            if (currentState != "LogIn")
            {
                UpdateData();
            }
            OnAuthCompleted?.Invoke();
            authView.Move.Disable();
            photosView.Disable();
        }

        private void UpdateData()
        {
            Player.Photo = currentPhotoIndex;
            string name = authView.InputField.text;
            if (currentState == "SingUp")
            {
                Player.Name = name != "" ? name : $"Player_" + string.Format("{0:d5}", UnityEngine.Random.Range(0, 99999));
                DateTime now = DateTime.Now;
                Player.RegistrationDate = $"{now.Day}/{now.Month}/{now.Year}";
                Player.Registred = true;
            }
            else
            {
                Player.Name = name != "" ? name : Player.Name;
            }
        }

        private void ChoosePhoto(int index)
        {
            currentPhotoIndex = index;
            authView.Move.Enable();
            photosView.Disable();
            authView.Photo.sprite = DefaultPhotos[currentPhotoIndex];
        }

        private void SetupSingUp()
        {
            currentState = "SingUp";
            currentPhotoIndex = 0;

            authView.Photo.sprite = DefaultPhotos[0];
            authView.Button.gameObject.SetActive(true);
            authView.InputField.transform.parent.gameObject.SetActive(true);
            authView.Move.Enable();
        }

        private void SetupLogIn()
        {
            currentState = "LogIn";
            currentPhotoIndex = Player.Photo;
            string name = Player.Name;

            authView.Photo.sprite = DefaultPhotos[currentPhotoIndex];
            authView.Button.gameObject.SetActive(false);
            authView.Name.text = name;
            authView.InputField.transform.parent.gameObject.SetActive(false);
            authView.Move.Enable();
        }

        private void SetupChange()
        {
            currentState = "Change";
            currentPhotoIndex = Player.Photo;
            string name = Player.Name;

            authView.Photo.sprite = DefaultPhotos[Player.Photo];
            authView.Button.gameObject.SetActive(true);
            authView.Name.text = name;
            authView.InputField.transform.parent.gameObject.SetActive(true);
            authView.InputField.placeholder.GetComponent<Text>().text = name;
            authView.Move.Enable();
        }
    }
}
