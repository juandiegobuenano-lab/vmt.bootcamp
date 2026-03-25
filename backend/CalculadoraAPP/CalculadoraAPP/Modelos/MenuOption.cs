using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraAPP.Modelos
{
    public class MenuOption
    {
       public int Id { get; set; }
        public string Name { get; set; }

        public MenuOption(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
