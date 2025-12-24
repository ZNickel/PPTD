using System;
using System.Collections;
using UnityEngine;

namespace Source.Game.Entity.Bullets
{
    public class TowerRay : UniBullet
    {
        [SerializeField] private float showTime = .1f;
        [SerializeField] private float hideTime = .2f;
        [SerializeField] private GameObject sd;
        [SerializeField] private GameObject md;
        [SerializeField] private GameObject ed;
        [SerializeField] private GameObject su;
        [SerializeField] private GameObject mu;
        [SerializeField] private GameObject eu;

        private (SpriteRenderer sr, Transform t)[] _comps;

        private void Awake()
        {
            _comps = new[] { ExtCom(su), ExtCom(sd), ExtCom(mu), ExtCom(md), ExtCom(eu), ExtCom(ed) };
        }

        private static (SpriteRenderer, Transform) ExtCom(GameObject o) =>
            (o.GetComponent<SpriteRenderer>(), o.transform);

        protected override void Launch()
        {
            Build();
            StartCoroutine(ShowHide());
        }

        private IEnumerator ShowHide()
        {
            var elapsed = 0f;

            while (elapsed < showTime)
            {
                elapsed += Time.deltaTime;
                SetAlpha(elapsed / showTime);
                yield return null;
            }

            SetAlpha(1f);

            Target.Hit(Damage);

            elapsed = 0f;
            while (elapsed < hideTime)
            {
                elapsed += Time.deltaTime;
                SetAlpha(elapsed / hideTime);
                yield return null;
            }

            SetAlpha(0f);

            Destroy(gameObject);
        }

        private void SetAlpha(float f)
        {
            var color = new Color(1f, 1f, 1f, f);
            foreach (var comp in _comps) comp.sr.color  = color;
        }

        private void Build()
        {
            var dir = To - From;
            var distance = Vector3.Distance(To, From) * 5;
            var halfDistance = distance / 2f + .5f;
            var center = From + dir / 2f;
            
            transform.position = center;
            transform.right = dir.normalized;
            
            _comps[0].t.localPosition = new Vector3(-halfDistance, 0, 0);
            _comps[1].t.localPosition = new Vector3(-halfDistance, 0, 0);
            
            _comps[2].t.localScale = new Vector3(distance, 1f, 1f);
            _comps[3].t.localScale = new Vector3(distance, 1f, 1f);
            
            _comps[4].t.localPosition = new Vector3(halfDistance, 0, 0);
            _comps[5].t.localPosition = new Vector3(halfDistance, 0, 0);
        }
    }
}