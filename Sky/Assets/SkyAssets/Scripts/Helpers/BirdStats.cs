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

    private const int basePointMultiplier =10;
	public BirdType MyBirdType { get; }
	public Vector2 BirdPosition { get; set; }
	public int Health { get; set; }

	private int streakPoints;                   public int StreakPoints => streakPoints * basePointMultiplier;
	private int comboPoints;                    public int ComboPoints => comboPoints * basePointMultiplier;

	private DamageKill pointBase, pointsToGive, threat, guts;

    public int TotalThreatValue => threat.Total(Health);
    public int TotalPointValue => pointBase.Total(Health) * basePointMultiplier;

    public int GutsToSpill => guts.GetDeathOrDamage(Health<=0, DamageTaken+1);
    public int PointsToAdd => pointsToGive.GetDeathOrDamage(Health<=0, DamageTaken) * basePointMultiplier;
    public int ThreatRemoved => threat.GetDeathOrDamage(Health<=0, DamageTaken);

    public void ModifyForStreak(int birdStreak){
        streakPoints = birdStreak-1;
        pointsToGive.Redefine(pointBase.Damage + streakPoints, pointBase.Kill + streakPoints);
	}
	public void ModifyForCombo(int birdsHit){
        comboPoints = Health<=0 ? pointsToGive.Kill : pointsToGive.Damage;
        pointsToGive.Multiply(birdsHit);
        comboPoints = (Health<=0 ? pointsToGive.Kill : pointsToGive.Damage) - comboPoints;
	}
    public void ModifyForEvent(int newKillPoint) {
        pointBase.kill = pointsToGive.kill = threat.kill = newKillPoint;
    }

	public BirdStats(BirdType birdType){
		MyBirdType = birdType;
		Health = 1;
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
		        Health = 7;
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
		        Health = 25;
		        guts.Redefine(4, 80);
                threat.Redefine(0, 15);
                pointsToGive.Redefine(1, 15);
		        break;
	        case BirdType.Pelican:
		        Health = 2;
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
		        Health = 30;
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