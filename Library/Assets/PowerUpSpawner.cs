using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public class PowerUpSpawner
    {
        public PowerUpType PowerUpType;
        public Point Location;

        private int timer;
        private PowerUp PowerUp;
        public int tileSize;

        private Texture2D texture;

        public PowerUpSpawner()
        {
        }

        public void Load(PowerUpType powerUpType, Point location, Texture2D texture, int tileSize)
        {
            this.texture = texture;
            PowerUpType = powerUpType;
            Location = location;
            timer = TimerLength;
            PowerUp = null;
            this.tileSize = tileSize;
        }
        private int TimerLength => PowerUpType == PowerUpType.Health ? 60 * 25 : 60 * 18;

        public void Update() {
            if (timer > 0)
                timer--;
            else {
                if (PowerUp == null) {
                    PowerUp = new PowerUp(this);
                    timer = TimerLength;
                }
            }

            PowerUp?.Update();
        }

        public PowerUp GetPowerUp() => PowerUp;

        public void Draw(SpriteBatch spriteBatch, GameProperties gameState) {
            PowerUp?.Draw(spriteBatch, gameState);
        }

        public void PowerUpCollected() {
            timer = TimerLength;
            PowerUp = null;
        }

        public Texture2D GetTexture()
            => texture;
    }
}
