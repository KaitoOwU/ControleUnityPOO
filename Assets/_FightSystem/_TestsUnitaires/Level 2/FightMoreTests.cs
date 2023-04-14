
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
            Equipment e = new Equipment(100, 0, 0, 0, false);

            c.Equip(e);
            c.Heal(999999);
            Assert.That(c.CurrentHealth, Is.EqualTo(200));

            c.Unequip();
            Assert.That(c.CurrentHealth, Is.EqualTo(c.MaxHealth));
        }

        [Test]
        public void PriorityEquipment()
        {
            Character slow = new Character(100, 100000, 10, 0, TYPE.NORMAL);
            Character fast = new Character(100, 100000, 10, 100, TYPE.NORMAL);
            Equipment e = new Equipment(0, 0, 0, 0, true);
            slow.Equip(e);

            Assert.That(slow.CurrentEquipment.PrioAttack, Is.EqualTo(true));

            Fight f = new Fight(slow, fast);
            Punch p = new Punch();
            f.ExecuteTurn(p, p);
            Assert.That(slow.IsAlive, Is.True);
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
            Character c = new(100, 0, 0, 0, TYPE.NORMAL);
            FireBall f = new();

            c.ReceiveAttack(f);
            Assert.That(c.CurrentStatus.DamageEachTurn, Is.EqualTo(10));

            int h = c.CurrentHealth;
            c.CurrentStatus.EndTurn(c);
            Assert.That(c.CurrentHealth, Is.EqualTo(h - 10));
        }

        [Test]
        public void PokemonStatusCrazyDamage()
        {
            Character c = new(100, 0, 0, 0, TYPE.NORMAL);
            Character c2 = new(100, 0, 0, 0, TYPE.NORMAL);
            Supersonic s = new();

            c.ReceiveAttack(s);
            Assert.That(c.CurrentStatus.DamageOnAttack, Is.EqualTo(0.3f));

            int h = c.CurrentHealth;
            c2.ReceiveAttack(new Punch(), c);
            c.CurrentStatus.EndTurn(c);
            Assert.That(c.CurrentHealth, Is.LessThan(h));
        }

    }
}
