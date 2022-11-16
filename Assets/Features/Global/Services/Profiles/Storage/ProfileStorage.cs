﻿using Global.Services.Profiles.Logs;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Global.Services.Profiles.Storage
{
    [DisallowMultipleComponent]
    public class ProfileStorage : MonoBehaviour, IProfileStoragePresenter, IProfileStorageProvider
    {
        [Inject]
        private void Construct(ProfileLogger logger)
        {
            _logger = logger;
        }

        private ProfileLogger _logger;

        [SerializeField] [ReadOnly] private string _userName = string.Empty;

        public string UserName => _userName;

        public void SetUserName(string userName)
        {
            _userName = userName;

            _logger.OnUserNameSet(userName);
        }
    }
}