using System.Threading;
using UnityEngine;

namespace SMoonUniversalAsset
{
    public static class CancellationTokenSourceExtensions
    {
        public static CancellationToken ResetToken(this CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new();
            return cancellationTokenSource.Token;
        }
    }
}