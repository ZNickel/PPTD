using System;
using Source.Event;
using TMPro;
using UnityEngine;

namespace Source.UI.Hud
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text coins;
        [SerializeField] private TMP_Text energy;
        [SerializeField] private TMP_Text health;
        
        private void Awake()
        {
            UIEventBus.Instance.EventCoinCountChanged.AddListener(UpdCoins);
            UIEventBus.Instance.EventPowerCountChanged.AddListener(UpdEnergy);
            UIEventBus.Instance.EventHealthCountChanged.AddListener(UpdHealth);
        }

        private void UpdCoins(int v) => coins.text = $"{v}";
        private void UpdEnergy(int v) => energy.text = $"{v}";
        private void UpdHealth(int v) => health.text = $"{v}";
    }
}