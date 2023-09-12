
using SuuchaStudio.Unity.Core;
using SuuchaStudio.Unity.Core.AdPlaying;
using Cysharp.Threading.Tasks;

namespace Suucha.Unity.Applovin.Max
{
    public class MaxAdPlayerManager : SuuchaBase, IAdPlayerManager
    {
        private readonly string devKey;
        private bool isInitialized = false;
        public MaxAdPlayerManager(string devKey)
        {
            this.devKey = devKey;
        }
        public UniTask<bool> Initialize()
        {
            if (isInitialized)
            {
                return UniTask.FromResult(isInitialized);
            }
            UniTaskCompletionSource<bool> tcs = new UniTaskCompletionSource<bool>();
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                Logger.LogDebug($"MAX SDK Initialized.");
                isInitialized = true;
                tcs.TrySetResult(true);
            };
            MaxSdkCallbacks.OnSdkConsentDialogDismissedEvent += () =>
            {
                tcs.TrySetResult(false);
            };
            MaxSdk.SetSdkKey(devKey);
            MaxSdk.InitializeSdk();
            return UniTask.FromResult(true);
        }


    }
}
