using System.Collections.Generic;
using Source.Game.Effects;
using Source.Game.Entity.Bullets;
using UnityEngine;

namespace Source.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Entities/Tower Data")]
    public class TowerData : ScriptableObject
    {
        [Header("Struct")] 
        [SerializeField] private Sprite[] headSprites;
        
        #region Main
        
        [Header("Main")] 
        [SerializeField] private string tName;
        [SerializeField] private int levelCount;
        [SerializeField] private UniBullet bullet;
        [SerializeField] private TowerSkillParams skill;
        [SerializeField] private TargetSelectBehaviour targetSelectBehaviour = TargetSelectBehaviour.Nearest;
        [SerializeField] private bool cascadeEffects;
        [SerializeField] private List<TowerEffectParams> effects = new();
        
        public TowerSkillParams Skill => skill;
        public string Tname => tName;
        public int LevelCount => levelCount;
        public Sprite[] HeadSprites => headSprites;
        public UniBullet BulletPrefab => bullet;
        public TargetSelectBehaviour TargetSelectBehaviour => targetSelectBehaviour;
        public bool CascadeEffects => cascadeEffects;
        public List<TowerEffectParams> Effects => effects;
        
        #endregion

        #region Economy

        [Header("Economy")] [SerializeField] private int basePrice = 50;
        [SerializeField] private int baseLvlUpPrice = 25;
        [SerializeField] private float lvlUpPriceFactor = 1.5f;
        [SerializeField] private float sellPriceFactor = .75f;

        public int Price => basePrice;
        
        public int LvlUpPrice(int lvl)
        {
            if (lvl == 0) return 0;
            var p = baseLvlUpPrice;
            for (var i = 1; i <= lvl; i++)
                p = Mathf.FloorToInt(p * lvlUpPriceFactor);
            return p;
        }

        public int SellPrice(int lvl)
        {
            var totalPrice = basePrice;
            for (var i = 0; i < lvl; i++) totalPrice += LvlUpPrice(i);
            return Mathf.FloorToInt(totalPrice * sellPriceFactor);
        }

        #endregion

        #region TTP

        [Header("TTP")] [SerializeField] private float range;
        [SerializeField] private float atkSpeed;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float hp;
        [SerializeField] private float damage;

        [Header("LvlUp")] 
        [SerializeField] private float lvlFacRange = 1.2f;
        [SerializeField] private float lvlFacAtkSpeed = 1.2f;
        [SerializeField] private float lvlFacProjSpeed = 1.2f;
        [SerializeField] private float lvlFacHp = 1.2f;
        [SerializeField] private float lvlFacDamage = 1.2f;

        [Header("Boost")] 
        [SerializeField] private float boostRange = 1f;
        [SerializeField] private float boostAtkSpeed = 1f;
        [SerializeField] private float boostFacProjSpeed = 1f;
        [SerializeField] private float boostHp = 1f;
        [SerializeField] private float boostDamage = 1f;
        
        public float BoostRange => boostRange;
        public float BoostAtkSpeed => boostAtkSpeed;
        public float BoostFacProjSpeed => boostFacProjSpeed;
        public float BoostHp => boostHp;
        public float BoostDamage => boostDamage;
        
        #endregion

        private TowerStats[] _stats;

        public TowerStats GetStats(int level)
        {
            _stats ??= new TowerStats[levelCount];

            _stats[level] ??= new TowerStats(
                CalcValue(range, lvlFacRange, level),
                CalcValue(atkSpeed, lvlFacAtkSpeed, level),
                CalcValue(projectileSpeed, lvlFacProjSpeed, level),
                CalcValue(hp, lvlFacHp, level),
                CalcValue(damage, lvlFacDamage, level)
            );

            return _stats[level];
        }

        //n0 (* fac)n
        private static float CalcValue(float n0, float fac, int n)
        {
            if (n == 0) return n0;
            var x = n0;
            for (var i = 1; i < n; i++) x *= fac;
            return x;
        }
    }
}