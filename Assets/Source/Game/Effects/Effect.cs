using System;
using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public abstract class Effect
    {
        protected TowerEffectParams Tep;
        protected int Level;

        public bool IsOver { get; protected set; }

        public abstract void Launch(List<Enemy> enemies);
        
        public void SetStats(int level, TowerEffectParams tep)
        {
            Level = level;
            Tep = tep;
        }

        public static Effect CreateEffect(TowerEffectType type, TowerEffectParams tep, int level)
        {
            Effect effect = type switch
            {
                TowerEffectType.Splash => new EffectSplash(),
                TowerEffectType.Dot => new EffectDot(),
                TowerEffectType.Chain => new EffectChain(),
                TowerEffectType.Debuff => new EffectDebuff(),
                _ => null
            };
            effect?.SetStats(level, tep);
            return effect;
        }

        public abstract void Tick(float t);
    }
}