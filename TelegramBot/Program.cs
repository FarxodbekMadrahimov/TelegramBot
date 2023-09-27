
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;

string Token = "6336261015:AAHQCPTv2LyDejggpqJTQP0PyribsTUTNUs";
var botClient = new TelegramBotClient(Token);
var my_bot = await botClient.GetMeAsync();
using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
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
    var error = "bu instagram link emas!";
    var welocome = "Assalomu alaykum bu bot Instagramdan video yuklovchi bot " +
        "(ammo rasmlarni hali yuklay olmaydi!!!)";
    var true_link = "?igshid=MzRlODBiNWFlZA==";




    if (messageText.Length < 50)
    {
        messageText = $"{messageText + true_link}";
    }



    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    bool containsInstagramUrl = ContainsInstagramUrl(messageText);
    bool isvalid = IsValidInstagramUrl(messageText);
    if(messageText == "/start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: welocome,
        cancellationToken: cancellationToken);

    }

    else if(!containsInstagramUrl)
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: error,
        cancellationToken: cancellationToken);
        

    }
     else if(isvalid )    
    {
           
            


        Message ms = await botClient.SendPhotoAsync(
        chatId: chatId,
        photo: $"{ReplaceWwwWithDd(messageText)}",
        caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
        parseMode: ParseMode.Html,
        cancellationToken: cancellationToken);



    }
    
    else
    {
        Message msg = await botClient.SendVideoAsync(
        chatId: chatId,
        video: $"{ReplaceWwwWithDd(messageText)}",
        supportsStreaming: true,
        cancellationToken: cancellationToken);
        

    }

    Console.WriteLine(messageText);



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
static string ReplaceWwwWithDd(string input)
{
    // Replace "www." with "dd"
    string result = Regex.Replace(input, @"www\.", "dd");

    // Replace "utm_source" with "igshid"
    result = Regex.Replace(result, "utm_source", "igshid");

    // Replace "ig_web_copy_link" with "MzRlODBiNWFlZA=="
    result = Regex.Replace(result, "ig_web_copy_link", "MzRlODBiNWFlZA==");

    return result;

    
}
static string ReplaceWwwWithDds(string input)
{
    string pattern = @"www\.";
    string replacement = "dd";
    return Regex.Replace(input, pattern, replacement);
}
static bool ContainsInstagramUrl(string input)
{
    string pattern = @"https://www\.instagram\.com";
    return Regex.IsMatch(input, pattern);
}
static bool IsValidInstagramUrl(string url)
{
    string pattern = @"https://www\.instagram\.com/p/";
    return Regex.IsMatch(url, pattern);
}
static string ReplaceUtmSourceWithIgshid(string input)
    {
        string pattern = @"utm_source";
        string replacement = "igshid";

        // Replace "utm_source" with "igshid" wherever it exists
        string result = Regex.Replace(input, pattern, replacement);

        return result;
    }

