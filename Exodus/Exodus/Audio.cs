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

            Attack[typeof(Gunner)] = LoadAudio(content, "Audio/6198", true);
            Attack[typeof(Spider)] = null;
            Attack[typeof(Worker)] = null;
            Attack[typeof(Creeper)] = null;
            Attack[typeof(Nothing1x1)] = null;
            Attack[typeof(Nothing2x2)] = null;
            Attack[typeof(Habitation)] = null;
            Attack[typeof(University)] = null;
            Attack[typeof(Laboratory)] = null;
            Attack[typeof (Gas)] = null;
            Attack[typeof(Laserman)] = LoadAudio(content, "Audio/laserman-attack", true);
            Attack[typeof(Iron)] = null;
            Attack[typeof(HydrogenExtractor)] = null;
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
            Die[typeof(Laserman)] = null;
            Die[typeof(Iron)] = null;
            Die[typeof(HydrogenExtractor)] = null;
            Selection[typeof(Gunner)] = LoadAudio(content, "Audio/gunner-selection", false);
            Selection[typeof(Spider)] = null;
            Selection[typeof(Worker)] = LoadAudio(content, "Audio/worker-selection", false);
            Selection[typeof(Creeper)] = null;
            Selection[typeof(Nothing1x1)] = null;
            Selection[typeof(Nothing2x2)] = null;
            Selection[typeof(Habitation)] = null;
            Selection[typeof(University)] = null;
            Selection[typeof(Laboratory)] = null;
            Selection[typeof (Gas)] = null;
            Selection[typeof(Laserman)] = LoadAudio(content, "Audio/laserman-selection", false);
            Selection[typeof(Laserman)].Volume = (float)Data.Config.LevelSound / 100f;
            Selection[typeof(Iron)] = null;
            Selection[typeof(HydrogenExtractor)] = null;
        }

        private static SoundEffectInstance LoadAudio(ContentManager content, string name, bool looped)
        {
            SoundEffectInstance s = content.Load<SoundEffect>(name).CreateInstance();
            s.IsLooped = looped;
            s.Volume = (float) Data.Config.LevelSound/100f;
            return s;
        }
    }
}
