#if LOOGA_FMOD_UNITASK_SUPPORT
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LoogaSoft.FMOD.Runtime
{
    public static class LoogaFmodUniTaskExtensions
    {
        public static async UniTask WaitUntilStoppedAsync(
            this LoogaFmodHandle handle,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            while (handle.IsPlaying)
            {
                await UniTask.Yield(timing, cancellationToken);
            }
        }

        public static async UniTask StopAndWaitAsync(
            this LoogaFmodHandle handle,
            LoogaFmodStopMode mode = LoogaFmodStopMode.AllowFadeout,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            if (!handle.IsValid)
                return;

            handle.Stop(mode, release: false);
            await handle.WaitUntilStoppedAsync(timing, cancellationToken);
            handle.Release();
        }

        public static async UniTask PlayLoopUntilCanceledAsync(
            this LoogaFmodEmitter emitter,
            CancellationToken cancellationToken,
            LoogaFmodStopMode stopMode = LoogaFmodStopMode.AllowFadeout,
            PlayerLoopTiming timing = PlayerLoopTiming.Update)
        {
            if (emitter == null)
                throw new ArgumentNullException(nameof(emitter));

            emitter.StartLoop();

            try
            {
                await UniTask.WaitUntilCanceled(cancellationToken, timing);
            }
            finally
            {
                emitter.Stop(stopMode);
            }
        }
    }
}
#endif