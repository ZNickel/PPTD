using UnityEngine;
using System.Runtime.InteropServices;

namespace Source
{
    public class YandexSDK : MonoBehaviour
    {
        public static YandexSDK Instance;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void InitYSDK();
    [DllImport("__Internal")] private static extern void ShowInterstitial();
    [DllImport("__Internal")] private static extern void ShowRewarded();
#endif

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        InitYSDK();
#endif
        }

        // ===== CALLBACKS =====
        public void OnSDKReady()
        {
            Debug.Log("Yandex SDK Ready");
        }

        public void OnInterstitialClosed()
        {
            Time.timeScale = 1f;
        }

        public void OnRewarded()
        {
            Debug.Log("Reward granted");
        }

        public void OnRewardedClosed()
        {
            Time.timeScale = 1f;
        }

        // ===== PUBLIC API =====
        public void ShowAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Time.timeScale = 0f;
        ShowInterstitial();
#endif
        }

        public void ShowRewardAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Time.timeScale = 0f;
        ShowRewarded();
#endif
        }
    }
}