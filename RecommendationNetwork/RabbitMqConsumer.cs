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

    // Generic event handler for any type of message
    public event EventHandler<GenericEventArgs> MessageReceived;

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

    public void StartConsuming<T>(string queueName) where T : class
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
                    var deserializedMessage = JsonConvert.DeserializeObject<T>(message);
                    MessageReceived?.Invoke(this, new GenericEventArgs { Message = deserializedMessage });
                    _channel.BasicAck(args.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
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

    // Generic event args class
    public class GenericEventArgs : EventArgs
    {
        public object Message { get; set; }
    }
}

