using MediatR;
using Microsoft.Extensions.Options;
using MyLab.EmailManager.App.Features.SendPendingMessages;

namespace MyLab.EmailManager.BackgroundSending
{
    public class BgSendingService(IServiceProvider serviceProvider, IOptions<EmailManagerOptions> opts) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var pTimer = new PeriodicTimer
                (
                    TimeSpan.FromSeconds(opts.Value.PendingMsgScanPeriodSec)
                );

            while (await pTimer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SendPendingMessagesCommand(), stoppingToken);
            }

        }
    }
}
