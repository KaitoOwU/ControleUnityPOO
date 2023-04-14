
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type

        [Test]
        public void HealDoesntOverflowMaxHP()
        {
            Character c = new Character(100, 10, 10, 10, TYPE.NORMAL);
            c.Heal(99999);
            Assert.That(c.CurrentHealth, Is.AtMost(c.MaxHealth));
        }

        [Test]
        public void HealNegativeHP()
        {
            Character c = new Character(100, 10, 10, 10, TYPE.NORMAL);
            Assert.Throws<ArgumentException>(() =>
            {
                c.Heal(-1);
            });

        }

        [Test]
        public void LowerMaxHPReduceCurrentHP()
        {
            Character c = new Character(100, 10, 10, 10, TYPE.NORMAL);
            Equipment e = new Equipment(100, 0, 0, 0);

            c.Equip(e);
            Assert.That(c.CurrentHealth, Is.EqualTo(200));

            c.Unequip();
            Assert.That(c.CurrentHealth, Is.EqualTo(c.MaxHealth));
        }

        [Test]
        public void PriorityEquipment()
        {

        }

        [Test]
        public void PokemonStatusSleepDontAttack()
        {
            Character pikachu = new(100, 10, 10, 10, TYPE.NORMAL);
            Character dracaufeu = new(100, 20, 20, 20, TYPE.GRASS);
            Fight f = new Fight(pikachu, dracaufeu);
            MagicalGrass s = new();
            Punch p = new();
            f.ExecuteTurn(p, s);

            //Correctement appliqué le statut SLEEP
            Assert.That(pikachu.CurrentStatus.CanAttack, Is.EqualTo(false));

            //Attaque loupée
            int dracaufeuHP = dracaufeu.CurrentHealth;
            f.ExecuteTurn(p, p);
            Assert.That(dracaufeu.CurrentHealth, Is.EqualTo(dracaufeuHP));
        }

        [Test]
        public void PokemonStatusBurnDamage()
        {

        }

        [Test]
        public void PokemonStatusCrazyDamage()
        {

        }

        [Test]
        public void StrengthWeaknesses()
        {

        }

        [Test]
        public void EquipmentStrengthWeaknesses()
        {

        }

    }
}
