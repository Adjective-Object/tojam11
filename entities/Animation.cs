using System;
using Microsoft.Xna.Framework.Graphics;

namespace Adventure
{
	public class Animation
	{
		private int currentFrameIndex = 0;
		private double frameTimeCounter = 0;

		private Frame[] frames;
		public double totalAnimTime;

		public Animation (Frame[] frames)
		{
			totalAnimTime = 0;
			foreach (Frame f in frames) {
				totalAnimTime += f.time;
			}
			this.frames = frames;
		}

		public void Reset() {
			this.currentFrameIndex = 0;
			this.frameTimeCounter = 0;
		}

		public Texture2D GetFrame(double elapsedTime) {
			frameTimeCounter += elapsedTime;
			Console.WriteLine (currentFrameIndex + " " + frameTimeCounter + " " + elapsedTime);
			while (frameTimeCounter > this.frames [currentFrameIndex].time) {
				frameTimeCounter -= this.frames [currentFrameIndex].time;
				currentFrameIndex = (currentFrameIndex + 1) % this.frames.Length;
			}
			return this.frames [currentFrameIndex].texture;
		}
	}

	public class Frame {
		public double time;
		public Texture2D texture;
		public Frame (double time, Texture2D texture) {
			this.time = time;
			this.texture = texture;
		}
	}
}

