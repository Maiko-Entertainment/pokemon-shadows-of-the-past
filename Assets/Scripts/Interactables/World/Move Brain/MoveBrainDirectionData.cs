[System.Serializable]
public class MoveBrainDirectionData
{
    public MoveBrainDirection direction;
    public bool justTurn = false;
    public bool jump = false;

    public MoveBrainDirectionData(MoveBrainDirection dir, bool justTurn)
    {
        direction = dir;
        this.justTurn = justTurn;
    }
}
