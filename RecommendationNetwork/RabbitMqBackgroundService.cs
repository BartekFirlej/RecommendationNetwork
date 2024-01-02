using RecommendationNetwork.DTOs;
using RecommendationNetwork.Services;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly RabbitMqConsumer _rabbitMqConsumer;
    private readonly ICustomerService _customerService;
    private readonly IVoivodeshipService _voivodeshipService;
    private readonly IProductService _productService;
    private readonly IProductTypeService _productTypeService;

    public RabbitMqBackgroundService(RabbitMqConsumer rabbitMqConsumer,
                                     ICustomerService customerService,
                                     IVoivodeshipService voivodeshipService, 
                                     IProductService productService,
                                     IProductTypeService productTypeService)
    {
        _rabbitMqConsumer = rabbitMqConsumer;
        _customerService = customerService;
        _voivodeshipService = voivodeshipService;
        _productService = productService;
        _productTypeService = productTypeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _rabbitMqConsumer.MessageReceived += OnMessageReceived;

        await Task.Run(() => _rabbitMqConsumer.StartConsuming<CustomerRequest>("customerQueue"), stoppingToken);
        await Task.Run(() => _rabbitMqConsumer.StartConsuming<VoivodeshipRequest>("voivodeshipQueue"), stoppingToken);
        await Task.Run(() => _rabbitMqConsumer.StartConsuming<ProductRequest>("productQueue"), stoppingToken);
        await Task.Run(() => _rabbitMqConsumer.StartConsuming<ProductTypeRequest>("productTypeQueue"), stoppingToken);

        Console.WriteLine("Started listening on customer, voivodeship product and product type queues");
    }

    private async void OnMessageReceived(object sender, RabbitMqConsumer.GenericEventArgs e)
    {
        try
        {
            if (e.Message is CustomerRequest customerRequest)
            {
                Console.WriteLine("Consuming CustomerRequest");
                var response = await _customerService.AddCustomer(customerRequest);
                // Handle CustomerRequest response
            }
            else if (e.Message is VoivodeshipRequest voivodeshipRequest)
            {
                var response = await _voivodeshipService.AddVoivodeship(voivodeshipRequest);
                Console.WriteLine("Consuming VoivodeshipRequest");
                // Handle VoivodeshipRequest
                // e.g., _voivodeshipService.SomeMethod(voivodeshipRequest);
            }
            else if (e.Message is ProductRequest productRequest)
            {
                var response = await _productService.AddProduct(productRequest);
                Console.WriteLine("Consuming ProductRequest");
                // Handle VoivodeshipRequest
                // e.g., _voivodeshipService.SomeMethod(voivodeshipRequest);
            }
            else if (e.Message is ProductTypeRequest productTypeRequest)
            {
                var response = await _productTypeService.AddProductType(productTypeRequest);
                Console.WriteLine("Consuming ProductTypeRequest");
                // Handle VoivodeshipRequest
                // e.g., _voivodeshipService.SomeMethod(voivodeshipRequest);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
            // Handle exceptions
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _rabbitMqConsumer.MessageReceived -= OnMessageReceived;
        await base.StopAsync(stoppingToken);
    }
}

