using GameOfSkateBotApi.BotSettings;
using GameOfSkateBotApi.Buttons;
using GameOfSkateBotApi.GameLogic;
using GameOfSkateBotApi.Webhook;
using Telegram.Bot;

namespace GameOfSkateBotApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Configuration.AddEnvironmentVariables();
			builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisGameStorage");
            });

            var token = builder.Configuration.GetSection(nameof(TelegramBotSettings));
			var telegramToken = token.Get<TelegramBotSettings>()
			?? throw new Exception("отсутствует токен");

			builder.Services.Configure<TelegramBotSettings>(token);
			builder.Services.AddHttpClient("telegram-bot-client")
				.AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
				{
					TelegramBotClientOptions options = new(telegramToken.TelegramToken);
					return new TelegramBotClient(options, httpClient);
				});

			builder.Services.AddHostedService<WebhookService>();
			//TODO: change to Scoped when redis will be introduced
			builder.Services.AddScoped<Tricks>();
			builder.Services.AddScoped<Game>();
			builder.Services.AddScoped<TelegramButtons>();
			builder.Services.AddScoped<WebhookHandler>();

			//TODO: add default global exception handling so app wont die on exception

			var app = builder.Build();

			app.MapControllers();

			app.Run();
		}
	}
}
