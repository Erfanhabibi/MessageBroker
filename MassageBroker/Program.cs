using System;
using MessageBroker.Broker;
using MessageBroker.Producer;
using MessageBroker.Consumer;


namespace MessageBroker;

class Program
{
    static void Main(string[] args)
    {
        IMessageBroker broker = new Broker.MessageBroker();

        broker.LoadMessageFromFile("messages.txt");


        IProducer producer = new Producer.Producer(broker);
        IConsumer consumer = new Consumer.Consumer(broker);

        
        Task.Run(() => {RunProducer(producer); });

        Task.Run(() => { RunConsumer(consumer); });

        Console.ReadLine();
    }

    static void RunProducer(IProducer producer)
    {
        for (int i = 1; i <= 5; i++)
        {
            producer.Produce($"message {i}");
            Thread.Sleep(500);
        }
    }

    static void RunConsumer(IConsumer consumer)
    {
        Thread.Sleep(1000);
        while (true)
        {
            consumer.Consume();
            Thread.Sleep(1000);
        }
    }
}

