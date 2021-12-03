using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Entities;
using RabbitConsumer.Storage;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitConsumer
{
    public class Consumer : BackgroundService
    {
        private readonly ILogger<Consumer> _logger;
        private Context _context;

        public Consumer(ILogger<Consumer> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<Context>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queueName = "testQ";

            ConnectionFactory factory = new ConnectionFactory()
            {
                DispatchConsumersAsync = true,
                HostName = "localhost",
                Port = 5672,
                UserName = "consumer",
                Password = "consumer",
                VirtualHost = "/"
            };

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;
            channel.BasicConsume(queueName, true, consumer);

            string consumerTag = channel.BasicConsume(queueName, false, consumer);
        }

        private async Task Consumer_Received(object ch, BasicDeliverEventArgs @ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"{DateTimeOffset.Now} Message received: {message}");

            Element element = new Element
            {
                Value = message
            };

            _context.Element.Add(element);

            await _context.SaveChangesAsync();
            await Task.Yield();
        }
    }
}