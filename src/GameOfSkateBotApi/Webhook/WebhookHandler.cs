using GameOfSkateBotApi.Buttons;
using GameOfSkateBotApi.GameLogic;
using GameOfSkateBotApi.Webhook.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Game = GameOfSkateBotApi.GameLogic.Game;

namespace GameOfSkateBotApi.Webhook
{
	public class WebhookHandler
	{
		private ITelegramBotClient _botclient;
		private readonly Game _game;
		private readonly TelegramButtons _telegramButtons;

        public WebhookHandler(ITelegramBotClient botclient, Game game, TelegramButtons telegramButtons)
		{
			_botclient = botclient;
			_game = game;
			_telegramButtons = telegramButtons;

			_game.SubscribeOnEvents(GameEventHandler);
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
		{
			var handler = update.Type switch
			{
				
				UpdateType.Message => StartGame(update.Message, cancellationToken),
				_ => throw new MessageMissingException("I don't know what to do with that...")
			};

			//TODO: create Messaging service class that will inject here using Dependency Injection
			// and call it instead
			await _botclient.SendTextMessageAsync(
				chatId: update.Message!.Chat.Id,
				text: await handler
			);
		}

		private Task LogUnknownMessage(Update update)
		{
			return Task.CompletedTask;
		}

		private Task<string> StartGame(Message? message, CancellationToken cancellationToken)
			=> message?.Text switch
			{
				"/beginner" => _game.Start(message.Chat.Id, GameLogic.Entity.Difficulty.Easy),
				"/advanced" => _game.Start(message.Chat.Id, GameLogic.Entity.Difficulty.Advanced),
				"/pro" => _game.Start(message.Chat.Id, GameLogic.Entity.Difficulty.Hard),
				"/onlyAdvanced" => _game.Start(message.Chat.Id, GameLogic.Entity.Difficulty.OnlyAdvanced),
				"/onlyPro" => _game.Start(message.Chat.Id, GameLogic.Entity.Difficulty.OnlyPro),
                "/next" => _game.NextTrick(message.Chat.Id),
				"/end" => _game.End(message.Chat.Id),
				_ => Task.FromResult("Seems like it is not a command so fuck off.")
			};

        private async void GameEventHandler(GameEventType eventType, long chatId)
        {
            switch (eventType)
            {
                case GameEventType.GameEnded:
                    await _telegramButtons.ShowAddingLevelOrEndButtons(chatId);
                    break;

                case GameEventType.GameStarted:
                    await _telegramButtons.ShowStartButtons(chatId);
                    break;

                case GameEventType.NextTrick:
                    await _telegramButtons.ShowNextOrEndButtons(chatId);
                    break;
            }
        }
    }
}