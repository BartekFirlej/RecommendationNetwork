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
                try
                {
                    Console.WriteLine("Consuming CustomerRequest");
                    await _customerService.AddCustomer(customerRequest);
                }
                catch
                {
                    Console.WriteLine("Consuming CustomerRequest again");
                    await _customerService.AddCustomer(customerRequest);
                }
            }
            else if (e.Message is VoivodeshipRequest voivodeshipRequest)
            {
                try
                {
                    Console.WriteLine("Consuming VoivodeshipRequest");
                    await _voivodeshipService.AddVoivodeship(voivodeshipRequest);
                }
                catch
                {
                    Console.WriteLine("Consuming VoivodeshipRequest again");
                    await _voivodeshipService.AddVoivodeship(voivodeshipRequest);
                }
            }
            else if (e.Message is ProductRequest productRequest)
            {
                try
                {
                    Console.WriteLine("Consuming ProductRequest");
                    await _productService.AddProduct(productRequest);
                }
                catch
                {
                    Console.WriteLine("Consuming ProductRequest again");
                    await _productService.AddProduct(productRequest);
                }
            }
            else if (e.Message is ProductTypeRequest productTypeRequest)
            {
                try
                {
                    Console.WriteLine("Consuming ProductTypeRequest");
                    await _productTypeService.AddProductType(productTypeRequest);
                }
                catch
                {
                    Console.WriteLine("Consuming ProductTypeRequest again");
                    await _productTypeService.AddProductType(productTypeRequest);
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

