using BRM.Sky.CustomWaveData;

public class ContinueData
{
    public int NumContinuesRemaining;
}

public class WeaponGrabbedData
{
    public ICollectable Collectable;
}

public class PauseData
{
    public bool IsPaused;
}

public class WaveEditorTestData
{
    public WaveEditorState State;
    public WaveData WaveData;
}

public enum WaveEditorState
{
    Testing,
    Editing,
}