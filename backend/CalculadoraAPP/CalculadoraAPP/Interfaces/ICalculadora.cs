using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraAPP.Interfaces
{
    public interface ICalculadora
    {
        double Add(double a, double b);
        double Subtract(double a, double b);
        double Multiply(double a, double b);
        double Divide(double a, double b);

    }
}
