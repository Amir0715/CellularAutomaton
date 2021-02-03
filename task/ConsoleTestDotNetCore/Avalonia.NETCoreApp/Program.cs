using System;
using System.Drawing;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCManager;
using Microsoft.Extensions.Logging.Abstractions;

namespace Avalonia.NETCoreApp
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", isEnabled: true);
            using var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions()
            {
                Credentials = ChannelCredentials.Insecure,
                LoggerFactory = new NullLoggerFactory()
            });
            var client = new Greeter.GreeterClient(channel);
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            // обмениваемся сообщениями с сервером
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            Console.WriteLine("Ответ сервера: " + reply.Message);
            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}