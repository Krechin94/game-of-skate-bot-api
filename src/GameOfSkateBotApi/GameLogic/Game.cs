using GameOfSkateBotApi.GameLogic.Common;
using GameOfSkateBotApi.GameLogic.Entity;
using GameOfSkateBotApi.GameLogic.Exceptions;

namespace GameOfSkateBotApi.GameLogic
{
	public class Game
	{
		private readonly Tricks _tricks;

		//TODO: replace the Dictionary with game state storage here:
		//public IRedis _cache;
		public Dictionary<long, Stack<string>> _gameIdWithTricks = [];

		public Game(Tricks tricks)
		{
			_tricks = tricks;
		}

		/// <summary>
		/// Starts the game for this particular <paramref name="gameId"/>. This means that list of tricks will be
		/// generated for this <paramref name="gameId"/> that should be iterated using <see cref="NextTrick(long)"/>.
		/// The very first trick will be populated as a result of starting a new game.
		/// </summary>
		/// <param name="gameId">Unique Id for new game</param>
		/// <param name="difficulty">Difficulty level</param>
		/// <returns>First trick from generated trick sequence that will be used in this new game.</returns>
		public Task<string> Start(long gameId, Difficulty difficulty)
		{
			//TODO: add generated 
			//var trickListForThisGame = _cache.Get(gameId);
			//if (trickListForThisGame == null) // if null then generate it
			//if not null throw exception - the game already started!
			if (_gameIdWithTricks.ContainsKey(gameId))
				throw new GameAlreadyStartedException("The game is already started, if you want to end it now, I can't help you.");
			_gameIdWithTricks.Add(gameId, new Stack<string>(_tricks.GenerateFor(difficulty).Shuffle()));

			return NextTrick(gameId);
		}

		/// <summary>
		/// Populates new trick for current game. Use this method to iterate over list of tricks, until there are no trick left.
		/// If no tricks are left <see cref="GameNotStartedException"/> will be thrown.
		/// </summary>
		/// <param name="gameId">Unique Id for current game</param>
		/// <returns>Next trick from generated trick sequence that is used in this new game.</returns>
		/// <exception cref="GameNotStartedException">Thrown if there are no tricks left or <see cref="Start(long, Difficulty)"/>
		/// wasn't invoked before accessing this method.</exception>
		public Task<string> NextTrick(long gameId)
		{
			//TODO: get from cache here then check if game is null -
			//if null then it wasn't ever written to the cache OR it's game over
			if (!_gameIdWithTricks.ContainsKey(gameId))
				throw new GameNotStartedException("You didn't start the game, ain't ya?");

			if (!_gameIdWithTricks[gameId].Any()) 
			
			return End(gameId);
		    
			return Task.FromResult(_gameIdWithTricks[gameId].Pop());
		}

		/// <summary>
		/// Clean all related data for current game
		/// </summary>
		public Task<string> End(long gameId)
		{
			//TODO: remove this code, clear cache destroy stack of tricks
			if (!_gameIdWithTricks.ContainsKey(gameId))
				throw new GameNotStartedException("You didn't start the game, ain't ya?");

			_gameIdWithTricks.Remove(gameId);
			return Task.FromResult("Game Over");
		}
	}
}