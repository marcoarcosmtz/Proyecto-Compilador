using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProyectoCompilador
{
    class Parser
    {
        string[,] grammar;
        string path;
        string[,] matriz;
        List<Nodo> listaNodos;
        public Parser(string path)
        {
            this.path = path;
            matriz = new string[390, 85];
            grammar = new string[87, 2];
            listaNodos = new List<Nodo>();
        }
        public Parser(string path, string[,] grammar)
        {
            this.path = path;
            this.grammar = grammar;
        }
        public void IniciaAnalisis(List<Token> tokens)
        {
            List<string[]> input = new List<string[]>();
            for (int i = 0; i < tokens.Count; i++)
                input.Add(new string[] { tokens[i].tokenType, tokens[i].nombre });
            input.Add(new string[] { "$", "$" });
            List<int> pilaEdos = new List<int>();
            pilaEdos.Add(0);
            List<string> pila = new List<string>();
            do
            {
                string accion = siguienteEdo(pilaEdos.Last(), input[0][0]);
                string act = accion.Substring(0, 1);
                string[] regla = new string[2];
                if (accion == "acc")
                {
                    Console.WriteLine("acc");
                    break;
                }
                if (act == "r")
                {
                    regla[0] = grammar[Convert.ToInt32(accion.Substring(1)), 0];
                    regla[1] = grammar[Convert.ToInt32(accion.Substring(1)), 1];
                    string[] reduccion = regla[1].Split(' ');
                    int j = pila.Count - 1;
                    List<string> pilaTemp = pila;
                    List<int> edoTemp = pilaEdos;
                    if (regla[1] != "''")
                    {
                        Nodo nodo = new Nodo(regla[0], regla[0]);
                        for (int i = reduccion.Length - 1; i >= 0; i--)
                        {
                            if (listaNodos[j].nombre == reduccion[i])
                            {
                                nodo.listaNodos.Insert(0, listaNodos[j]);
                                //nodo.listaNodos.Add();
                                listaNodos.RemoveAt(listaNodos.Count - 1);
                                j--;
                            }
                        }
                        listaNodos.Add(nodo);
                        j = pila.Count - 1;
                        for (int i = reduccion.Length; i > 0; i--)
                        {
                            if (reduccion[i - 1] == pila[j])
                            {
                                /*if (listaNodos[i - 1].valor == reduccion[i - 1])
                                {
                                    nodo.listaNodos.Add(listaNodos[i - 1]);
                                    
                                }
                                else
                                {
                                    nodo.listaNodos.Add(new Nodo(reduccion[i - 1]));
                                }*/
                                j--;
                                pilaTemp.RemoveAt(pilaTemp.Count - 1);
                                edoTemp.RemoveAt(edoTemp.Count - 1);
                            }
                            else
                            {
                                j = -1;
                                break;
                            }
                        }
                        // listaNodos.Add(nodo);
                        if (j >= 0)
                        {
                            pila = pilaTemp;
                            pilaEdos = edoTemp;
                        }
                    }
                    else
                    {
                        Nodo nodo = new Nodo(regla[0], "");
                        // nodo.listaNodos.Add(new Nodo(regla[1], regla[1]));
                        listaNodos.Add(nodo);
                    }
                    accion = siguienteEdo(pilaEdos.Last(), regla[0]);
                    pila.Add(regla[0]);
                    pilaEdos.Add(Convert.ToInt32(accion));
                }
                else if (act == "s")
                {
                    pila.Add(input[0][0]);
                    pilaEdos.Add(Convert.ToInt32(accion.Substring(1)));
                    Nodo nodo = new Nodo(input[0][0], input[0][1]);
                    listaNodos.Add(nodo);
                    input.RemoveAt(0);
                }
                else
                {
                    Console.WriteLine("Error");
                    return;
                }
                PrintPila(pilaEdos, pila);
            } while (true);
            printTree(listaNodos[0]);
        }
        public void printTree(Nodo nodo)
        {
            if (nodo.listaNodos.Count < 1)
            {
                Console.Write(nodo.valor + " ");
                return;
            }
            for (int i = 0; i < nodo.listaNodos.Count; i++)
            {
                printTree(nodo.listaNodos[i]);
            }
        }
        private void PrintPila(List<int> pilaEdos, List<string> pila)
        {
            Console.Write("{0} ", pilaEdos[0]);
            for (int i = 0; i < pila.Count; i++)
            {
                Console.Write("{0} {1} ", pila[i], pilaEdos[i + 1]);
            }
            Console.WriteLine();
        }
        private string siguienteEdo(int actual, string cadena)
        {
            return matriz[actual, GetEdo(cadena)];
        }
        public void SetGrammar(string path)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path, System.Text.Encoding.Default);
            string line;
            int counter = 0;
            while ((line = file.ReadLine()) != null)
            {
                string[] g = Regex.Split(line, "->");
                grammar[counter, 0] = g[0].Trim(' ');
                grammar[counter, 1] = g[1].TrimStart(' ');
                counter++;
            }
            file.Close();
            /*for(int i = 0; i < grammar.GetLength(0); i++)
            {
                Console.WriteLine("{0} -> {1}", grammar[i, 0], grammar[i, 1]);
            }*/
        }
        public void SetAutomata()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path, System.Text.Encoding.Default);
            string line;
            int counter = 0;

            while ((line = file.ReadLine()) != null)
            {
                if (counter > 0)
                {
                    string[] token = line.Split('\t');
                    for (int i = 1; i < token.Length; i++)
                    {
                        matriz[counter - 1, i - 1] = token[i];
                    }
                }
                counter++;
            }
            file.Close();
        }
        private int GetEdo(string token)
        {
            switch (token)
            {
                case "var":
                    return 0;
                case ";":
                    return 1;
                case ",":
                    return 2;
                case "(":
                    return 3;
                case ")":
                    return 4;
                case "{":
                    return 5;
                case "}":
                    return 6;
                case "=":
                    return 7;
                case "++":
                    return 8;
                case "--":
                    return 9;
                case "if":
                    return 10;
                case "elseif":
                    return 11;
                case "else":
                    return 12;
                case "loop":
                    return 13;
                case "break":
                    return 14;
                case "return":
                    return 15;
                case "||":
                    return 16;
                case "&&":
                    return 17;
                case "==":
                    return 18;
                case "!=":
                    return 19;
                case "<":
                    return 20;
                case "<=":
                    return 21;
                case ">":
                    return 22;
                case ">=":
                    return 23;
                case "+":
                    return 24;
                case "-":
                    return 25;
                case "*":
                    return 26;
                case "/":
                    return 27;
                case "%":
                    return 28;
                case "!":
                    return 29;
                case "[":
                    return 30;
                case "]":
                    return 31;
                case "int":
                    return 32;
                case "char":
                    return 33;
                case "str":
                    return 34;
                case "id":
                    return 35;
                case "$":
                    return 36;
                case "PROGRAM'":
                    return 37;
                case "PROGRAM":
                    return 38;
                case "DEF-LIST":
                    return 39;
                case "DEF":
                    return 40;
                case "VAR-DEF":
                    return 41;
                case "VAR-LIST":
                    return 42;
                case "ID-LIST":
                    return 43;
                case "ID-LIST-CONT":
                    return 44;
                case "FUN-DEF":
                    return 45;
                case "PARAM-LIST":
                    return 46;
                case "VAR-DEF-LIST":
                    return 47;
                case "STMT-LIST":
                    return 48;
                case "STMT":
                    return 49;
                case "STMT-ASSIGN":
                    return 50;
                case "STMT-INCR":
                    return 51;
                case "STMT-DECR":
                    return 52;
                case "STMT-FUN-CALL":
                    return 53;
                case "FUN-CALL":
                    return 54;
                case "EXPR-LIST":
                    return 55;
                case "EXPR-LIST-CONT":
                    return 56;
                case "STMT-IF":
                    return 57;
                case "ELSE-IF-LIST":
                    return 58;
                case "ELSE":
                    return 59;
                case "STMT-LOOP":
                    return 60;
                case "STMT-BREAK":
                    return 61;
                case "STMT-RETURN":
                    return 62;
                case "STMT-EMPTY":
                    return 63;
                case "EXPR":
                    return 64;
                case "EXPR-OR":
                    return 65;
                case "EXPR-AND":
                    return 66;
                case "EXPR-COMP":
                    return 67;
                case "OP-COMP":
                    return 68;
                case "EXPR-REL":
                    return 69;
                case "OP-REL":
                    return 70;
                case "EXPR-ADD":
                    return 71;
                case "OP-ADD":
                    return 72;
                case "EXPR-MUL":
                    return 73;
                case "OP-MUL":
                    return 74;
                case "EXPR-UNARY":
                    return 75;
                case "OP-UNARY":
                    return 76;
                case "EXPR-PRIMARY":
                    return 77;
                case "ARRAY":
                    return 78;
                case "LIT":
                    return 79;
                case "LIT-INT":
                    return 80;
                case "LIT-CHAR":
                    return 81;
                case "LIT-STR":
                    return 82;
                case "ID":
                    return 83;
                default:
                    return -1;
            }
        }
    }
}
