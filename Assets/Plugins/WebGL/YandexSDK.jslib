mergeInto(LibraryManager.library, {
  InitYSDK: function () {
    YaGames.init().then(function (ysdk) {
      window.ysdk = ysdk;

      ysdk.features.LoadingAPI.ready();

      unityInstance.SendMessage(
        'YandexSDK',
        'OnSDKReady'
      );
    });
  },

  ShowInterstitial: function () {
    if (!window.ysdk) return;

    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function () {
          unityInstance.SendMessage(
            'YandexSDK',
            'OnInterstitialClosed'
          );
        },
        onError: function () {
          unityInstance.SendMessage(
            'YandexSDK',
            'OnInterstitialClosed'
          );
        }
      }
    });
  },

  ShowRewarded: function () {
    if (!window.ysdk) return;

    ysdk.adv.showRewardedVideo({
      callbacks: {
        onRewarded: function () {
          unityInstance.SendMessage(
            'YandexSDK',
            'OnRewarded'
          );
        },
        onClose: function () {
          unityInstance.SendMessage(
            'YandexSDK',
            'OnRewardedClosed'
          );
        }
      }
    });
  }
});
