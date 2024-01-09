using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;


public class RabbitMqPublisher
{
    private readonly IConnection _rabbitMQConnection;

    public RabbitMqPublisher(IConnection rabbitMQConnection)
    {
        _rabbitMQConnection = rabbitMQConnection;
    }

    public void PublishMessage<T>(T message, string queueName)
    {
        try
        {
            using (var channel = _rabbitMQConnection.CreateModel())
            {
                // Declare the queue
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Serialize the message to JSON
                var serializedMessage = JsonConvert.SerializeObject(message);

                // Convert the serialized message to bytes
                var body = Encoding.UTF8.GetBytes(serializedMessage);

                // Publish the message to the queue
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions related to RabbitMQ publishing
            Console.WriteLine($"Error publishing message to RabbitMQ: {ex.Message}");
        }
    }
}


