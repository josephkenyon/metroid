using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class CharacterStats
    {
        public int shotsFired = 0;
        public int shotsHit = 0;
        public float damageDealt = 0;
        public float hitPointsHealed = 0;
        public float AccuracyPercentage => shotsFired != 0 ? (float)shotsHit / shotsFired * 100f : 0f;
    }
}
