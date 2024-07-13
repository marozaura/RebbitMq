using System.Diagnostics.Metrics;
using System.Text;
using RabbitMQ.Client;




var counter = 0;
do
{
    var random = new Random();
    var routingKey = counter % 4 == 0
        ? "tesla.red.fast.ecological"
        : $"{GetResourse().Item1[random.Next(0, 3)]}.{GetResourse().Item2[random.Next(0, 3)]}";
    var timeToSleep = random.Next(1000, 5000);
    Thread.Sleep(timeToSleep);
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using var connection = factory.CreateConnection();
    using var chanel = connection.CreateModel();

    chanel.ExchangeDeclare(exchange: "topic_logs", ExchangeType.Topic);

    counter++;
    var message = $"message type [topic] from publisher {counter}";
    var messageBytes = Encoding.UTF8.GetBytes(message);

    chanel.BasicPublish(exchange: "topic_logs",
        routingKey: routingKey,
        basicProperties: null,
    body: messageBytes);

    Console.WriteLine($"message type {routingKey} sent to topic exchange #{counter}");
} while (true);





static (List<string>, List<string>) GetResourse()
{
    return (new List<string> { "BWM", "Audi", "Mersedec" },
        new List<string> { "White", "Red", "Green" });
}


