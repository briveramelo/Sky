public class Pigeon : LinearBird {

	protected override void Awake () {
		moveSpeed =2f;
		birdStats = new BirdStats(BirdType.Pigeon);
		base.Awake();
	}
}
