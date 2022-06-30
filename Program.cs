using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;

var botClient = new TelegramBotClient("5502239227:AAEM4jqaQObi24BJs2jqlwkSlFXjdYPehBo");

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var text = messageText;
    // var value = ;
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text
    if (text == "/start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Bizning oddiy simple botimizga 🤖 hush kelibsiz oddiy matematik amallarni bajarib beraman faqat (kop * , bol /)",
                cancellationToken: cancellationToken);
        await botClient.SendStickerAsync(
            chatId: chatId,
            sticker: "https://cdn.vectorstock.com/i/1000x1000/50/76/male-teacher-vector-33975076.webp",
            cancellationToken: cancellationToken);        
    }
    else if(char.IsDigit(text[0]) || text[0] =='('  )
    {
             Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"{Postfix.CountExp(text)}",
                cancellationToken: cancellationToken);

    }
}
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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