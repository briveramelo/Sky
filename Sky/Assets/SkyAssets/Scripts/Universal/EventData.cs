using BRM.Sky.CustomWaveData;

public interface ISkyEventData
{
}

public class ContinueData : ISkyEventData
{
    public int NumContinuesRemaining;
}

public class BasketDeathData : ISkyEventData
{
    public int NumContinuesRemaining;
}

public class WeaponGrabbedData : ISkyEventData
{
    public ICollectable Collectable;
}

public class PauseData : ISkyEventData
{
    public bool IsPaused;
}

public class WaveEditorTestData : ISkyEventData
{
    public WaveEditorState State;
    public WaveData WaveData;
}

public class WaveEditorSliderData : ISkyEventData
{
    public int TargetWave;
}

public class BatchStartData : ISkyEventData
{
    public int StartedBatchIndex;
}

public class BatchSavedData : ISkyEventData
{
}

public enum WaveEditorState
{
    Testing,
    Editing,
}