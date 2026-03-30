using System;

namespace TapCat.Animation
{
    /// <summary>
    /// Tap-driven animation state module that tracks frame index and loop count.
    /// </summary>
    public sealed class AnimationModule
    {
        /// <summary>
        /// Total frames for TapCat animation (fixed 10 frames, 0-9).
        /// </summary>
        public const int FixedFrameCount = 10;

        private int currentFrameIndex;
        private int completedLoops;

        /// <summary>
        /// Event fired when the current frame index changes.
        /// </summary>
        public event Action<int> OnFrameChanged;

        /// <summary>
        /// Event fired when a full loop completes.
        /// </summary>
        public event Action<int> OnLoopCompleted;

        /// <summary>
        /// Event fired when the status text changes.
        /// </summary>
        public event Action<string> OnStatusChanged;

        /// <summary>
        /// Current zero-based frame index (0-9).
        /// </summary>
        public int CurrentFrameIndex => currentFrameIndex;

        /// <summary>
        /// Number of completed loops.
        /// </summary>
        public int CompletedLoops => completedLoops;

        /// <summary>
        /// Total frames.
        /// </summary>
        public int TotalFrames => FixedFrameCount;

        /// <summary>
        /// Status text in the format "循环X，帧Y/10".
        /// </summary>
        public string StatusInfo => BuildStatus(completedLoops, currentFrameIndex);

        /// <summary>
        /// Advance one frame in response to input.
        /// </summary>
        public void AdvanceFrame()
        {
            int nextFrame = currentFrameIndex + 1;
            if (nextFrame >= FixedFrameCount)
            {
                completedLoops++;
                currentFrameIndex = 0;
                OnLoopCompleted?.Invoke(completedLoops);
            }
            else
            {
                currentFrameIndex = nextFrame;
            }

            OnFrameChanged?.Invoke(currentFrameIndex);
            OnStatusChanged?.Invoke(StatusInfo);
        }

        /// <summary>
        /// Reset to the first frame and clear loop count.
        /// </summary>
        public void ResetState()
        {
            currentFrameIndex = 0;
            completedLoops = 0;

            OnFrameChanged?.Invoke(currentFrameIndex);
            OnStatusChanged?.Invoke(StatusInfo);
        }

        private static string BuildStatus(int loopCount, int frameIndex)
        {
            int displayFrame = frameIndex + 1;
            // "循环X，帧Y/10"
            return string.Format("循环{0}，帧{1}/10", loopCount, displayFrame);
        }
    }
}