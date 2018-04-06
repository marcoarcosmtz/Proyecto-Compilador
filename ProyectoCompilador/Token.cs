using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCompilador
{
    class Token
    {
        public string nombre, valor, tokenType;
        public TokenType token_tipo;
        public int linea_codigo, index;
        public Token(string nombre, string valor, TokenType token_tipo, int linea_codigo, int index)
        {
            this.nombre = nombre;
            this.valor = valor;
            this.token_tipo = token_tipo;
            this.linea_codigo = linea_codigo;
        }
        public Token(string nombre, string tokenType, int linea_codigo, int index)
        {
            this.nombre = nombre;
            this.tokenType = tokenType;
            this.linea_codigo = linea_codigo;
            this.index = index;
        }
        public Token()
        {

        }

    }
    enum TokenType
    {
        comentario_simple = 1,
        comentario_multilinea = 2,
        comentario_inicio = 3,
        comentario_fin = 4,
        operador_comparasion = 5,
        operador_logico = 6,
        char_literal = 7,
        coma = 8,
        punto_coma = 9,
        punto = 10,
        operador_aritmetico = 11,
        asignacion = 12,
        identificador = 13,
        integer_literal = 14,
        agrupacion = 15,
        string_literal = 16,
        desconocido = 17,
        /*corchete_abierto =14,
        corchete_cerrado,
        parentesis_abierto,
        parentesis_cerrado,*/
        literal, operador, keyword,
        @break, loop, @else, @return, elseif, var, @if,
        parentesis_abierto, parentesis_cerrado,
        corchete_abierto, corchete_cerrado,
        llave_abierta, llave_cerrada,
        incremento, decremento,
        and, or,
        igual_igual, diferente, menor, menor_igual, mayor, mayor_igual,
        mas, menos, mult, div, mod, neg, dolar
    }
}
