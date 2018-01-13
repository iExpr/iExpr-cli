using iExpr;
using iExpr.Evaluators;
using iExpr.Parser;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.IO;

namespace iExpr_cli
{
    class Program
    {
        static ParseEnvironment ep = null;
        static EvalEnvironment ev = null;
        static ExprBuilder eb = null;

        static IExpr eval(string s)
        {
            var e = eb.GetExpr(s);
            return ev.CreateContext().Evaluate(e);
        }

        static IExpr eval(string s, Dictionary<string, double> vals)
        {
            var e = eb.GetExpr(s);
            var c = ev.CreateContext().GetChild();
            foreach (var v in vals)
            {
                c.Variables.Add(v.Key, new ConcreteValue(v.Value));
            }
            return c.Evaluate(e);
        }

        static void Main(string[] args)
        {
            ep = new iExpr.Exprs.Program.EParse();
            ev = new iExpr.Exprs.Program.EEval();
            eb = new ExprBuilder(ep);
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write("> ");
                string s = Console.ReadLine();
                if (s == "exit") return;
                if (s == "cls") { Console.Clear(); continue; }
                if (s == "view")
                {
                    foreach(var v in ev.Variables)
                    {
                        Console.Write(v.Key + ",");
                    }
                    Console.WriteLine();
                    continue;
                }
                if (s == "code")
                {
                    try
                    {
                        Console.Write("Input a file: ");
                        string path = Console.ReadLine();
                        if (File.Exists(path))
                        {
                            string code = File.ReadAllText(path);
                            var e = eb.GetExpr(code);
                            var v= ev.CreateContext().Evaluate(e);
                            Console.WriteLine(v);
                        }
                        else Console.WriteLine("File not found.");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("Error: " + ex.Message);
                    }
                    continue;
                }
                try
                {
                    Console.WriteLine(eval(s).ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
