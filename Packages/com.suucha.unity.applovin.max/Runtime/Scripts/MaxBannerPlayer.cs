using Newtonsoft.Json;
using SuuchaStudio.Unity.Core;
using SuuchaStudio.Unity.Core.AdPlaying;

namespace SuuchaStudio.Unity.Applovin.Max
{
    public class MaxBannerPlayer : SuuchaBase, IBannerPlayer
    {

        public event BannerLoadedHandler OnLoaded;
        public event BannerFailedHandler OnLoadFailed;
        public event BannerClickedHandler OnClicked;
        public event BannerRevenuePaidHandler OnRevenuePaid;
        public MaxBannerPlayer()
        {
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClickedEventInternal;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoadedEventInternal;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailedEventInternal;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEventInternal;
        }

        private void OnAdRevenuePaidEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Banner revenue paid, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnRevenuePaid?.Invoke(adUnitId, new AdCallbackInfo
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
            var errorMessage = $"Banner load failed. ErrorCode:{errorInfo.Code},Message:{errorInfo.Message}, FailureInfo:{errorInfo.AdLoadFailureInfo}";
            Logger.LogError(errorMessage);
            OnLoadFailed?.Invoke(adUnitId, errorMessage);
        }

        private void OnAdLoadedEventInternal(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogDebug($"Banner loaded, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnLoaded?.Invoke(adUnitId, new AdCallbackInfo
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
            Logger.LogDebug($"Banner clicked, ad info:{JsonConvert.SerializeObject(adInfo)}");
            OnClicked?.Invoke(adUnitId, new AdCallbackInfo {
                AdUnitId = adUnitId, 
                NetworkName = adInfo.NetworkName, 
                NetworkPlacement = adInfo.NetworkPlacement, 
                Placement = adInfo.Placement,
                Revenue = adInfo.Revenue });
        }

        public void HideBanner(string adUnitId)
        {
            MaxSdk.HideBanner(adUnitId);
        }

        public void RequestBanner(string adUnitId, BannerPosition position)
        {
            MaxSdk.CreateBanner(adUnitId, Map(position));
        }

        public void ShowBanner(string adUnitId)
        {
            MaxSdk.ShowBanner(adUnitId);
        }
        private MaxSdk.BannerPosition Map(BannerPosition position)
        {
            MaxSdk.BannerPosition maxPosition = MaxSdkBase.BannerPosition.BottomCenter;
            switch (position)
            {
                case BannerPosition.BottomCenter:
                    maxPosition = MaxSdkBase.BannerPosition.BottomCenter;
                    break;
                case BannerPosition.BottomLeft:
                    maxPosition = MaxSdkBase.BannerPosition.BottomLeft;
                    break;
                case BannerPosition.BottomRight:
                    maxPosition = MaxSdkBase.BannerPosition.BottomRight;
                    break;
                case BannerPosition.Centered:
                    maxPosition = MaxSdkBase.BannerPosition.Centered;
                    break;
                case BannerPosition.TopCenter:
                    maxPosition = MaxSdkBase.BannerPosition.TopCenter;
                    break;
                case BannerPosition.TopLeft:
                    maxPosition = MaxSdkBase.BannerPosition.TopLeft;
                    break;
                case BannerPosition.TopRight:
                    maxPosition = MaxSdkBase.BannerPosition.TopRight;
                    break;
            }
            return maxPosition;
        }
    }
}
