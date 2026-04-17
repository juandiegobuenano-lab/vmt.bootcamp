using PracticasExpresionFuncionesWebApi.Models;

namespace PracticasExpresionFuncionesWebApi.Services
{
    public class FunctionService
    {
        // 1. Función con tipos simples
        public string Saludar(string nombre)
        {
            return $"Hola {nombre}";
        }

        // Función con entero
        public int Duplicar(int numero)
        {
            return numero * 2;
        }

        // 2. Expresión (lambda)
        public Func<int, int> Cuadrado = x => x * x;

        // Expresión con modelo (clase)
        public Func<User, string> ObtenerInfo = user => $"{user.Name} tiene {user.Age} años";
    }
}
