﻿using System;
using Common.EditableScriptableObjects.Attributes;
using GamePlay.Cities.Instance.Root.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Factions.Selections.UI.Runtime
{
    [DisallowMultipleComponent]
    public class FactionEntry : MonoBehaviour
    {
        [SerializeField] [EditableObject] private CityDefinition _definition;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _cityName;
        
        public event Action<CityDefinition> Selected;

        private void OnEnable()
        {
            Debug.Log("Enable");
            _button.onClick.AddListener(OnClicked);
            _cityName.text = _definition.Name;
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Debug.Log("Clicked");
            Selected?.Invoke(_definition);
        }
    }
}