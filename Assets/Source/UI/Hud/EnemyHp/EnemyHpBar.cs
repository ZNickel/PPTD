using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Source.UI.Hud.EnemyHp
{
    public class EnemyHpBar : MonoBehaviour
    {
        private const float ShiftTime = 0.1f;
        private const float ShowHideTime = 0.2f;
        
        private static readonly int HashShake = Animator.StringToHash("Shake");
        [SerializeField] private RectTransform filler;

        private RectTransform _rt;
        private CanvasGroup _cg;
        private Animator _animator;
        private float _fillerW;
        private int _id = -1;
        
        public int Id
        {
            get => _id;
            set => _id = _id != -1 ? _id : value;
        }

        public RectTransform Rt => _rt;
        
        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _cg = GetComponent<CanvasGroup>();
            _cg.alpha = 0f;
            _animator = GetComponent<Animator>();
            _fillerW = filler.sizeDelta.x;
        }

        public void Show() => StartCoroutine(ShowHide(_cg, null, 0f, 1f));

        public void Hide(UnityAction endAction) => StartCoroutine(ShowHide(_cg, endAction, 1f, 0f));

        public void UpdateHp(float hpNormalized)
        {
            _animator.SetTrigger(HashShake);

            var start = filler.anchoredPosition.x;
            var end = -_fillerW * hpNormalized;
            StartCoroutine(SmoothShift(start, end));
        }

        private IEnumerator SmoothShift(float start, float end)
        {
            var elapsed = 0f;
            var y = filler.anchoredPosition.y;

            while (elapsed < ShiftTime)
            {
                elapsed += Time.deltaTime;
                var x = Mathf.SmoothStep(start, end, elapsed / ShiftTime);
                filler.anchoredPosition = new Vector2(x, y);
                yield return null;
            }
            filler.anchoredPosition = new Vector2(end, y);
        }

        private static IEnumerator ShowHide(CanvasGroup cg, UnityAction endAction, float from, float to)
        {
            var elapsed = 0f;

            while (elapsed < ShowHideTime)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.SmoothStep(from, to, elapsed / ShowHideTime);
                yield return null;
            }
            cg.alpha = to;
            endAction?.Invoke();
        }

        public override int GetHashCode() => Id;

        public override bool Equals(object other) => other is EnemyHpBar obj && Id == obj.Id;
    }
}