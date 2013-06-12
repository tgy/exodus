using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Exodus
{
    static class Audio
    {
        public static SoundEffectInstance MenuMusic;
        public static SoundEffectInstance PlayStateMusic;
        public static Dictionary<string, SoundEffectInstance> Attack = new Dictionary<string,SoundEffectInstance>();
        public static Dictionary<string, SoundEffectInstance> Die = new Dictionary<string,SoundEffectInstance>();
        public static void LoadAudio(ContentManager content)
        {
            MenuMusic = content.Load<SoundEffect>("Audio/The-me").CreateInstance();
            MenuMusic.IsLooped = true;
            MenuMusic.Volume = (float)Data.Config.LevelSound / 100f;
            PlayStateMusic = content.Load<SoundEffect>("Audio/The-me-2").CreateInstance();
            PlayStateMusic.IsLooped = true;
            PlayStateMusic.Volume = (float)Data.Config.LevelSound / 100f;
            Attack.Add("Gunner", content.Load<SoundEffect>("Audio/6198").CreateInstance());
            Attack["Gunner"].IsLooped = true;
            Attack["Gunner"].Volume = (float)Data.Config.LevelSound / 100f;
        }
    }
}
