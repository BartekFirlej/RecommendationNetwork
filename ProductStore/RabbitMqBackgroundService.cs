using ProductStore.Services;
using ProductStore.DTOs;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly RabbitMqConsumer _rabbitMqConsumer;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMqBackgroundService(RabbitMqConsumer rabbitMqConsumer,
                                     IServiceScopeFactory scopeFactory)
    {
        _rabbitMqConsumer = rabbitMqConsumer;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _rabbitMqConsumer.MessageReceived += OnMessageReceived;

        await Task.Run(() => _rabbitMqConsumer.StartConsuming<PurchaseProposalRequest>("purchaseProposalQueue"), stoppingToken);
    }

    private async void OnMessageReceived(object sender, RabbitMqConsumer.GenericEventArgs e)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var purchaseProposalService = scope.ServiceProvider.GetRequiredService<IPurchaseProposalService>();

            try
            {
                if (e.Message is PurchaseProposalRequest purchaseProposalRequest)
                {
                    try
                    {
                        Console.WriteLine("Consuming PurchaseProposalRequest");
                        await purchaseProposalService.PostPurchaseProposal(purchaseProposalRequest);
                    }
                    catch
                    {
                        Console.WriteLine("Consuming PurchaseProposalRequest again");
                        await purchaseProposalService.PostPurchaseProposal(purchaseProposalRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _rabbitMqConsumer.MessageReceived -= OnMessageReceived;
        await base.StopAsync(stoppingToken);
    }
}

