using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using BeetrootTgBot.Services;
using Telegram.Bot.Types;

namespace BeetrootTgBot.Controllers
{
    public class BotController : ControllerBase
    {
        private readonly ITelegramService _telegramService;

        public BotController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update, CancellationToken cancellationToken = default)
        {
            await _telegramService.HandleMessage(update, cancellationToken);
            return Ok();
        }
    }
}
