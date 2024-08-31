﻿namespace GameOfSkateBotApi.GameLogic.Exceptions
{
	public class GameAlreadyStartedException : Exception
	{
		public GameAlreadyStartedException(string? message) : base(message)
		{
		}
	}
}
