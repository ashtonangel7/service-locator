namespace patterns
{
    public interface ILogger
    {
        void Log(string message);
    }

    public interface IEmailService
    {
        void SendEmail(string address, string subject, string body);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Console Message: {message}");
        }
    }

    public class EmailSender : IEmailService
    {
        public void SendEmail(string address, string subject, string body)
        {
            Console.WriteLine($"Sending Email: address ({address})\nsubject: {subject}\nbody: {body}");
        }
    }

    public class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public static void RegisterService<T>(T service)
        {
            if (service == null)
            {
                Console.WriteLine("Please provide a valid service");
                return;
            }
            _services.Add(typeof(T), service);
        }

        public static T? GetService<T>()
        {
            bool result = _services.TryGetValue(typeof(T), out var service);

            if (result == false || service == null)
            {
                Console.WriteLine("Could not retrun a sevice for type {typeof(T)}");
                return default;
            }

            return (T)service;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Register the services with the Service Locator
            ServiceLocator.RegisterService<ILogger>(new ConsoleLogger());
            ServiceLocator.RegisterService<IEmailService>(new EmailSender());

            // Now, let's use the services without directly referencing their concrete implementations
            ILogger? logger = ServiceLocator.GetService<ILogger>();

            if (logger == null)
            {
                Console.WriteLine("Could not locate ILogger service");
                return;
            }

            IEmailService? emailService = ServiceLocator.GetService<IEmailService>();
            if (emailService == null)
            {
                Console.WriteLine("Could not locate IEmailService service");
                return;
            }

            // Use the services
            logger.Log("This is a log message.");
            emailService.SendEmail("recipient@example.com", "Hello", "This is a test email.");
        }
    }
}
