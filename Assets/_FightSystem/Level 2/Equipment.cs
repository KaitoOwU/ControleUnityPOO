﻿
namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Défintion simple d'un équipement apportant des boost de stats
    /// </summary>
    public class Equipment
    {

        int _bonusHealth;
        int _bonusAttack;
        int _bonusDefense;
        int _bonusSpeed;
        bool _prioAttack;

        public Equipment(int bonusHealth, int bonusAttack, int bonusDefense, int bonusSpeed, bool prioAttack)
        {
            _bonusHealth = bonusHealth;
            _bonusAttack = bonusAttack;
            _bonusDefense = bonusDefense;
            _bonusSpeed = bonusSpeed;
            _prioAttack = prioAttack;
        }

        public int BonusHealth { get => _bonusHealth; protected set => _bonusHealth = value; }
        public int BonusAttack { get => _bonusAttack; protected set => _bonusAttack = value; }
        public int BonusDefense { get => _bonusDefense; protected set => _bonusDefense = value; }
        public int BonusSpeed { get => _bonusSpeed; protected set => _bonusSpeed = value; }
        public bool PrioAttack { get => _prioAttack; protected set => _prioAttack = value; }
    }
}
