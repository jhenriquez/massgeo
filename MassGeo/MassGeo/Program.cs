namespace MassGeo
{
    using System;
    using System.Configuration;
    using MassGeo.Domain;
    using MassTransit;

    class Program
    {
        static IServiceBus InboundServiceBus;
        static IServiceBus OutboundServiceBus;

        static void Main(string[] args)
        {
            InboundServiceBus = ServiceBusFactory.New(b =>
            {
                b.UseRabbitMq(r =>
                {
                    r.ConfigureHost(new Uri("rabbitmq://localhost/main/raw"),
                        u =>
                        {
                            u.SetUsername("guest");
                            u.SetPassword("guest");
                        });
                });
                b.ReceiveFrom(ConfigurationManager.AppSettings["RawQueue"]);
                b.Subscribe(cfg =>
                {
                    cfg.Handler<InboundMessage>(ProcessInboundMessage);
                });
            });

            OutboundServiceBus = ServiceBusFactory.New(b =>
            {
                b.UseRabbitMq(r =>
                {
                    r.ConfigureHost(new Uri(ConfigurationManager.AppSettings["ProcessedQueue"]),
                        u =>
                        {
                            u.SetUsername("guest");
                            u.SetPassword("guest");
                        });
                });
            });

            InboundServiceBus.Publish(new InboundMessage() { PageView = "/page", IpAddress = "10.0.7.7" });
        }

        static void ProcessInboundMessage(InboundMessage message)
        {
            Console.WriteLine("Message Received");
            Console.WriteLine(message.PageView);
        }
    }
}
