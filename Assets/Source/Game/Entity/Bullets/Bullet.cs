using UnityEngine;

namespace Source.Game.Entity.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public float Damage { get; private set; }

        private SpriteRenderer _sr;
        private float _speed;
        private Vector3 _dir;
        private Vector3 _start;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void Setup(Sprite sprite, float dmg, float speed, Vector3 start, Vector3 direction)
        {
            _sr.sprite = sprite;
            transform.position = start;
            transform.up = direction.normalized;
            Damage = dmg;
            _speed = speed;
            _dir = direction.normalized;
        }

        private void FixedUpdate()
        {
            var d = _dir * (_speed * Time.fixedDeltaTime);
            transform.position += d;
            
            if ((transform.position - _start).magnitude < 3f) Destroy(gameObject);
        }
    }
}