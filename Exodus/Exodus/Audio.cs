using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Exodus.PlayGame.Items.Units;
using Exodus.PlayGame.Items.Obstacles;
using Exodus.PlayGame.Items.Buildings;

namespace Exodus
{
    internal static class Audio
    {
        public static SoundEffectInstance MenuMusic;
        public static SoundEffectInstance PlayStateMusic;
        public static Dictionary<Type, SoundEffectInstance> Attack = new Dictionary<Type, SoundEffectInstance>();
        public static Dictionary<Type, SoundEffectInstance> Die = new Dictionary<Type, SoundEffectInstance>();
        public static Dictionary<Type, SoundEffectInstance> Selection = new Dictionary<Type, SoundEffectInstance>();

        public static void LoadAudio(ContentManager content)
        {
            MenuMusic = content.Load<SoundEffect>("Audio/The-me").CreateInstance();
            MenuMusic.IsLooped = true;
            MenuMusic.Volume = (float) Data.Config.LevelSound/100f;
            PlayStateMusic = content.Load<SoundEffect>("Audio/The-me-2").CreateInstance();
            PlayStateMusic.IsLooped = true;
            PlayStateMusic.Volume = (float) Data.Config.LevelSound/100f;

            Attack[typeof(Gunner)] = LoadAudio(content, "Audio/6198");
            Attack[typeof(Spider)] = null;
            Attack[typeof(Worker)] = null;
            Attack[typeof(Creeper)] = null;
            Attack[typeof(Nothing1x1)] = null;
            Attack[typeof(Nothing2x2)] = null;
            Attack[typeof(Habitation)] = null;
            Attack[typeof(University)] = null;
            Attack[typeof(Laboratory)] = null;
            Attack[typeof (Gas)] = null;
            Die[typeof(Gunner)] = null;
            Die[typeof(Spider)] = null;
            Die[typeof(Worker)] = null;
            Die[typeof(Creeper)] = null;
            Die[typeof(Nothing1x1)] = null;
            Die[typeof(Nothing2x2)] = null;
            Die[typeof(Habitation)] = null;
            Die[typeof(University)] = null;
            Die[typeof(Laboratory)] = null;
            Die[typeof (Gas)] = null;
            Selection[typeof(Gunner)] = null;
            Selection[typeof(Spider)] = null;
            Selection[typeof(Worker)] = null;
            Selection[typeof(Creeper)] = null;
            Selection[typeof(Nothing1x1)] = null;
            Selection[typeof(Nothing2x2)] = null;
            Selection[typeof(Habitation)] = null;
            Selection[typeof(University)] = null;
            Selection[typeof(Laboratory)] = null;
            Selection[typeof (Gas)] = null;
        }

        private static SoundEffectInstance LoadAudio(ContentManager content, string name)
        {
            SoundEffectInstance s = content.Load<SoundEffect>(name).CreateInstance();
            s.IsLooped = true;
            s.Volume = (float) Data.Config.LevelSound/100f;
            return s;
        }
    }
}
