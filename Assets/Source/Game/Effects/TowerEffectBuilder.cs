using System.Collections.Generic;
using Source.Game.Controllers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class TowerEffectBuilder : MonoBehaviour
    {
        private List<Effect> _activeEffects;
        private WaveController _waveController;

        public void Setup(WaveController waveController)
        {
            _waveController = waveController;
            _activeEffects = new List<Effect>();
        }

        public void Launch(Effect effect, List<Enemy> enemies)
        {
            if (effect == null) return;
            effect.Launch(enemies);
            if (!effect.IsOver) _activeEffects.Add(effect);
        }

        private void FixedUpdate()
        {
            if (_activeEffects.Count == 0) return;
            foreach (var effect in _activeEffects) effect.Tick(Time.fixedDeltaTime);
            _activeEffects.RemoveAll(effect => effect.IsOver);
        }
    }
}