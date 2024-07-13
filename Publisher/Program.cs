using System.Text;
using RabbitMQ.Client;

Task.Run(PublishMessage(10000, "error"));
//Task.Run(PublishMessage(12000, "warning"));

Console.ReadLine();

static Func<Task> PublishMessage(int timeToSleepTo, string routingKey)
{
    return () =>
    {
        var counter = 0;
        do
        {

            var timeToSleep = new Random().Next(1000, timeToSleepTo);
            Thread.Sleep(timeToSleep);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var chanel = connection.CreateModel();

            chanel.ExchangeDeclare(exchange: "direct_logs", ExchangeType.Direct);

            counter++;
            var message = $"message type [error] from publisher {counter}";
            var messageBytes = Encoding.UTF8.GetBytes(message);

            chanel.BasicPublish(exchange: "direct_logs",
                routingKey: routingKey,
                basicProperties: null,
                body: messageBytes);

            Console.WriteLine($"message type {routingKey} sent to direct exchange #{counter}");
        } while (true);
    };
}



