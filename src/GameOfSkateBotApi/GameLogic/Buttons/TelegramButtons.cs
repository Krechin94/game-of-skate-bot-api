using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameOfSkateBotApi.Buttons
{
    public class TelegramButtons
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramButtons(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        ReplyKeyboardMarkup startReplyKeyboard = new ReplyKeyboardMarkup(
                                   new List<KeyboardButton[]>()
                                   {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/beginner"),
                                            new KeyboardButton("/advanced"),
                                            new KeyboardButton("/pro"),
                                        },
                                   })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true,
        };

        ReplyKeyboardMarkup nextOrEndReplyKeyboard = new ReplyKeyboardMarkup(
                                   new List<KeyboardButton[]>()
                                   {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/next"),
                                            new KeyboardButton("/end"),
                                        },
                                   })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true,
        };

        ReplyKeyboardMarkup addingLevelOrEndReplyKeyboard = new ReplyKeyboardMarkup(
                                   new List<KeyboardButton[]>()
                                   {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/onlyAdvanced"),
                                            new KeyboardButton("/onlyPro"),
                                            new KeyboardButton("/end"),
                                        },
                                   })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true,
        };
        public async Task ShowStartButtons(long chatId)
        {
            await ShowButtons(chatId, startReplyKeyboard, "Choose level of tricks");
        }

        public async Task ShowNextOrEndButtons(long chatId)
        {
            await ShowButtons(chatId, nextOrEndReplyKeyboard, "Next trick");
        }

        public async Task ShowAddingLevelOrEndButtons(long chatId)
        {
            await ShowButtons(chatId, addingLevelOrEndReplyKeyboard, "Choose What to do next");
        }
        public async Task ShowButtons(long chatid, ReplyKeyboardMarkup replyKeyboard, string message)
        {
            try
            {

                await _botClient.SendTextMessageAsync(
                                        chatid, message!,
                                        replyMarkup: replyKeyboard);
            }
            catch
            {
                throw new Exception();
            }

        }
    }
}