public class Pigeon : LinearBird
{
    protected override BirdType MyBirdType => BirdType.Pigeon;
    protected override void Awake()
    {
        MoveSpeed = 2f;
        base.Awake();
    }
}