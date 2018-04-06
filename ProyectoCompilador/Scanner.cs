using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProyectoCompilador
{
    class Scanner
    {
        string pattern;
        string path;
        int[,] matriz;
        string[] keywords;
        public List<Token> tokens;
        public bool Success;
        public Scanner(string path)
        {
            matriz = new int[333, 256];
            this.path = path;
            keywords = new string[] { "break", "else", "elseif", "if", "loop", "loop", "return", "var" };
            tokens = new List<Token>();
            Success = true;
        }
        public void InitScanner()
        {
            Matriz matriz = new Matriz();
            matriz.SetMatriz();
            // string codigo = System.IO.File.ReadAllText(path, Encoding.Default) + "\t";
            System.IO.StreamReader file = new System.IO.StreamReader(path, Encoding.Default);
            int edo_actual = 0, edo_anterior = 0, linea = 1, index = 0;// , code = 0;
            string string_token = "", codigo;
            while ((codigo = file.ReadLine()) != null)
            {
                codigo += '\n';
                for (int i = 0; i < codigo.Length; i++)
                {
                    char char_actual = codigo[i];
                    edo_actual = matriz.SiguienteEstado(edo_actual, char_actual);
                    if (edo_actual == -1)
                    {
                        string tokenType = matriz.GetTokenType(edo_anterior);
                        if (tokenType == "id")
                        {
                            int indice = Array.BinarySearch(keywords, string_token);
                            if (indice >= 0)
                                tokenType = keywords[indice];
                        }
                        else if (tokenType == "desc")
                            Success = false;
                        if (tokenType != "comm")
                        {
                            tokens.Add(new Token(string_token, tokenType, linea, i));
                            if (tokens.Last().tokenType == "desc")
                                Console.WriteLine("ERROR - Token desconocido ({0}) {1}:{2}", tokens.Last().nombre, tokens.Last().linea_codigo, tokens.Last().index);
                            else
                                Console.WriteLine("Token {0} {2}:{3}", tokens.Last().nombre, tokens.Last().tokenType, tokens.Last().linea_codigo, tokens.Last().index);
                        }
                        edo_actual = 0;
                        edo_anterior = edo_actual;
                        string_token = "";
                        i--;
                        // code = 0;
                    }
                    else if (edo_actual != 0)
                    {
                        edo_anterior = edo_actual;
                        string_token += char_actual;
                        /*index = index == 0 ? i : index;
                        if (code == 0) code = i;*/
                    }
                    index = i;
                }
                linea++;
            }
            if (edo_actual == 41 || edo_actual == 42)
            {
                tokens.Add(new Token(string_token, "desc", linea, index));
                Success = false;
            }
            file.Close();
            /*for (int i = 0; i < codigo.Length; i++)
            {
                char char_actual = codigo[i];
                if (char_actual == '\n')
                {
                    linea++;
                    index = 0;
                }
                edo_actual = matriz.SiguienteEstado(edo_actual, char_actual);
                if (edo_actual == -1)
                {
                    string tokenType = matriz.GetTokenType(edo_anterior);
                    if (tokenType == "id")
                    {
                        int indice = Array.BinarySearch(keywords, tokenType);
                        if (indice >= 0)
                            tokenType = keywords[indice];
                        
                    }
                    tokens.Add(new Token(string_token, tokenType, linea, index - string_token.Length));
                    // Console.WriteLine("{0} {1} {2}", string_token, edo_anterior, tokenType);
                    edo_actual = 0;
                    edo_anterior = edo_actual;
                    string_token = "";
                    i--;
                }
                else if (edo_actual != 0)
                {
                    edo_anterior = edo_actual;
                    string_token += char_actual;
                    index++;
                }
            }*/
        }
        public Scanner(string @pattern, string path)
        {
            this.pattern = System.IO.File.ReadAllText(pattern);
            this.path = path;
        }

        static int GetType(GroupCollection groupCollection)
        {
            int r = 0;
            for (int i = 1; i < groupCollection.Count; i++)
            {
                if (groupCollection[i].Value != "")
                {
                    r = i;
                }
            }
            return r;
        }
        private TokenType SetTokenType(TokenType tokenType, string valor)
        {
            switch (tokenType)
            {
                case TokenType.identificador:
                    switch (valor)
                    {
                        case "var":
                            return TokenType.var;
                        case "if":
                            return TokenType.@if;
                        case "elseif":
                            return TokenType.elseif;
                        case "else":
                            return TokenType.@else;
                        case "loop":
                            return TokenType.loop;
                        case "break":
                            return TokenType.@break;
                        case "return":
                            return TokenType.@return;
                        default:
                            break;
                    }
                    break;
                case TokenType.operador_logico:
                    switch (valor)
                    {
                        case "!":
                            return TokenType.neg;
                        case "&&":
                            return TokenType.and;
                        case "||":
                            return TokenType.or;
                        default:
                            break;
                    }
                    break;
                case TokenType.operador_aritmetico:
                    switch (valor)
                    {
                        case "+":
                            return TokenType.mas;
                        case "-":
                            return TokenType.menos;
                        case "*":
                            return TokenType.mult;
                        case "/":
                            return TokenType.div;
                        case "%":
                            return TokenType.mod;
                        case "++":
                            return TokenType.incremento;
                        case "--":
                            return TokenType.decremento;
                        default:
                            break;
                    }
                    break;
                case TokenType.agrupacion:
                    switch (valor)
                    {
                        case "(":
                            return TokenType.parentesis_abierto;
                        case ")":
                            return TokenType.parentesis_cerrado;
                        case "[":
                            return TokenType.corchete_abierto;
                        case "]":
                            return TokenType.corchete_cerrado;
                        case "{":
                            return TokenType.llave_abierta;
                        case "}":
                            return TokenType.llave_cerrada;
                        default:
                            break;
                    }
                    break;
                case TokenType.operador_comparasion:
                    switch (valor)
                    {
                        case "==":
                            return TokenType.igual_igual;
                        case "!=":
                            return TokenType.diferente;
                        case "<":
                            return TokenType.menor;
                        case "<=":
                            return TokenType.menor_igual;
                        case ">":
                            return TokenType.mayor;
                        case ">=":
                            return TokenType.mayor_igual;
                        default:
                            break;
                    }
                    break;
                default:
                    return tokenType;
            }
            return tokenType;
        }
        public List<Token> GetTokens()
        {
            List<Token> tablaTokens = new List<Token>();
            System.IO.StreamReader file = new System.IO.StreamReader(path, System.Text.Encoding.Default);
            string line;
            int counter = 0;
            while ((line = file.ReadLine()) != null)
            {
                Regex regex = new Regex(pattern);
                foreach (Match match in Regex.Matches(line, pattern, RegexOptions.IgnoreCase))
                {
                    string cadena = match.Value;
                    // Token token_anterior = tablaTokens.Count > 0 ? tablaTokens.Last().Count > 0 ? tablaTokens.Last().Last() : null : null;
                    Token token_anterior = tablaTokens.Count > 0 ? tablaTokens.Last() : null;
                    TokenType TokenType = SetTokenType((TokenType)GetType(match.Groups), cadena);
                    Token token = new Token(null, cadena, TokenType, counter, match.Index);
                    if (token_anterior == null)
                    {
                        tablaTokens.Add(token);
                    }
                    else
                    {
                        if (token.token_tipo == TokenType.comentario_fin && token_anterior.token_tipo == TokenType.comentario_inicio)
                        {
                            tablaTokens.Add(token);
                        }
                        else if (token_anterior.token_tipo == TokenType.comentario_inicio)
                        {
                        }
                        /*else if (token_anterior.token_tipo == TokenType.COMENTARIO_FIN)
                        {
                            Console.WriteLine("Error");
                        }*/
                        else
                        {
                            tablaTokens.Add(token);
                        }
                    }
                    // Console.Write("{0} ", token.token_tipo);
                }
                counter++;
                // Console.WriteLine();
            }
            file.Close();
            return tablaTokens;
        }
    }
}
