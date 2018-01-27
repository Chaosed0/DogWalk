public struct RoundStartEvent {}

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
    // add the connection data structure here.
}

public struct NoSegmentsHoveredEvent
{

}