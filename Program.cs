namespace JetBrainsResetTrialConsole;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "JetBrains Trial Reset";
        var products = new List<ProductItem>
        {
            new("IntelliJIdea"), new("CLion"), new("DataGrip"), new("GoLand"),
            new("PhpStorm"), new("PyCharm"), new("ReSharper"), new("Rider"),
            new("RubyMine"), new("WebStorm"), new("Datalore"), new("ReSharperC")
        };

        var resetter = new TrialResetter(msg =>
        {
            Console.ForegroundColor = msg.TextColor;
            Console.WriteLine(msg.Text);
            Console.ResetColor();
        });

        int currentIndex = 0;
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine($"  JetBrains Trial Reset Console - {Environment.OSVersion.Platform}");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Yukari/Asagi Ok: Gezin | Bosluk: Sec | Enter: Baslat | ESC: Cikis\n");
            Console.ResetColor();

            for (int i = 0; i < products.Count; i++)
            {
                if (i == currentIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string checkbox = products[i].IsSelected ? "[ X ]" : "[   ]";
                Console.WriteLine($" {checkbox} {products[i].Name,-15} ");
                Console.ResetColor();
            }

            Console.WriteLine("\n -> [A] Tumunu Sec / Kaldir");
            Console.WriteLine(" -> [Enter] Secilenleri Sifirla");

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    currentIndex = (currentIndex > 0) ? currentIndex - 1 : products.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    currentIndex = (currentIndex < products.Count - 1) ? currentIndex + 1 : 0;
                    break;
                case ConsoleKey.Spacebar:
                    products[currentIndex].IsSelected = !products[currentIndex].IsSelected;
                    break;
                case ConsoleKey.A:
                    bool targetState = products.Exists(p => !p.IsSelected);
                    foreach (var p in products) p.IsSelected = targetState;
                    break;
                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
                case ConsoleKey.Enter:
                    var selectedProducts = products.FindAll(p => p.IsSelected).ConvertAll(p => p.Name);
                    if (selectedProducts.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n[ ! ] Lutfen en az bir urun seciniz! Devam etmek icin bir tusa basin...");
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Sifirlama islemi baslatiliyor...\n");
                        Console.ResetColor();
                        await resetter.ResetProductsAsync(selectedProducts);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("\nIslem tamamlandi. Menuye donmek icin bir tusa basin...");
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    break;
            }
        }
    }
}

class ProductItem(string name)
{
    public string Name { get; } = name; 
    public bool IsSelected { get; set; }
}
