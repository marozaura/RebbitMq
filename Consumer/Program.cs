using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var chanel = connection.CreateModel();

chanel.ExchangeDeclare(exchange: "direct_logs", ExchangeType.Direct);

var queueName = chanel.QueueDeclare().QueueName;

chanel.QueueBind(queueName, "direct_logs", "error");

var consumer  = new EventingBasicConsumer(chanel);

consumer.Received += delegate(object? sender, BasicDeliverEventArgs eventArgs)
{
    var body = eventArgs.Body;

    var message = Encoding.UTF8.GetString(body.ToArray());

    Console.WriteLine($"{message}");
};

chanel.BasicConsume(queue: queueName,
    autoAck:true,
    consumer: consumer);

Console.WriteLine($"Subscribe queue: {queueName}");
Console.ReadLine();