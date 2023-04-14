
using System;
using System.Diagnostics;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Fight
    {
        public Fight(Character character1, Character character2)
        {
            //Guard
            if (character1 == null) throw new ArgumentNullException("character1", "This variable is null");
            if (character2 == null) throw new ArgumentNullException("character2", "This variable is null");

            Character1 = character1;
            Character2 = character2;
        }

        public Character Character1 { get; }
        public Character Character2 { get; }
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished => Character1.CurrentHealth <= 0 || Character2.CurrentHealth <= 0;

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurn(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            if(Character1.CurrentEquipment is null)
            {
                if(Character2.CurrentEquipment is null)
                {
                    LaunchAttack(skillFromCharacter1, skillFromCharacter2);
                    return;
                } else if (Character2.CurrentEquipment.PrioAttack)
                {
                    LaunchAttack(skillFromCharacter1, skillFromCharacter2, Character2);
                    return;
                }
            } else if(Character2.CurrentEquipment is null)
            {
                if (Character1.CurrentEquipment.PrioAttack)
                {
                    LaunchAttack(skillFromCharacter1, skillFromCharacter2, Character1);
                    return;
                }
            }
            LaunchAttack(skillFromCharacter1, skillFromCharacter2);
            return;
        }

        private void LaunchAttack(Skill skillFromCharacter1, Skill skillFromCharacter2, Character priority = null)
        {
            if(priority is null)
            {
                Character faster = GetFasterCharacterInFight();
                if(faster == Character1)
                {
                    Character2.ReceiveAttack(skillFromCharacter1, Character1);
                    if (Character2.IsAlive)
                    {
                        Character1.ReceiveAttack(skillFromCharacter2, Character2);
                    }
                } else
                {
                    Character1.ReceiveAttack(skillFromCharacter2, Character2);
                    if (Character1.IsAlive)
                    {
                        Character2.ReceiveAttack(skillFromCharacter1, Character1);
                    }
                }
            } else
            {
                if(priority == Character1)
                {
                    Character2.ReceiveAttack(skillFromCharacter1, Character1);
                    if (Character2.IsAlive)
                    {
                        Character1.ReceiveAttack(skillFromCharacter2, Character2);
                    }
                } else
                {
                    Character1.ReceiveAttack(skillFromCharacter2, Character2);
                    if (Character1.IsAlive)
                    {
                        Character2.ReceiveAttack(skillFromCharacter1, Character1);
                    }
                }
            }

            Character1.CurrentStatus?.EndTurn(Character1);
            Character2.CurrentStatus?.EndTurn(Character2);
        }

        private Character GetFasterCharacterInFight()
        {
            if (Character1.Speed > Character2.Speed)
            {
                return Character1;
            }
            else if (Character1.Speed == Character2.Speed)
            {
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                    default:
                        return Character1;
                    case 1:
                        return Character2;
                }
            }
            else
            {
                return Character2;
            }
        }
    }
}
