using GameOfSkateBotApi.Buttons;
using GameOfSkateBotApi.GameLogic.Common;
using GameOfSkateBotApi.GameLogic.Entity;
using GameOfSkateBotApi.GameLogic.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace GameOfSkateBotApi.GameLogic
{
    public enum GameEventType {GameEnded, GameStarted, NextTrick}
    public class Game
	{
		private readonly Tricks _tricks;
        private Action<GameEventType, long>? callback;
        private IDistributedCache _cache;
        public Game(Tricks tricks, IDistributedCache cache)
		{
			_tricks = tricks;
			_cache = cache;
		}
        public void SubscribeOnEvents(Action<GameEventType, long> callback) => this.callback = callback;

        public void Unsubscribe() => callback = null;

        /// <summary>
        /// Starts the game for this particular <paramref name="gameId"/>. This means that list of tricks will be
        /// generated for this <paramref name="gameId"/> that should be iterated using <see cref="NextTrick(long)"/>.
        /// The very first trick will be populated as a result of starting a new game.
        /// </summary>
        /// <param name="gameId">Unique Id for new game</param>
        /// <param name="difficulty">Difficulty level</param>
        /// <returns>First trick from generated trick sequence that will be used in this new game.</returns>
        public async Task<string> Start(long gameId, Difficulty difficulty)
		{
            if ((await _cache.GetStringAsync(gameId.ToString())) != null)
                throw new GameAlreadyStartedException("The game is already started, if you want to end it now, I can't help you.");
            var gameState = new GameState(gameId, new Stack<string>(_tricks.GenerateFor(difficulty).Shuffle()));
            await _cache.SetStringAsync(gameState.GameId.ToString(), JsonSerializer.Serialize(gameState));

            return await NextTrick(gameId);
		}

		/// <summary>
		/// Populates new trick for current game. Use this method to iterate over list of tricks, until there are no trick left.
		/// If no tricks are left <see cref="GameNotStartedException"/> will be thrown.
		/// </summary>
		/// <param name="gameId">Unique Id for current game</param>
		/// <returns>Next trick from generated trick sequence that is used in this new game.</returns>
		/// <exception cref="GameNotStartedException">Thrown if there are no tricks left or <see cref="Start(long, Difficulty)"/>
		/// wasn't invoked before accessing this method.</exception>
		public async Task<string> NextTrick(long gameId)
        {
            if ((await _cache.GetStringAsync(gameId.ToString())) == null)
            {
                throw new GameNotStartedException("You didn't start the game, ain't ya?");

            }

            var gameState = JsonSerializer.Deserialize<GameState>(await _cache.GetStringAsync(gameId.ToString()));

            if (!gameState.Tricks.Any())
                return await End(gameId);

            callback?.Invoke(GameEventType.NextTrick, gameId);
            var nextTrick = gameState.Tricks.Pop();
            await _cache.SetStringAsync(gameState.GameId.ToString(), JsonSerializer.Serialize(gameState));

            return nextTrick;
        }

		public async Task<string> End(long gameId)
        {
            if ((await _cache.GetStringAsync(gameId.ToString())) == null)
                throw new GameNotStartedException("You didn't start the game, ain't ya?");

            await _cache.RemoveAsync(gameId.ToString());

            callback?.Invoke(GameEventType.GameEnded, gameId);
            return "Game Over";
        }
    }
}