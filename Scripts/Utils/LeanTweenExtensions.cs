using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SMoonUniversalAsset
{
    public static class LeanTweenExtensions
    {
        public static UniTask ToUniTask(this LTDescr tween)
        {
            var completionSource = new UniTaskCompletionSource();

            tween.setOnComplete(() => completionSource.TrySetResult());

            return completionSource.Task;
        }
    }

}