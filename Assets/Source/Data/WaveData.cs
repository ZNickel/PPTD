using System;
using System.Collections.Generic;
using System.Linq;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public class WaveData
    {
        [Serializable]
        private class EnemyOnWay
        {
            public int wayIndex;
            public EnemyData enemy;
        }
        
        [SerializeField] private List<EnemyOnWay> enemies;
        [SerializeField] private float initialDelay = 15f;
        [SerializeField] private float interval = 1f;
        [SerializeField] private float bountyFac = 1f;
        [SerializeField] private int completeBounty = 200;

        public Queue<(int, EnemyData)> Queue => 
            new(enemies.Select(w => (w.wayIndex, w.enemy)));

        public float InitialDelay => initialDelay;
        public float Interval => interval;
        public float BountyFactor => bountyFac;
        public int CompleteBounty => completeBounty;
    }
}