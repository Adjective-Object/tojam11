using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Adventure
{
	public class SoundFont
	{
		int count;
		const double SOUND_JITTER = 0.1;
		SoundEffect[] sounds;
		String directory;
		Random random;
		public SoundFont (String directoryName, int count)
		{
			this.directory = directoryName;
			this.random = new Random ();
			this.count = count;
		}

		public void LoadContent(ContentManager content)	{
			sounds = new SoundEffect[count + 1];
			for (int i = 0; i <= count; i++) {
				sounds[i] = content.Load<SoundEffect> (directory + "/" + i.ToString().PadLeft(3, '0'));
			}
		}

		public SoundEffectInstance FetchRandomInstance() {
			int number = random.Next() % (count + 1);
			SoundEffectInstance instance = sounds [number].CreateInstance();
			instance.Pitch = (float)(random.NextDouble() * SOUND_JITTER * 2 - SOUND_JITTER);
			return instance;
		}
	}
}

