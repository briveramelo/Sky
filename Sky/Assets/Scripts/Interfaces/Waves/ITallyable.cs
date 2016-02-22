public interface ITallyable {
	void TallyBirth(BirdType birdType);
	void TallyDeath(BirdType birdType);
	void TallyKill(BirdType birdType);
	void TallyPoints(BirdType birdType, int thesePoints, float pointMultiplier);
}
