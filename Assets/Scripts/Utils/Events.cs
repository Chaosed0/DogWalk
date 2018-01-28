public struct RoundStartEvent {}

public struct RoundActuallyStartEvent { }
public struct LevelCreationStartEvent { }
public struct LevelCreationActuallyStartEvent { }
public struct StartDogSequenceEvent { }
public struct EndDogSequenceEvent { }
public struct WinrarEvent {
    public int player;
    public WinrarEvent(int player) { this.player = player; }
}

public struct RoundEndEvent {
    public float remainingTime;

    public RoundEndEvent(float remainingTime)
    {
        this.remainingTime = remainingTime;
    }
}

public struct ToggleCurrentPlayerEvent
{
    public int currentPlayer;

    public ToggleCurrentPlayerEvent(int currentPlayer)
    {
        this.currentPlayer = currentPlayer;
    }
}

public struct ActiveSegmentHoveredEvent
{
    public ConnectionUI connectionUI;

    public ActiveSegmentHoveredEvent(ConnectionUI connectionUI)
    {
        this.connectionUI = connectionUI;
    }
}

public struct NoSegmentsHoveredEvent
{
    public ConnectionUI connectionUI;

    public NoSegmentsHoveredEvent (ConnectionUI connectionUI)
    {
        this.connectionUI = connectionUI;
    }
}

public struct ToggleTendrilEvent
{
    public PathTendril tendril;

    public ToggleTendrilEvent(PathTendril tendril)
    {
        this.tendril = tendril;
    }
}