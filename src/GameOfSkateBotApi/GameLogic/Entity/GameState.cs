namespace GameOfSkateBotApi.GameLogic.Entity
{
    public record GameState(long GameId, Stack<string> Tricks);
}
