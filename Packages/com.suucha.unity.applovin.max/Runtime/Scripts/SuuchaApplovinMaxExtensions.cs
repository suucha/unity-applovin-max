using SuuchaStudio.Unity.Core;
using SuuchaStudio.Unity.Core.AdPlaying;
using SuuchaStudio.Unity.Core.Ioc;

namespace SuuchaStudio.Unity.Applovin.Max
{
    public static class SuuchaMaxExtensions
    {
        public static Suucha UseApplovinMax(this Suucha suucha, string devKey, IAdPlayingStrategy adPlayingStrategy)
        {
            if (!IocContainer.TryResolve<IAdPlayerManager>(out IAdPlayerManager maxAdPlayerManager))
            {
                maxAdPlayerManager = new MaxAdPlayerManager(devKey);
                IocContainer.RegisterInstance<IAdPlayerManager, MaxAdPlayerManager>(maxAdPlayerManager as MaxAdPlayerManager);
            }
            if (!IocContainer.TryResolve<IRewardedVideoPlayer>(out IRewardedVideoPlayer rewardedVideoPlayer))
            {
                rewardedVideoPlayer = new MaxRewardedVideoPlayer();
                IocContainer.RegisterInstance<IRewardedVideoPlayer, MaxRewardedVideoPlayer>(rewardedVideoPlayer as MaxRewardedVideoPlayer);
            }
            if (!IocContainer.TryResolve<IInterstitialVideoPlayer>(out IInterstitialVideoPlayer interstitialVideoPlayer))
            {
                interstitialVideoPlayer = new MaxInterstitialVideoPlayer();
                IocContainer.RegisterInstance<IInterstitialVideoPlayer, MaxInterstitialVideoPlayer>(interstitialVideoPlayer as MaxInterstitialVideoPlayer);
            }
            if (!IocContainer.TryResolve<IBannerPlayer>(out IBannerPlayer bannerPlayer))
            {
                bannerPlayer = new MaxBannerPlayer();
                IocContainer.RegisterInstance<IBannerPlayer, MaxBannerPlayer>(bannerPlayer as MaxBannerPlayer);
            }
            suucha.EnableAdPlaying(maxAdPlayerManager, adPlayingStrategy);
            maxAdPlayerManager.Initialize();
            return suucha;
        }
    }
}