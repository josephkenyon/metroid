using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assets
{
    public class CharacterSounds
    {
        public SortedList<int, SoundEffect> jumpSounds;
        public SortedList<int, SoundEffect> jumpVoiceSounds;
        public SortedList<int, SoundEffect> runSounds;
        public SoundEffect hurtSound;
        public SoundEffect deathSound;
        public SoundEffect emptyGunSound;
        public SoundEffect ammoCollectSound;
        public SoundEffect healthUpSound;

        public CharacterSounds(ContentManager content) {
            jumpVoiceSounds = new SortedList<int, SoundEffect> {
                { 1, content.Load<SoundEffect>("Sound\\jump1") },
                { 2, content.Load<SoundEffect>("Sound\\jump2") }
            };

            jumpSounds = new SortedList<int, SoundEffect> {
                { 1, content.Load<SoundEffect>("Sound\\jumpfx1") },
                { 2, content.Load<SoundEffect>("Sound\\jumpfx2") }
            };

            runSounds = new SortedList<int, SoundEffect> {
                { 1, content.Load<SoundEffect>("Sound\\jump1") },
            };

            emptyGunSound = content.Load<SoundEffect>("Sound\\ammoEmpty");
            ammoCollectSound = content.Load<SoundEffect>("Sound\\ammoCollect");
            healthUpSound = content.Load<SoundEffect>("Sound\\healthUp");
            hurtSound = content.Load<SoundEffect>("Sound\\hurt");
            deathSound = content.Load<SoundEffect>("Sound\\deathSound");
        }
    }
}
