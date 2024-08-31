using GameOfSkateBotApi.GameLogic.Exceptions;
using GameOfSkateBotApi.Webhook.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GameOfSkateBotApi.Webhook
{
	[ApiController]
	[Route("bot/[controller]")]
	public class UpdateController : ControllerBase
	{
		private readonly WebhookHandler _webhookHandler;
		private readonly ITelegramBotClient _botClient;
		private readonly ILogger _logger;

		public UpdateController(WebhookHandler webhookHandler, ITelegramBotClient botClient, ILogger<UpdateController> logger)
		{
			this._webhookHandler = webhookHandler;
			this._botClient = botClient;
			this._logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> NewUpdateWebhook(Update update, CancellationToken cancellationToken)
		{
			try
			{
				await _webhookHandler.HandleUpdateAsync(update, cancellationToken);
			}
			catch (MessageMissingException mme)
			{
				//TODO: remove that because if message is missing then update.Message will be always null..
				await SendMessageToChat(update.Message?.Chat.Id, mme.Message);
			}
			catch (GameAlreadyStartedException gase)
			{
				await SendMessageToChat(update.Message?.Chat.Id, gase.Message);
			}
			catch (GameNotStartedException gnse)
			{
				await SendMessageToChat(update.Message?.Chat.Id, gnse.Message);
			}

			return Ok();
		}

		//TODO: create Messaging service class that will inject here using Dependency Injection
		// and call it instead
		private Task SendMessageToChat(long? chatId, string message)
		{
			if (chatId == null)
			{
				_logger.LogCritical($"Something wrong during message send. Chat Id is null! Original message: {message}");
				return Task.CompletedTask;
			}

			//TODO: do try catch here since user could block this bot and we are unable to send messages
			return _botClient.SendTextMessageAsync(
				chatId: chatId.Value,
				text: message
			);
		}
	}
}