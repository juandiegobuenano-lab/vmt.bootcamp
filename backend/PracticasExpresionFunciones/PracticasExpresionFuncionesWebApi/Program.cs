
using PracticasExpresionFuncionesWebApi.Models;
using PracticasExpresionFuncionesWebApi.Repository;
using PracticasExpresionFuncionesWebApi.Services;

var service = new FunctionService();
var repo = new GenericRepository<User>();

bool salir = false;

while (!salir)
{
    Console.WriteLine("\n=== MENÚ ===");
    Console.WriteLine("1. Saludar");
    Console.WriteLine("2. Duplicar número");
    Console.WriteLine("3. Cuadrado (expresión)");
    Console.WriteLine("4. Crear usuario");
    Console.WriteLine("5. Listar usuarios");
    Console.WriteLine("6. Buscar usuario por edad");
    Console.WriteLine("0. Salir");

    Console.Write("Opción: ");
    var opcion = Console.ReadLine();

    try
    {
        switch (opcion)
        {
            case "1":
                Console.Write("Nombre: ");
                var nombre = Console.ReadLine();
                Console.WriteLine(service.Saludar(nombre!));
                break;

            case "2":
                Console.Write("Número: ");
                int num = int.Parse(Console.ReadLine()!);
                Console.WriteLine(service.Duplicar(num));
                break;

            case "3":
                Console.Write("Número: ");
                int n = int.Parse(Console.ReadLine()!);
                Console.WriteLine(service.Cuadrado(n));
                break;

            case "4":
                var user = new User();

                Console.Write("Id: ");
                user.Id = int.Parse(Console.ReadLine()!);

                Console.Write("Nombre: ");
                user.Name = Console.ReadLine()!;

                Console.Write("Edad: ");
                user.Age = int.Parse(Console.ReadLine()!);

                repo.Add(user);
                Console.WriteLine("Usuario agregado.");
                break;

            case "5":
                var users = repo.GetAll();
                foreach (var u in users)
                {
                    Console.WriteLine(service.ObtenerInfo(u));
                }
                break;

            case "6":
                Console.Write("Edad mínima: ");
                int edad = int.Parse(Console.ReadLine()!);

                var filtrados = repo.Find(x => x.Age >= edad);

                foreach (var u in filtrados)
                {
                    Console.WriteLine($"{u.Name} - {u.Age}");
                }
                break;

            case "0":
                salir = true;
                break;

            default:
                Console.WriteLine("Opción inválida");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}