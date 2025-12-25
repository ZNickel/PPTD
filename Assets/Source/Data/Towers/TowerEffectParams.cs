using UnityEngine;

namespace Source.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerEffect", menuName = "Entities/Tower Effect")]
    public class TowerEffectParams : ScriptableObject
    {
        [Header("Main"), SerializeField] private string effectName;
        [SerializeField] private TowerEffectType type;
        [SerializeField] private TargetSelectBehaviour targetSelectBehaviour;

        [Header("Show in UI")]
        [SerializeField] private bool showRadius = true;
        [SerializeField] private bool showCount = true;
        [SerializeField] private bool showTime = true;

        [Header("Effect TTP"), SerializeField, Range(.1f, 50f)]
        private float damage = 30f;

        [SerializeField, Range(.1f, 10f)] private float radius = 2f;
        [SerializeField, Range(1, 50)] private int count = 3;
        [SerializeField, Range(.001f, 10f)] private float time = 1f;
        [SerializeField, Range(1f, 0f)] private float maxFalloff = 1f;
        [SerializeField, Range(0f, 1f)] private float minFalloff = .25f;
        [SerializeField, Range(0f, 1f)] private float interval = .3333f;

        [Header("Effect Upgrade"), SerializeField, Range(1f, 2f)]
        private float lvlDamage = 1.1f;

        [SerializeField, Range(1f, 2f)] private float lvlRadius = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlCount = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlTime = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlMaxFall = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlMinFall = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlInterval = .9f;

        public TargetSelectBehaviour SelectBehaviour => targetSelectBehaviour;
        
        public bool ShowRadius => showRadius;
        public bool ShowCount => showCount;
        public bool ShowTime => showTime;
        public string EffectName => effectName;
        public TowerEffectType EffectType => type;

        public float GetDamage(int level) => CalcLvl(damage, lvlDamage, level);
        public float GetRadius(int level) => CalcLvl(radius, lvlRadius, level);
        public float GetCount(int level) => CalcLvl(count, lvlCount, level);
        public float GetTime(int level) => CalcLvl(time, lvlTime, level);
        public float GetMaxFall(int level) => CalcLvl(maxFalloff, lvlMaxFall, level);
        public float GetMinFall(int level) => CalcLvl(minFalloff, lvlMinFall, level);
        public float GetInterval(int level) => CalcLvl(interval, lvlInterval, level);

        //n0 (* fac)n
        private static float CalcLvl(float baseV, float f, int lvl)
        {
            if (lvl == 0) return baseV;
            var x = baseV;
            for (var i = 1; i < lvl; i++) x *= f;
            return x;
        }
    }
}