using System.Collections;
using Source.Data.Way;
using Source.Event;
using Source.Game.Entity.Bullets;
using UnityEngine;
using UnityEngine.Events;

namespace Source.Game.Entity
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        private UnityAction _dieAction;
        private UnityAction _doneAction;
        
        private float _currentHp;
        private WayData _wayData;
        private int _wpIndex;
        private SpriteRenderer _sr;

        public EnemyData Data => data;

        private float _debuff;
        private bool _skillDebuff;
        
        public void AddDebuff(float v, bool skillDebuff = false, float t = 0f)
        {
            if (_skillDebuff) return;
            if (skillDebuff)
            {
                _skillDebuff = true;
                StartCoroutine(AutoUnDebuff(t));
            }
            _debuff = v;
            _sr.color = Color.blue;;
        }
        
        public void CancelDebuff(bool skillDebuff = false)
        {
            if (skillDebuff && _skillDebuff) _skillDebuff = false;
            _debuff = 1f;
            _sr.color = Color.white;
        }
        
        public void Setup(EnemyData d, WayData wayData, UnityAction dieAction, UnityAction doneAction)
        {
            _dieAction = dieAction;
            _doneAction = doneAction;
            data = d;
            _currentHp = data.Hp;
            _wayData = wayData;
            _wpIndex = 0;
            done = false;
            transform.position = wayData[_wpIndex];
            _sr = GetComponent<SpriteRenderer>();
            _sr.sprite = d.Sprite;
            _debuff = 1f;
        }

        private void Update()
        {
            Move();
        }

        private bool done;
        
        private void Move()
        {
            if (done) return;
            if (_wpIndex >= _wayData.PointCount)
            {
                _doneAction.Invoke();
                done = true;
                Destroy(gameObject);
                return;
            }

            var target = _wayData[_wpIndex];

            var d = data.Speed * Time.deltaTime * _debuff;
            var next = Vector2.MoveTowards(transform.position, target, d);

            var z = -.5f + d * .1f;
            
            transform.position = new Vector3(next.x, next.y, z);
            if (Vector2.Distance(transform.position, target) < 0.05f)
                _wpIndex++;
        }

        public void Hit(float damage)
        {
            if (_currentHp < 0f) return;
            
            var old = _currentHp;

            damage *= _skillDebuff ? 1f : 1f / _debuff;
            
            _currentHp -= damage;
            UIEventBus.Instance.Trigger_ShowPopupNumber(Mathf.RoundToInt(damage), this);

            if (Mathf.Approximately(old, data.Hp) && _currentHp < data.Hp)
                UIEventBus.Instance.Trigger_AttachHpBar(this);
            UIEventBus.Instance.Trigger_UpdateEnemyHp(this, 1f - _currentHp / data.Hp);
            
            if (_currentHp > 0) return;
            
            UIEventBus.Instance.Trigger_DetachHpBar(this);
            _dieAction?.Invoke();
            if (gameObject)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var o = other.gameObject;
            if (!o.CompareTag("Bullet")) return;
            var b = o.GetComponent<UniBullet>();
            Destroy(o);
            Hit(b.Damage);
        }

        private IEnumerator AutoUnDebuff(float t)
        {
            yield return new WaitForSeconds(t);
            CancelDebuff();
        }
    }
}