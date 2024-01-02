using RecommendationNetwork.DTOs;
using RecommendationNetwork.Services;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly RabbitMqConsumer _rabbitMqConsumer;
    private readonly ICustomerService _customerService;

    public RabbitMqBackgroundService(RabbitMqConsumer rabbitMqConsumer, ICustomerService customerService)
    {
        _rabbitMqConsumer = rabbitMqConsumer;
        _customerService = customerService;  
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _rabbitMqConsumer.CustomerAdded += OnCustomerAdded;

        // Start the consumer in a separate thread to avoid blocking the main thread
        await Task.Run(() => _rabbitMqConsumer.StartConsuming("customerQueue"), stoppingToken);
        Console.WriteLine("START LISTENING");
    }

    private async void OnCustomerAdded(object sender, CustomerRequest customerToAdd)
    {
        // Handle the message
        // Asynchronously process the received customer request
        try
        {
            Console.WriteLine("CONSUMING");
            var response = await _customerService.AddCustomer(customerToAdd);
            // Handle the response, e.g., logging or further processing
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during processing
            // Log the exception, send a notification, etc.
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        // Perform cleanup, if necessary
        // E.g., unsubscribe from the event
        _rabbitMqConsumer.CustomerAdded -= OnCustomerAdded;

        await base.StopAsync(stoppingToken);
    }
}
