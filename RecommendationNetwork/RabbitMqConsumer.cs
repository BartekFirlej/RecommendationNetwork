using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RecommendationNetwork.DTOs;

public class RabbitMqConsumer
{
    private readonly IConnection _rabbitMQConnection;
    private IModel _channel;

    public event EventHandler<CustomerRequest> CustomerAdded;

    public RabbitMqConsumer(IConnection rabbitMqConnection)
    {
        _rabbitMQConnection = rabbitMqConnection ?? throw new ArgumentNullException(nameof(rabbitMqConnection));
        Initialize();
    }

    private void Initialize()
    {
        if (_rabbitMQConnection.IsOpen)
        {
            Console.WriteLine("RabbitMQ connection is open.");
            _channel = _rabbitMQConnection.CreateModel();
        }
        else
        {
            Console.WriteLine("RabbitMQ connection is NOT open.");
        }
    }

    public void StartConsuming(string queueName)
    {
        if (_channel == null)
        {
            Console.WriteLine("RabbitMQ channel is not initialized.");
            return;
        }

        try
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var customerRequest = JsonConvert.DeserializeObject<CustomerRequest>(message);
                    CustomerAdded?.Invoke(this, customerRequest);
                    _channel.BasicAck(args.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    // Optionally, handle the failure (e.g., logging, retrying, etc.)
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"Listening for messages on queue: {queueName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error consuming from RabbitMQ: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
    }

    public bool HasSubscriber(EventHandler<CustomerRequest> eventHandler)
    {
        if (eventHandler == null)
            return false;

        var subscribers = eventHandler.GetInvocationList();
        return subscribers.Length > 0;
    }
}
