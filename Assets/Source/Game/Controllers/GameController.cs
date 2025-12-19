using System.Linq;
using Source.Event;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelGrid levelGrid;
        [SerializeField] GameObject endScreen;

        public ResourceController CResource { get; private set; }
        public TowerController CTowers { get; private set; }
        public ClickController CClick { get; private set; }
        public WaveController CWave { get; private set; }
        
        private void Awake()
        {
            CResource = GetComponent<ResourceController>();
            CTowers = GetComponent<TowerController>();
            CClick = GetComponent<ClickController>();
            CWave = GetComponent<WaveController>();
            UIEventBus.Instance.EventGameOver.AddListener(End);
        }

        private void End(bool win)
        {
            endScreen.SetActive(true);
            var text = endScreen.GetComponentInChildren<TMP_Text>();
            text.text = win ? "Win" : "Fail";
        }

        private void Start()
        {
            endScreen.SetActive(false);
            var ld = levelGrid.CurrentLevelData;
            
            CResource.Setup(ld.StartCoins, ld.StartPower, ld.StartHealth);
            CTowers.Setup(ld.Types, ld.Width, ld.Height);

            CWave.Setup(ld.Waves, ld.Ways);
        }
    }
}