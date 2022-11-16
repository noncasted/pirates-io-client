﻿using System;
using GamePlay.Items.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.Services.Common.InventoryGrids
{
    [DisallowMultipleComponent]
    public class GridCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        private int _id;
        private IItem _item;

        public int Id => _id;

        public IItem Item => _item;

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Selected?.Invoke(this);
        }

        public event Action<GridCell> Selected;

        public void Setup(int id)
        {
            _id = id;
        }

        public void AssignItem(IItem item)
        {
            gameObject.SetActive(true);

            _icon.sprite = item.BaseData.Icon;
            _count.text = item.Count.ToString();

            _item = item;
        }

        public void Disable()
        {
            _item = null;
            gameObject.SetActive(false);
        }
    }
}