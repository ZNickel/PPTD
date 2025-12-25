using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.UI.Map
{
    public class UIMapController : MonoBehaviour
    {
        [SerializeField] private LevelFrame levelFrame;
        [SerializeField] private UIMapMarker uiMarkerPrefab;
        [SerializeField] private GameObject markerContainer;
        [SerializeField] private MapController mapController;
        
        private void Start()
        {
            var data = LoadAllLevels();
            var positions = mapController.GetPositions();
            var lim = Math.Max(data.Count, positions.Count);
            for (var i = 0; i < lim; i++) 
                CreateMarker(i >= data.Count ? null : data[i], this, positions[i]);
        }

        public void Select(LevelData data)
        {
            if (data == null) return;
            levelFrame.gameObject.SetActive(true);
            levelFrame.Open(data);
        }

        private void CreateMarker(LevelData data, UIMapController controller, Vector2 pos)
        {
            Instantiate(uiMarkerPrefab, markerContainer.transform)
                .GetComponent<UIMapMarker>()
                .Setup(data, controller, pos);
        }

        private static List<LevelData> LoadAllLevels()
        {
            var levels = Resources.LoadAll<LevelData>("Levels").ToList();
            levels.Sort((a, b) => a.Index.CompareTo(b.Index));
            return levels;
        }
        
        public void LoadMenu() => SceneManager.LoadScene(0);
    }
}