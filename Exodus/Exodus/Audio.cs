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
        public static Dictionary<Type, SoundEffectInstance> Attack = new Dictionary<Type,SoundEffectInstance>();
        public static Dictionary<string, SoundEffectInstance> Die = new Dictionary<string,SoundEffectInstance>();
        public static Dictionary<string, SoundEffectInstance> Selection = new Dictionary<string,SoundEffectInstance>();
        public static void LoadAudio(ContentManager content)
        {
            MenuMusic = content.Load<SoundEffect>("Audio/The-me").CreateInstance();
            MenuMusic.IsLooped = true;
            MenuMusic.Volume = (float)Data.Config.LevelSound / 100f;
            PlayStateMusic = content.Load<SoundEffect>("Audio/The-me-2").CreateInstance();
            PlayStateMusic.IsLooped = true;
            PlayStateMusic.Volume = (float)Data.Config.LevelSound / 100f;
            Attack.Add(typeof(PlayGame.Items.Units.Gunner), content.Load<SoundEffect>("Audio/6198").CreateInstance());
            Attack[typeof(PlayGame.Items.Units.Gunner)].IsLooped = true;
            Attack[typeof(PlayGame.Items.Units.Gunner)].Volume = (float)Data.Config.LevelSound / 100f;
        }
    }
}
