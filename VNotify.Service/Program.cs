namespace VNotify.Service
{
    class Program
    {
        static void Main()
        {
            var service = new BackgroundService();

            while (service.IsAlive)
            {
                service.DoMessageLoop();
            }
        }
    }
}
