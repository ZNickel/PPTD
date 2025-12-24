using System.Collections.Generic;
using Source.Event;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.UI.Hud.PopUpNumbers
{
    public class PopUpNumber : MonoBehaviour
    {
        private static readonly int HashPlay = Animator.StringToHash("Play");
        private static readonly int HashType = Animator.StringToHash("Type");
        [SerializeField] private TMP_Text text;

        private Animator _animator;
        private Stack<PopUpNumber> _freeStack;
        private bool _isPlaying;
        private float _boost;

        public RectTransform Rt { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Rt = GetComponent<RectTransform>();
            UIEventBus.Instance.EventClickPowerChanged.AddListener(boost => _boost = boost);
        }

        public void Show(int value, Stack<PopUpNumber> freeStack, float speed)
        {
            if (_isPlaying) return;
            _freeStack = freeStack;
            text.text = value.ToString();
            _animator.speed = speed;
            _animator.SetTrigger(HashPlay);
            _animator.SetInteger(HashType, Random.Range(0, 6));
            _isPlaying = true;
            text.color = Color.Lerp(text.color, Color.red, _boost);
        }

        public void Handle_AnimationEnd()
        {
            _isPlaying = false;
            _freeStack.Push(this);
        }
    }
}