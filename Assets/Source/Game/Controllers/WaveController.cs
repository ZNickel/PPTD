using System;
using System.Collections.Generic;
using System.Linq;
using Source.Data;
using Source.Data.Way;
using Source.Event;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Controllers
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private GameObject enemyContainer;
        [SerializeField] private GameObject enemy;

        private GameController _gc;
        
        private WaveData[] _waves;
        private int _currentWaveIndex;
        
        private Queue<(int, EnemyData)> _queue;
        private WayPackData _ways;

        private float _elapsedDelay = 0f;
        private bool _spawn;
        private float _timer;
        
        private List<Enemy>[] _alive;
        
        public EnemySelector Selector {get; private set;}

        public float WaveDelay => _waves[_currentWaveIndex].InitialDelay;
        
        private void Awake() => _gc = GetComponent<GameController>();

        public void Setup(WaveData[] waves, WayPackData ways)
        {
            _alive = new List<Enemy>[ways.Count];
            Selector = new EnemySelector(_alive);
            for (var i = 0; i < ways.Count; i++) _alive[i] = new List<Enemy>();
            _waves = waves;
            _ways = ways;
            _currentWaveIndex = 0;
            Launch(_waves[_currentWaveIndex]);
        }
        
        private void Launch(WaveData wave)
        {
            _queue = wave.Queue;
            _spawn = true;
            _timer = 0;
            _elapsedDelay = 0;
            UIEventBus.Instance.Trigger_ShowWaveIndicator(this, _ways.GetIndicatorPosition());
        }

        private bool _endTriggered;

        public void ForcedLaunch() => _elapsedDelay = float.MaxValue / 2f;

        private void Update()
        {
            if (_endTriggered) return;
            if (_currentWaveIndex >= _waves.Length)
            {
                UIEventBus.Instance.Trigger_EventGameOver(_gc.CResource.Health > 0);
                _endTriggered = true;
                return;
            }
            
            if (_elapsedDelay < WaveDelay)
            {
                _elapsedDelay += Time.deltaTime;
                return;
            }
            
            if (_spawn)
            {
                _timer += Time.deltaTime;
                if (_timer < _waves[_currentWaveIndex].Interval) return;
                _timer = 0;
                if (_queue.TryDequeue(out var d)) 
                    SpawnEnemy(d.Item1, d.Item2);
                else 
                    _spawn = false;
                return;
            }

            if (!HasEnemies())
            {
                Debug.Log($"Wave #{_currentWaveIndex + 1} End");
                _currentWaveIndex++;
                if (_currentWaveIndex >= _waves.Length) return;
                Debug.Log($"Wave #{_currentWaveIndex + 1} Start");
                _gc.CResource.ChangeCoin(_waves[_currentWaveIndex].CompleteBounty);
                Launch(_waves[_currentWaveIndex]);
            }
        }

        private void SpawnEnemy(int index, EnemyData ed)
        {
            var e = Instantiate(enemy, enemyContainer.transform);
            var eScript = e.GetComponent<Enemy>();
            eScript.Setup(ed, _ways[index], () => DieAction(eScript), () => DoneAction(eScript));
            _alive[index].Add(eScript);
        }

        private void DieAction(Enemy ed)
        {
            _gc.CResource.ChangeCoin(ed.Data.Bounty);
            _gc.CResource.ChangePower(ed.Data.EnergyBounty);
            foreach (var l in _alive)
            {
                if (l.Contains(ed))
                {
                    l.Remove(ed);
                }
            }
        }

        private void DoneAction(Enemy ed)
        {
            _gc.CResource.ChangeHealth((int)-ed.Data.Damage);
            foreach (var l in _alive)
            {
                if (l.Contains(ed))
                {
                    l.Remove(ed);
                }
            }
        }

        private bool HasEnemies() => _alive.Any(list => list.Count > 0);
    }
}