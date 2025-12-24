using Source.Event;
using Source.Game.Controllers;
using TMPro;
using UnityEngine;

namespace Source.UI.Hud
{
    public class ResourceDisplay : MonoBehaviour
    {
        private static readonly int HashFilled = Animator.StringToHash("Filled");
        [SerializeField] private TMP_Text coins;
        [SerializeField] private TMP_Text energy;
        [SerializeField] private GameObject healthContainer;
        [SerializeField] private GameObject healthPrefab;
        
        private Animator[] _healthAnimators;
        
        private void Awake()
        {
            UIEventBus.Instance.EventCoinCountChanged.AddListener(UpdCoins);
            UIEventBus.Instance.EventPowerCountChanged.AddListener(UpdEnergy);
            UIEventBus.Instance.EventHealthCountChanged.AddListener(UpdHealth);
        }

        private void Start()
        {
            var gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            CreateHealths(gc.LevelData.StartHealth);
        }

        private void UpdCoins(int v) => coins.text = $"{v}";

        private void UpdEnergy(int v) => energy.text = $"{v}";

        private void UpdHealth(int v)
        {
            for (var i = 0; i < _healthAnimators.Length; i++) 
                _healthAnimators[i].SetBool(HashFilled, i < v);
        }

        private void CreateHealths(int n)
        {
            _healthAnimators = new Animator[n];
            for (var i = 0; i < n; i++) 
                _healthAnimators[i] = Instantiate(healthPrefab, healthContainer.transform).GetComponent<Animator>();
        }
    }
}