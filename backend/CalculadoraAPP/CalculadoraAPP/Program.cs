using CalculadoraAPP.Modelos;
using CalculadoraAPP.Servicios;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new CalculadoraServicie();

        var menu = new List<MenuOption>()
        {
            new MenuOption(1, "Sumar"),
            new MenuOption(2, "Restar"),
            new MenuOption(3, "Multiplicar"),
            new MenuOption(4, "Dividir"),
            new MenuOption(5, "Salir")
        };

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("===== CALCULADORA =====");

            foreach (var option in menu)
            {
                Console.WriteLine($"{option.Id}. {option.Name}");
            }

            Console.Write("Seleccione una opción: ");

            try
            {
                int choice = int.Parse(Console.ReadLine());

                if (choice == 5)
                {
                    running = false;
                    break;
                }

                Console.Write("Ingrese el primer número: ");
                double num1 = double.Parse(Console.ReadLine());

                Console.Write("Ingrese el segundo número: ");
                double num2 = double.Parse(Console.ReadLine());

                double result = 0;

                switch (choice)
                {
                    case 1:
                        result = calculator.Add(num1, num2);
                        break;

                    case 2:
                        result = calculator.Subtract(num1, num2);
                        break;

                    case 3:
                        result = calculator.Multiply(num1, num2);
                        break;

                    case 4:
                        result = calculator.Divide(num1, num2);
                        break;

                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }

                Console.WriteLine($"Resultado: {result}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Debes ingresar números válidos.");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}

