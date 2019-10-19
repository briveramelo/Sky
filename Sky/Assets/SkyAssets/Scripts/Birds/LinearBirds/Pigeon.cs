public class Pigeon : LinearBird
{
    public override BirdType MyBirdType => BirdType.Pigeon;
    protected override void Awake()
    {
        MoveSpeed = 2f;
        base.Awake();
    }
}