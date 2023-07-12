using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Rabbit.Models;
using Rabbit.Models.Entities;

// See https://aka.ms/new-console-template for more information

var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "admin",
    Password = "123456"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "rabbitMensagensQueue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);

    RabbitMensagem mensagem = JsonSerializer.Deserialize<RabbitMensagem>(json); 

    System.Threading.Thread.Sleep(1000);

    Console.WriteLine($"Titulo: {mensagem.titulo}; Texto: {mensagem.texto}; Id: {mensagem.id}");
};
channel.BasicConsume(queue: "rabbitMensagensQueue",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
