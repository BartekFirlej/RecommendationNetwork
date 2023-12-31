using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RecommendationNetwork.DTOs;

public class RabbitMqCustomerConsumer
{
    private readonly IConnection _rabbitMQConnection;

    public event EventHandler<CustomerRequest> CustomerAdded;

    public RabbitMqCustomerConsumer(IConnection rabbitMqConnection)
    {
        _rabbitMQConnection = rabbitMqConnection;

        if (_rabbitMQConnection.IsOpen)
        {
            Console.WriteLine("RabbitMQ connection is open.");
        }
        else
        {
            Console.WriteLine("RabbitMQ connection is NOT open.");
        }
    }


    public void StartConsuming(string queueName)
    {
        try
        {
            using (var channel = _rabbitMQConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, args) =>
                {
                    Console.WriteLine($"Received message from queue: {queueName}");
                    // Process the received message
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Deserialize the message to the appropriate type
                    var customerRequest = JsonConvert.DeserializeObject<CustomerRequest>(message);

                    // Raise the event to notify subscribers (controllers)
                    CustomerAdded?.Invoke(this, customerRequest);

                    // Acknowledge the message to RabbitMQ
                    channel.BasicAck(args.DeliveryTag, multiple: false);
                };

                // Start consuming messages from the queue
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine($"Listening for messages on queue: {queueName}");
                //Console.ReadLine(); // Keep the application running to continue listening
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions related to RabbitMQ consuming
            //Console.WriteLine($"Error consuming from RabbitMQ: {ex.Message}");
        }
    }

    public bool HasSubscriber(EventHandler<CustomerRequest> eventHandler)
    {
        if (eventHandler == null)
            return false;

        var subscribers = eventHandler.GetInvocationList();
        return subscribers.Length > 0;
    }
}
