
using GameOfSkateBotApi.BotSettings;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace GameOfSkateBotApi.Webhook
{
    public class WebhookService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TelegramBotSettings _token;
        private readonly ILogger<WebhookService> _logger;

        public WebhookService(ITelegramBotClient botClient, IOptions<TelegramBotSettings> token, ILogger<WebhookService> logger)
        {
            this._botClient = botClient;
            this._token = token.Value;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var me = await _botClient.GetMeAsync();
            try
            {
                await _botClient.SetWebhookAsync(
                    url: _token.WebhookAddress,
                    allowedUpdates: Array.Empty<UpdateType>(),
                    secretToken: "",
                    cancellationToken: cancellationToken);
            }
            catch (ApiRequestException are)
            {
                _logger.LogCritical(are, "Couldn't mint webhook to address:");
            }
        }
        

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing webhook");
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
