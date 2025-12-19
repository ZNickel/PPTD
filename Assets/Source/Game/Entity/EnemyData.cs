using UnityEngine;

namespace Source.Game.Entity
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Entities/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Setup")]
        [SerializeField] private Sprite sprite;
        
        [Header("Parameters")]
        [SerializeField] private float speed;
        [SerializeField] private float hp;
        [SerializeField] private float damage;

        [Header("Economy")] 
        [SerializeField] private int bounty;
        [SerializeField] private int energyBounty = 5;
        
        public Sprite Sprite => sprite;
        public float Speed => speed;
        public float Hp => hp;
        public float Damage => damage;
        public int Bounty => bounty;
        public int EnergyBounty => energyBounty;
    }
}