using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Adventure
{
	public class SoundFont
	{
		const int SOUNDFONT_COUNT = 095;
		const double SOUND_JITTER = 0.1;
		SoundEffect[] sounds;
		String directory;
		Random random;
		public SoundFont (String directoryName)
		{
			this.directory = directoryName;
			this.random = new Random ();
		}

		public void LoadContent(ContentManager content)
		{
			sounds = new SoundEffect[SOUNDFONT_COUNT];
			for (int i = 0; i < SOUNDFONT_COUNT; i++) {
				sounds[i] = content.Load<SoundEffect> (directory + "/" + i.ToString().PadLeft(3, '0'));
			}
		}

		public SoundEffectInstance FetchRandomInstance() {
			int number = random.Next() % SOUNDFONT_COUNT;
			SoundEffectInstance instance = sounds [number].CreateInstance();
			instance.Pitch = (float)(random.NextDouble() * SOUND_JITTER * 2 - SOUND_JITTER);
			return instance;
		}
	}
}

