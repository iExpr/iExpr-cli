using iExpr;
using iExpr.Exprs.Math;
using iExpr.Parser;
using iExpr.Values;
using System;
using System.Collections.Generic;

namespace iExpr_cli
{
    class Program
    {
        static EParse ep = new EParse();
        static EEval ev = new EEval();
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
            eb = new ExprBuilder(ep);
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write("> ");
                string s = Console.ReadLine();
                if (s == "exit") return;
                if (s == "cls") { Console.Clear(); continue; }
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
