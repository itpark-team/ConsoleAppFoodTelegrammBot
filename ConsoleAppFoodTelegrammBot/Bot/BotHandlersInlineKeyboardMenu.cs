using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlersInlineKeyboardMenu
{
    private BotLogicInlineKeyboardMenu _botLogicInlineKeyboardMenu;

    public BotHandlersInlineKeyboardMenu(BotLogicInlineKeyboardMenu botLogicInlineKeyboardMenu)
    {
        _botLogicInlineKeyboardMenu = botLogicInlineKeyboardMenu;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;

            if (requestMessageText == "/start")
            {
                await _botLogicInlineKeyboardMenu.FirstSendMainMenu(botClient, chatId, cancellationToken);
            }
        }
        else if (update.CallbackQuery != null)
        {
            ChatId chatId = update.CallbackQuery.Message.Chat.Id;
            int messageId = update.CallbackQuery.Message.MessageId;
            string callBackData = update.CallbackQuery.Data;

            await _botLogicInlineKeyboardMenu.CallMethodByName(callBackData, botClient, chatId, messageId,
                cancellationToken);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}