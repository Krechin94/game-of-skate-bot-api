namespace GameOfSkateBotApi.GameLogic.Exceptions
{
	public class GameNotStartedException : Exception
	{
		public GameNotStartedException(string? message) : base(message)
		{
		}
	}
}
