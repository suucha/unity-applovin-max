using SuuchaStudio.Unity.Core;
using Newtonsoft.Json;
using SuuchaStudio.Unity.Core.AdPlaying;

namespace SuuchaStudio.Unity.Applovin.Max
{
    public class MaxInterstitialVideoPlayer : SuuchaBase, IInterstitialVideoPlayer
    {
        public event InterstitialVideoDismissedHandler OnDismissed;
        public event InterstitialVideoLoadedHandler OnLoaded;
        public event InterstitialVideoLoadFailedHandler OnLoadFailed;
        public event InterstitialVideoShownHandler OnShown;
        public event InterstitialVideoShowFailedHandler OnShowFailed;
        public event InterstitialVideoClickedHandler OnClicked;
        public event InterstitialVideoRevenuePaidHandler OnRevenuePaid;
        public event InterstitialVideoCompletedHandler OnCompleted;

        public MaxInterstitialVideoPlayer()
        {
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnAdClickedEventInternal;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnAdDisplayedEventInternal;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdDisplayFailedEventInternal;
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoadedEventInternal;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailedEventInternal;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEventInternal;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHiddenEventInternal;
        }

        private void OnAdHiddenEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Interstitial video hidden, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnDismissed?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }

        private void OnAdRevenuePaidEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Interstitial video revenue paid, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnRevenuePaid?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
            OnCompleted?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }

        private void OnAdLoadFailedEventInternal(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            var errorMessage = $"Interstitial video load failed. ErrorCode:{errorInfo.Code},Message:{errorInfo.Message}, FailureInfo:{errorInfo.AdLoadFailureInfo}";
            Logger.LogError(errorMessage);
            OnLoadFailed?.Invoke(adUnitId, errorMessage);
        }

        private void OnAdLoadedEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Interstitial video loaded, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnLoaded?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }

        private void OnAdDisplayFailedEventInternal(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            var errorMessage = $"Interstitial video display failed. ErrorCode:{errorInfo.Code},Message:{errorInfo.Message}, FailureInfo:{errorInfo.AdLoadFailureInfo},Placement:{adInfo.Placement}";
            Logger.LogError(errorMessage);
            OnShowFailed?.Invoke(adUnitId, errorMessage, adInfo.Placement);
        }

        private void OnAdDisplayedEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Interstitial video displayed, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnShown?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }

        private void OnAdClickedEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Interstitial video clicked, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnClicked?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }

        public bool HasInterstitialVideo(string adUnitId)
        {
            return MaxSdk.IsInterstitialReady(adUnitId);
        }

        public void RequestInterstitialVideo(string adUnitId)
        {
            MaxSdk.LoadInterstitial(adUnitId);
        }

        public void ShowInterstitialVideo(string adUnitId, string placement)
        {
            MaxSdk.ShowInterstitial(adUnitId, placement);
        }
    }
}
