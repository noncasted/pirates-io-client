﻿using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Services.UI.Runtime
{
    [DisallowMultipleComponent]
    public class MenuUI : MonoBehaviour, IMenuUI
    {
        [SerializeField] private GameObject _loginBody;
        [SerializeField] private GameObject _loadingBody;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_Text _connectionErrorText;
        [SerializeField] private Button _playButton;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
        }

        public void OnLogin()
        {
            _loginBody.SetActive(true);
            _loadingBody.SetActive(false);
        }

        public void OnLoginWithError(string error)
        {
            _loginBody.SetActive(true);
            _loadingBody.SetActive(false);

            _connectionErrorText.text = error;
        }

        public void OnLoading()
        {
            _loginBody.SetActive(false);
            _loadingBody.SetActive(true);
        }

        private void OnPlayClicked()
        {
            var userName = _nameInput.text;

            if (string.IsNullOrWhiteSpace(userName) == true)
                return;

            var clicked = new PlayClickedEvent(userName);
            MessageBroker.Default.Publish(clicked);
        }
    }
}