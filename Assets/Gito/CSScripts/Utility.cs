using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Niwatori
{
    public static class Utility
    {
        public static void RandomFunc(params UnityAction[] funcs)
        {
            int r = Random.Range(0, funcs.Length);
            funcs[r].Invoke();
        }

        public static void PercentDoFunc(float percent, UnityAction hit, UnityAction lost)
        {
            if (Random.value < percent)
            {
                hit.Invoke();
            }
            else
            {
                lost.Invoke();
            }
        }

        public static void Delay(float duration, UnityAction call)
        {
            CoroutineHandler.StartStaticCoroutine(DelayCor(duration, call));
        }

        private static IEnumerator DelayCor(float duration, UnityAction call)
        {
            yield return new WaitForSecondsRealtime(duration);
            call.Invoke();
        }
    }
}
