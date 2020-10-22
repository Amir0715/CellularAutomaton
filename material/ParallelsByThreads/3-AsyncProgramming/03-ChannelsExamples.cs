using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ParallelsByThreads
{
    public class ChannelsExamples
    {
        public static async Task BasicUsage()
        {
            var channel = Channel.CreateUnbounded<string>();

            var consumer = Task.Run(
                async () =>
                {
                    while (await channel.Reader.WaitToReadAsync())
                    {
                        "Consuming".Print();
                        var message = await channel.Reader.ReadAsync();
                        message.Print();
                    }
                }
            );

            var producer = Task.Run(
                async () =>
                {
                    var random = new Random();
                    for (var i = 0; i < 10; ++i)
                    {
                        "Producing".Print();
                        await Task.Delay(random.Next(10));
                        await channel.Writer.WriteAsync($"Message: {i}");
                    }
                    channel.Writer.Complete();
                }
            );

            await Task.WhenAll(consumer, producer);
        }
    }
}