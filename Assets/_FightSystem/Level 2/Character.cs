using System;
using System.Diagnostics;
using UnityEngine.WSA;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPD
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        //
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public int MaxExperience { get; private set; }

        private const int BaseExperienceToNextLevel = 100;
        public int MaxLevel { get; set; }
        public bool MaxLevelReached { get; set; }
        public int ExperiencePoints { get; set; }
        public int ExperienceToNextLevel { get; set; }
        
        public Character(int level, int experience, int maxExperience)
        {
            Level = level;
            Experience = experience;
            MaxExperience = maxExperience;
        }
        //

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;

            CurrentHealth = baseHealth;

            //
            Level = 1;
            ExperiencePoints = 0;
            ExperienceToNextLevel = BaseExperienceToNextLevel;
            //
        }


        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        public int CurrentHealth { get; private set; }
        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth
        {
            get
            {
                if(CurrentEquipment is null)
                {
                    return _baseHealth;
                } else
                {
                    return _baseHealth + CurrentEquipment.BonusHealth;
                }
            }
        }

        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                if (CurrentEquipment is null)
                {
                    return _baseAttack;
                }
                else
                {
                    return _baseAttack + CurrentEquipment.BonusAttack;
                }
            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                if (CurrentEquipment is null)
                {
                    return _baseDefense;
                }
                else
                {
                    return _baseDefense + CurrentEquipment.BonusDefense;
                }
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                if (CurrentEquipment is null)
                {
                    return _baseSpeed;
                }
                else
                {
                    return _baseSpeed + CurrentEquipment.BonusSpeed;
                }
            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttack(Skill s)
        {
            if(s is null)
            {
                throw new ArgumentNullException("s", "This variable is null");
            }

            CurrentHealth = Math.Clamp(CurrentHealth - (s.Power - Defense), 0, MaxHealth);

            if(s.Status != StatusPotential.NONE && CurrentStatus is null)
            {
                CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
            }
        }

        public void ReceiveAttack(Skill s, Character launcher)
        {
            if (s is null)
            {
                throw new ArgumentNullException("s", "This variable is null");
            }
            if (launcher is null)
            {
                throw new ArgumentNullException("launcher", "This variable is null");
            }

            if (launcher.CurrentStatus is not null)
            {
                if (!launcher.CurrentStatus.CanAttack)
                {
                    return;
                }

                if (launcher.CurrentStatus.DamageOnAttack > 0f)
                {
                    launcher.Damage((int) ((s.Power + launcher.Attack - Defense) * launcher.CurrentStatus.DamageOnAttack));
                }
            }

            UnityEngine.Debug.Log(CurrentHealth);
            UnityEngine.Debug.Log((s.Power + Math.Clamp(launcher.Attack - Defense, 0, int.MaxValue)));
            CurrentHealth = Math.Clamp(CurrentHealth - (s.Power + Math.Clamp(launcher.Attack - Defense, 0, int.MaxValue)), 0, MaxHealth);
            UnityEngine.Debug.Log(CurrentHealth);

            if (s.Status != StatusPotential.NONE && CurrentStatus is null)
            {
                CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
            }
        }
        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if(newEquipment is null)
            {
                throw new ArgumentNullException("newEquipment", "This variable is null");
            }

            CurrentEquipment = newEquipment;
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
            if(CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        /// <summary>
        /// Heal un certain nombre de HP au pokémon
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Value cannot be under or equal 0", "amount");

            CurrentHealth = Math.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }

        /// <summary>
        /// Enlève un certain nombre de HP sans avoir à passer par l'instanciation d'une attaque
        /// </summary>
        public void Damage(int amount)
        {
            CurrentHealth = Math.Clamp(CurrentHealth - amount, 0, MaxHealth);
        }

        public void GainExperience(int experience)
        {
            if (experience <= 0)
            {
                return;
            }

            ExperiencePoints += experience;

            MaxLevelReached = (Level >= MaxLevel);

            if (!MaxLevelReached && ExperiencePoints >= ExperienceToNextLevel)
            {
                Level++;

                ExperienceToNextLevel = Level * 100;

                if (ExperiencePoints > ExperienceToNextLevel)
                {
                    var remainingExperience = ExperiencePoints - ExperienceToNextLevel;
                    GainExperience(remainingExperience);
                }
            }
            else if (MaxLevelReached)
            {
            }
        }

        private int ComputeMaxExperience(int level)
        {
            // calculer la valeur maxExperience en fonction du niveau
            return (int)Math.Pow(level, 3);
        }

        public void SetMaxLevel(int maxLevel)
        {
            MaxLevel = maxLevel;
        }

    }
}
