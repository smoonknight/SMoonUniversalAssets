using UnityEngine;

namespace SMoonUniversalAsset
{
    [System.Serializable]
    public class UnscaleTimeChecker : TimeChecker
    {
        public override float GetTime() => Time.unscaledTime;
    }
}