using UnityEngine; 

public struct DamageKill {
    public int damage; public int Damage => damage;
    public int kill;   public int Kill => kill;

    public DamageKill(int damage, int kill) {
        this.damage = damage;
        this.kill = kill;
    }
    public DamageKill(DamageKill model) {
        this.damage = model.Damage;
        this.kill = model.Kill;
    }
    public void Redefine(int damage, int kill) {
        this.damage = damage;
        this.kill = kill;
    }
    public void Multiply(int multiplier) {
        this.damage *= multiplier;
        this.kill *= multiplier;
    }
    public int Total(int damageMultiplier) {
        return damage * (damageMultiplier-1) + kill;
    }
    public int GetDeathOrDamage(bool isDead, int damageTaken) {
        return isDead ? Total(damageTaken) : damage*damageTaken;
    }
}

public class BirdStats {

    public int DamageTaken;

	const int basePointMultiplier =10;
	BirdType myBirdType;		        public BirdType MyBirdType => myBirdType;
	Vector2 birdPosition;   	        public Vector2 BirdPosition{get => birdPosition;
		set => birdPosition = value;
	}
	int health;					        public int Health{get => health;
		set => health = value;
	}

    int streakPoints;                   public int StreakPoints => streakPoints * basePointMultiplier;
    int comboPoints;                    public int ComboPoints => comboPoints * basePointMultiplier;

    DamageKill pointBase, pointsToGive, threat, guts;

    public int TotalThreatValue => threat.Total(health);
    public int TotalPointValue => pointBase.Total(health) * basePointMultiplier;

    public int GutsToSpill => guts.GetDeathOrDamage(health<=0, DamageTaken+1);
    public int PointsToAdd => pointsToGive.GetDeathOrDamage(health<=0, DamageTaken) * basePointMultiplier;
    public int ThreatRemoved => threat.GetDeathOrDamage(health<=0, DamageTaken);

    public void ModifyForStreak(int birdStreak){
        streakPoints = birdStreak-1;
        pointsToGive.Redefine(pointBase.Damage + streakPoints, pointBase.Kill + streakPoints);
	}
	public void ModifyForCombo(int birdsHit){
        comboPoints = health<=0 ? pointsToGive.Kill : pointsToGive.Damage;
        pointsToGive.Multiply(birdsHit);
        comboPoints = (health<=0 ? pointsToGive.Kill : pointsToGive.Damage) - comboPoints;
	}
    public void ModifyForEvent(int newKillPoint) {
        pointBase.kill = pointsToGive.kill = threat.kill = newKillPoint;
    }

	public BirdStats(BirdType birdType){
		myBirdType = birdType;
		health = 1;
        pointsToGive = guts = threat = new DamageKill(1, 1);

        switch (birdType){
            case BirdType.Pigeon:
                guts.kill = 3;
		    break;
	        case BirdType.Duck:
		        guts.kill = 4;
                threat.kill = 1;
		        //pointsToGive.Kill set to 3 if going solo or upon breaking formation
		        break;
	        case BirdType.DuckLeader:
		        guts.kill = 7;
                threat.kill = 2;
		        pointsToGive.kill = 2;
		        break;
	        case BirdType.Albatross:
		        health = 7;
		        guts.Redefine(10, 60);
                threat.Redefine(1, 3);
                pointsToGive.Redefine(2, 20);
		        break;
	        case BirdType.BabyCrow:
		        guts.kill = 1;
                threat.Redefine(0, 0);
		        pointsToGive.Redefine(0,0);
		        break;
	        case BirdType.Crow:
		        guts.kill = 5;
                threat.kill = 3;
		        pointsToGive.kill = 10;
		        break;
	        case BirdType.Seagull:
		        guts.kill = 4;
                threat.kill = 2;
		        pointsToGive.kill = 2;
		        break;
	        case BirdType.Tentacles:
		        health = 25;
		        guts.Redefine(4, 80);
                threat.Redefine(0, 15);
                pointsToGive.Redefine(1, 15);
		        break;
	        case BirdType.Pelican:
		        health = 2;
                guts.Redefine(4, 10);
                threat.Redefine(2, 4);
		        pointsToGive.Redefine(2, 5);
		        break;
	        case BirdType.Shoebill:
		        guts.kill = 6;
                threat.kill = 3;
		        pointsToGive.kill = 2;
		        break;
	        case BirdType.Bat:
		        guts.kill = 3;
                threat.kill = 3;
		        pointsToGive.kill = 2;
		        break;
	        case BirdType.Eagle:
		        health = 30;
		        guts.Redefine (4, 160);
                threat.Redefine(0,50);
		        pointsToGive.Redefine(5, 100);
		        break;
	        case BirdType.BirdOfParadise:
		        guts.kill = 40;
                threat.kill = 0;
                pointsToGive.kill = 10;
		        break;
		}

		pointBase = new DamageKill(pointsToGive);
	}
}