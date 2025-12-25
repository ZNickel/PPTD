using System.Linq;
using Source.Event;
using Source.UI;
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

        public LevelData LevelData => levelGrid.CurrentLevelData;
        
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
            endScreen.GetComponent<EndScreen>()?.Show(win);
        }

        private void Start()
        {
            var ld = levelGrid.CurrentLevelData;
            CResource.Setup(ld.StartCoins, ld.StartPower, ld.StartHealth);
            CTowers.Setup(ld.Types, ld.Width, ld.Height);
            CWave.Setup(ld.Waves, ld.Ways);
        }
    }
}