using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BeetrootTgBot.Services
{
    public class InitService : IHostedService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BotConfiguration _botConfiguration;

        public InitService(ITelegramBotClient telegramBotClient, BotConfiguration botConfiguration)
        {
            _telegramBotClient = telegramBotClient;
            _botConfiguration = botConfiguration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var domain = _botConfiguration.Domain;
            var token = _botConfiguration.BotAccessToken;

            return _telegramBotClient.SetWebhookAsync($"{domain}/bot/{token}",
                allowedUpdates: Enumerable.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
