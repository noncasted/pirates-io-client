﻿using GamePlay.Cities.Instance.Storage.Runtime;
using GamePlay.Cities.Instance.Trading.Ports.Root.Runtime;
using GamePlay.Cities.Instance.Trading.Ports.UI.Runtime.Origin.Events;
using GamePlay.Cities.Instance.Trading.Ports.UI.Runtime.Trade;
using GamePlay.Cities.Instance.Trading.Ports.UI.Runtime.Trade.Events;
using GamePlay.Items.Abstract;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Cities.Instance.Trading.Ports.UI.Runtime.Origin
{
    public class AvailableItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Button _transferButton;

        private IItem _item;
        private ItemOrigin _origin;
        private bool _isActive;
        private int _price;

        private IPriceProvider _priceProvider;

        public bool IsActive => _isActive;

        private void OnEnable()
        {
            _transferButton.onClick.AddListener(OnTransferClicked);
        }

        private void OnDisable()
        {
            _transferButton.onClick.RemoveListener(OnTransferClicked);

            if (_item == null)
                return;

            _priceProvider.Unfreeze(_item.BaseData.Type);
            _item.CountChanged -= OnCountChanged;

            _item = null;
        }

        public void AssignItem(IItem item, ItemOrigin origin, IPriceProvider priceProvider)
        {
            _priceProvider = priceProvider;
            _origin = origin;
            gameObject.SetActive(true);
            _transferButton.gameObject.SetActive(true);

            _item = item;

            _icon.sprite = item.BaseData.Icon;
            _count.text = item.Count.ToString();
            _cost.text = _priceProvider.GetPrice(item.BaseData.Type).ToString();

            _isActive = false;

            OnCountChanged(_item.Count);

            _item.CountChanged += OnCountChanged;
        }

        public void Disable()
        {
            _item = null;
            gameObject.SetActive(false);

            _isActive = false;
        }

        public void OnTransferCanceled()
        {
            _priceProvider.Unfreeze(_item.BaseData.Type);

            _isActive = false;

            _transferButton.gameObject.SetActive(true);
        }

        public void UpdatePrice()
        {
            if (_item == null)
                return;

            if (_priceProvider == null)
                return;

            if (_isActive == true)
                return;

            _price = _priceProvider.GetPrice(_item.BaseData.Type);
            _cost.text = _price.ToString();
        }

        private void OnTransferClicked()
        {
            _priceProvider.Freeze(_item.BaseData.Type);

            _isActive = true;
            _transferButton.gameObject.SetActive(false);

            var tradable = new TradableItem(_item, _price);

            var data = new TransferRequestedEvent(tradable, _origin);

            MessageBroker.Default.Publish(new TradeRequestedEvent());
            MessageBroker.Default.Publish(data);
        }

        private void OnCountChanged(int count)
        {
            _count.text = count.ToString();
        }
    }
}