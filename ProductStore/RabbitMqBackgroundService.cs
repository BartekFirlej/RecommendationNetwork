using ProductStore.Services;
using ProductStore.DTOs;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly RabbitMqConsumer _rabbitMqConsumer;
    private readonly IPurchaseProposalService _purchaseProposalService;

    public RabbitMqBackgroundService(RabbitMqConsumer rabbitMqConsumer,
                                     IPurchaseProposalService purchaseProposalService)
    {
        _rabbitMqConsumer = rabbitMqConsumer;
        _purchaseProposalService = purchaseProposalService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _rabbitMqConsumer.MessageReceived += OnMessageReceived;

        await Task.Run(() => _rabbitMqConsumer.StartConsuming<PurchaseProposalRequest>("purchaseProposalQueue"), stoppingToken);

    }

    private async void OnMessageReceived(object sender, RabbitMqConsumer.GenericEventArgs e)
    {
        try
        {
            if (e.Message is PurchaseProposalRequest purchaseProposalRequest)
            {
                try
                {
                    Console.WriteLine("Consuming PurchaseProposalRequest");
                    await _purchaseProposalService.PostPurchaseProposal(purchaseProposalRequest);
                }
                catch
                {
                    Console.WriteLine("Consuming PurchaseProposalRequest again");
                    await _purchaseProposalService.PostPurchaseProposal(purchaseProposalRequest);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _rabbitMqConsumer.MessageReceived -= OnMessageReceived;
        await base.StopAsync(stoppingToken);
    }
}

