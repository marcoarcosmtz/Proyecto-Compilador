using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCompilador
{
    class Nodo
    {
        public string nombre, valor;
        public List<Nodo> listaNodos;
        public Nodo(string nombre, string valor)
        {
            this.nombre = nombre;
            this.valor = valor;
            listaNodos = new List<Nodo>();
            //nodos = new List<Nodo>();
        }
    }
}
