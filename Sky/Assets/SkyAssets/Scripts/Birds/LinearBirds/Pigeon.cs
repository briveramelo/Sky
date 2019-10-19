public class Pigeon : LinearBird
{
    protected override BirdType _myBirdType => BirdType.Pigeon;
    protected override void Awake()
    {
        MoveSpeed = 2f;
        base.Awake();
    }
}