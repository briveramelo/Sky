using UnityEngine;

public struct DamageKill
{
    public int Damage;
    public int Kill;

    public DamageKill(int damage, int kill)
    {
        Damage = damage;
        Kill = kill;
    }

    public DamageKill(DamageKill model)
    {
        Damage = model.Damage;
        Kill = model.Kill;
    }

    public void Redefine(int damage, int kill)
    {
        Damage = damage;
        Kill = kill;
    }

    public void Multiply(int multiplier)
    {
        Damage *= multiplier;
        Kill *= multiplier;
    }

    public int Total(int damageMultiplier)
    {
        return Damage * (damageMultiplier - 1) + Kill;
    }

    public int GetDeathOrDamage(bool isDead, int damageTaken)
    {
        return isDead ? Total(damageTaken) : Damage * damageTaken;
    }
}

public class BirdStats
{
    public int DamageTaken;

    private const int _basePointMultiplier = 10;
    public BirdType MyBirdType { get; }
    public Vector2 BirdPosition { get; set; }
    public int Health { get; set; }

    private int _streakPoints;
    public int StreakPoints => _streakPoints * _basePointMultiplier;
    private int _comboPoints;
    public int ComboPoints => _comboPoints * _basePointMultiplier;

    private DamageKill _pointBase, _pointsToGive, _threat, _guts;

    public int TotalThreatValue => _threat.Total(Health);
    public int TotalPointValue => _pointBase.Total(Health) * _basePointMultiplier;

    public int GutsToSpill => _guts.GetDeathOrDamage(Health <= 0, DamageTaken + 1);
    public int PointsToAdd => _pointsToGive.GetDeathOrDamage(Health <= 0, DamageTaken) * _basePointMultiplier;
    public int ThreatRemoved => _threat.GetDeathOrDamage(Health <= 0, DamageTaken);

    public void ModifyForStreak(int birdStreak)
    {
        _streakPoints = birdStreak - 1;
        _pointsToGive.Redefine(_pointBase.Damage + _streakPoints, _pointBase.Kill + _streakPoints);
    }

    public void ModifyForCombo(int birdsHit)
    {
        _comboPoints = Health <= 0 ? _pointsToGive.Kill : _pointsToGive.Damage;
        _pointsToGive.Multiply(birdsHit);
        _comboPoints = (Health <= 0 ? _pointsToGive.Kill : _pointsToGive.Damage) - _comboPoints;
    }

    public void ModifyForEvent(int newKillPoint)
    {
        _pointBase.Kill = _pointsToGive.Kill = _threat.Kill = newKillPoint;
    }

    public BirdStats(BirdType birdType)
    {
        MyBirdType = birdType;
        Health = 1;
        _pointsToGive = _guts = _threat = new DamageKill(1, 1);

        switch (birdType)
        {
            case BirdType.Pigeon:
                _guts.Kill = 3;
                break;
            case BirdType.Duck:
                _guts.Kill = 4;
                _threat.Kill = 1;
                //_pointsToGive.Kill set to 3 if going solo or upon breaking formation
                break;
            case BirdType.DuckLeader:
                _guts.Kill = 7;
                _threat.Kill = 2;
                _pointsToGive.Kill = 2;
                break;
            case BirdType.Albatross:
                Health = 7;
                _guts.Redefine(10, 60);
                _threat.Redefine(1, 3);
                _pointsToGive.Redefine(2, 20);
                break;
            case BirdType.BabyCrow:
                _guts.Kill = 1;
                _threat.Redefine(0, 0);
                _pointsToGive.Redefine(0, 0);
                break;
            case BirdType.Crow:
                _guts.Kill = 5;
                _threat.Kill = 3;
                _pointsToGive.Kill = 10;
                break;
            case BirdType.Seagull:
                _guts.Kill = 4;
                _threat.Kill = 2;
                _pointsToGive.Kill = 2;
                break;
            case BirdType.Tentacles:
                Health = 25;
                _guts.Redefine(4, 80);
                _threat.Redefine(0, 15);
                _pointsToGive.Redefine(1, 15);
                break;
            case BirdType.Pelican:
                Health = 2;
                _guts.Redefine(4, 10);
                _threat.Redefine(2, 4);
                _pointsToGive.Redefine(2, 5);
                break;
            case BirdType.Shoebill:
                _guts.Kill = 6;
                _threat.Kill = 3;
                _pointsToGive.Kill = 2;
                break;
            case BirdType.Bat:
                _guts.Kill = 3;
                _threat.Kill = 3;
                _pointsToGive.Kill = 2;
                break;
            case BirdType.Eagle:
                Health = 30;
                _guts.Redefine(4, 160);
                _threat.Redefine(0, 50);
                _pointsToGive.Redefine(5, 100);
                break;
            case BirdType.BirdOfParadise:
                _guts.Kill = 40;
                _threat.Kill = 0;
                _pointsToGive.Kill = 10;
                break;
        }

        _pointBase = new DamageKill(_pointsToGive);
    }
}