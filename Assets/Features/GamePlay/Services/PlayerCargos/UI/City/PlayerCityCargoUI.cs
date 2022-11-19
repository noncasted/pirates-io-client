﻿using System;
using GamePlay.Items.Abstract;
using GamePlay.Services.Common.InventoryGrids;
using UnityEngine;

namespace GamePlay.Services.PlayerCargos.UI.City
{
    public class PlayerCityCargoUI : MonoBehaviour, IPlayerCityCargoUI
    {
        [SerializeField] private GameObject _body;
        [SerializeField] private ItemsListView _grid;

        private void Awake()
        {
            _body.SetActive(false);
        }

        public bool IsActive => _body.activeSelf;

        public Action<IItem[]> Open(IItem[] items)
        {
            _body.SetActive(true);
            _grid.Fill(items);

            return Redraw;
        }

        public void Close()
        {
            _body.SetActive(false);
        }

        private void Redraw(IItem[] items)
        {
            _grid.Fill(items);
        }
    }
}