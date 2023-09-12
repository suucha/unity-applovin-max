using SuuchaStudio.Unity.Core;
using Newtonsoft.Json;
using static MaxSdkBase;
using SuuchaStudio.Unity.Core.AdPlaying;

namespace Suucha.Unity.Applovin.Max
{
    public class MaxRewardedVideoPlayer : SuuchaBase, IRewardedVideoPlayer
    {
        public MaxRewardedVideoPlayer()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        }

        public event RewardedVideoClickedHandler OnClicked;
        public event RewardedVideoClosedHandler OnClosed;
        public event RewardedVideoShownHandler OnShown;
        public event RewardedVideoShowFailedHandler OnShowFailed;
        public event RewardedVideoLoadedHandler OnLoaded;
        public event RewardedVideoLoadFailedHandler OnLoadFailed;
        public event RewardedVideoRevenuePaidHandler OnRevenuePaid;
        public event RewardedVideoCompletedHandler OnCompleted;

        public void OnRewardedAdLoadedEvent(string adUnitId, AdInfo adInfo)
        {
            Logger.LogDebug($"Rewarded video loaded, ad info:{JsonConvert.SerializeObject(adUnitId)}");
            OnLoaded?.Invoke(adInfo.AdUnitIdentifier, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }
        public void OnRewardedAdFailedEvent(string adUnitId, ErrorInfo errorInfo)
        {
            var errorMessage = $"Rewarded video load failed. ErrorCode:{errorInfo.Code},Message:{errorInfo.Message}, FailureInfo:{errorInfo.AdLoadFailureInfo}";
            Logger.LogError(errorMessage);
            OnLoadFailed?.Invoke(adUnitId, errorMessage);
        }
        public void OnRewardedAdFailedToDisplayEvent(string adUnitId, ErrorInfo errorInfo, AdInfo adInfo)
        {
            var errorMessage = $"Rewarded video show failed. ErrorCode:{errorInfo.Code},Message:{errorInfo.Message}, FailureInfo:{errorInfo.AdLoadFailureInfo}";
            Logger.LogError(errorMessage);
            OnShowFailed?.Invoke(adUnitId, errorMessage, adInfo.Placement);
        }
        public void OnRewardedAdDisplayedEvent(string adUnitId, AdInfo adInfo)
        {
            Logger.LogDebug($"Rewarded shown, ad info:{JsonConvert.SerializeObject(adUnitId)}");
            OnShown?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }
        public void OnRewardedAdClickedEvent(string adUnitId, AdInfo adInfo)
        {
            Logger.LogDebug($"Rewarded clicked, ad info:{JsonConvert.SerializeObject(adUnitId)}");
            OnClicked?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }
        public void OnRewardedAdDismissedEvent(string adUnitId, AdInfo adInfo)
        {
            Logger.LogDebug($"Rewarded hidden, ad info:{JsonConvert.SerializeObject(adUnitId)}");
            OnClosed?.Invoke(adUnitId, new AdCallbackInfo
            {
                AdUnitId = adUnitId,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue
            });
        }
        public void OnRewardedAdRevenuePaidEvent(string adUnitId, AdInfo adInfo)
        {
            Logger.LogDebug($"Rewarded revenue paid, ad info:{JsonConvert.SerializeObject(adUnitId)}");
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

        public bool HasRewardedVideo(string adUnitId)
        {
            return MaxSdk.IsRewardedAdReady(adUnitId);
        }

        public void RequestRewardedVideo(string adUnitId)
        {
            MaxSdk.LoadRewardedAd(adUnitId);
        }

        public void ShowRewardedVideo(string adUnitId, string placement)
        {
            MaxSdk.ShowRewardedAd(adUnitId, placement);
        }
    }
}
