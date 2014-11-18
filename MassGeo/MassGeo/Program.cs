namespace MassGeo
{
    using System;
    using System.Configuration;
    using MassGeo.Domain;
    using MassTransit;

    class Program
    {
        static void Main(string[] args)
        {
            Bus.Initialize(b =>
            {
                b.UseRabbitMq(r =>
                {
                    r.ConfigureHost(new Uri("rabbitmq://localhost:15672/main"),
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

            Bus.Instance.Publish(new InboundMessage() { PageView = "/page", IpAddress = "10.0.7.7" });
        }

        static void ProcessInboundMessage(InboundMessage message)
        {
            Console.WriteLine("Message Received");
            Console.WriteLine(message.PageView);
        }
    }
}
