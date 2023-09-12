<!--
 * @Date: 2023-09-12 14:27:51
 * @LastEditors: Xiequan
 * @LastEditTime: 2023-09-12 14:27:59
 * @FilePath: \Suucha.Unity.Applovin.Max\Packages\com.suucha.unity.applovin.max\README.md
 * @Author: Xiequan
-->
# Suucha ADKU - Applovin Max
AppLovin Max是AppLovin公司推出的一种移动广告媒体购买平台和广告中介服务，旨在帮助开发者和广告商更有效地管理和优化移动广告活动。

## 接入Applovin Max Unity SDK
由于Applovin Max目前并未支持UPM，所以需要先去Applovin的官网下载最新的.unitypackage包，手动导入到项目中，下载地址及导入方法见此[链接](https://dash.applovin.com/documentation/mediation/unity/getting-started/integration)

## 接入Suucha Unity Applovin Max
修改Packages/manifest.json文件在dependencies中添加：
``` json
"dependencies": {
  "com.suucha.unity.applovin.max":"1.0.0"，

  //...
 }
```

## 开始

在实现了接口IAfterSuuchaInit的类的Execute方法中，用以下代码启用Applovin Max：
``` csharp
var adPlayingConfig = new MaxTimesAdPlayingStrategyConfiguration()
        {
            RewardedMaxTimes = 60,
            AdUnits = new List<MaxTimesAdUnitConfiguration>
            {
                new MaxTimesAdUnitConfiguration
                {
                    AdType = SuuchaStudio.Unity.Core.AdPlaying.AdType.Rewarded,
                    AdUnitId = "your rewarded ad unit id",
                    MaxTimesToPlay = 100,
                    Priority =1,

                },
                new MaxTimesAdUnitConfiguration
                {
                    AdType = SuuchaStudio.Unity.Core.AdPlaying.AdType.Interstitial,
                    AdUnitId = "your interstitial ad unit id",
                    MaxTimesToPlay = 30,
                    Priority = 1
                },
                new MaxTimesAdUnitConfiguration
                {
                    AdType = SuuchaStudio.Unity.Core.AdPlaying.AdType.Banner,
                    AdUnitId = "your banner ad unit id",
                    MaxTimesToPlay = 30,
                    Priority = 1
                }
            },
            RetryIntervals = new List<int> { 5000, 10000, 12000, 15000 }
        };
        var adPlayingStrategy = new MaxTimesAdPlayingStrategy(adPlayingConfig);
        
Suucha.App.UseApplovinMax("your max dev key", adPlayingStrategy);
```
其中adPlayingStrategy是广告播放策略，例子中是Suucha ADKU中自带的每天最多播放次数策略，也可以自己实现接口IAdPlayingStrategy使用自己的策略播放广告。
Suucha ADKU播放广告时可以设定是否上报对应的埋点事件，通过下面的代码来启用埋点事件上报：
``` csharp
//允许上报的事件
var enableEvents = new List<AdPlayingEnableEvents>
{
AdPlayingEnableEvents.RewardedVideoClicked,
AdPlayingEnableEvents.RewardedVideoRevenuePaid,
AdPlayingEnableEvents.RewardedVideoShown,
AdPlayingEnableEvents.InterstitialVideoClicked
// ...
};
//允许上报事件名称中带placement的事件
//比如在解锁关卡（placement为level2）的时候播放了激励视频，用户点击激励视频，
//此时会上报两个事件，一个是ad_rewarded_click，一个是ad_rewarded_click_level2
var enablePlacements = new List<AdPlayingEnableEvents>
{
AdPlayingEnableEvents.RewardedVideoClicked
//...
};
Suucha.App.EnableAdPlayingEvent(enableEvents, enablePlacements);
```