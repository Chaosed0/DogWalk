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