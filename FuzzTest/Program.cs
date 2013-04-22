using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using WatiN.Core;

namespace FuzzTest
{
    class Program
    {
        static private readonly Random rnd = new Random(Environment.TickCount);

        static private readonly List<string> _deadEnds = new List<string>();
        private static int MaxNumberOfCalls = 50;
        private static int MaxNumberOfRuns = 50;


        [STAThread]
        private static void Main()
        {
            TextFieldExtended.Register();


            for (int i = 0; i < MaxNumberOfRuns; i++)
            {
                var sw = Stopwatch.StartNew();

                //var url = "http://localhost:1662";
                //var url = "http://mongo.learninglineapp.com";
                //var url = "http://angularjstest.azurewebsites.net/";
                //var url = "http://dotnetevents.nl/";
                //var url = "http://www.windowsworkflowfoundation.eu/";
                //var url = "http://wiki.windowsworkflowfoundation.eu/";
                var url = "http://ravendbtest.azurewebsites.net/";

                var browser = new IE(url);
                {
                    var stack = new StringBuilder();
                    ExecuteAction(browser, 0, stack);
                }
                try
                {
                    browser.Dispose();
                }
                catch (Exception)
                {
                }
                sw.Stop();
                Console.WriteLine("Test ran for {0}", sw.Elapsed);
                Console.WriteLine();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Console.ReadLine();
        }

        private static void ExecuteAction(Browser browser, int callNumber, StringBuilder stack)
        {
            var actions = FindAllActions(browser);
            var posibleActions = actions.Where(fa => fa.CanExecute()).ToArray();

            if (posibleActions.Any())
            {
                var allActions = new List<FuzzyAction>();
                foreach (var posibleAction in posibleActions)
                {
                    for (int i = 0; i < posibleAction.Weight; i++)
                    {
                        allActions.Add(posibleAction);
                    }
                }

                var index = rnd.Next(allActions.Count);
                var action = allActions[index];
                stack.AppendLine(action.StackId);

                if (!_deadEnds.Contains(stack.ToString()))
                {
                    try
                    {
                        action.Execute();

                        if (
                            browser.ContainsText(
                                "<span><H1>Server Error in '/' Application.<hr width=100% size=1 color=silver></H1>"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("**** Server error *****");
                            Console.WriteLine(stack);
                            _deadEnds.Add(stack.ToString());
                            Console.ResetColor();
                        }
                        else
                        {
                            if (callNumber < MaxNumberOfCalls)
                            {
                                var newStack = new StringBuilder(stack.ToString());
                                ExecuteAction(browser, callNumber + 1, newStack);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("**** Error: {0} ***** ", ex.Message);
                        Console.WriteLine(stack);
                        _deadEnds.Add(stack.ToString());
                        Console.ResetColor();

                        Process.GetProcessById(browser.ProcessID).Kill();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("**** Nothing more to execute *****");
                Console.WriteLine(stack);
                _deadEnds.Add(stack.ToString());
                Console.ResetColor();
            }
        }

        private static IEnumerable<FuzzyAction> FindAllActions(Browser browser)
        {
            var actions = new List<FuzzyAction>();

            var factories = from asm in AppDomain.CurrentDomain.GetAssemblies()
                            from t in asm.GetTypes()
                            where typeof(IFuzzyActionFactory).IsAssignableFrom(t)
                            && t.IsClass
                            && !t.IsAbstract
                            select (IFuzzyActionFactory)Activator.CreateInstance(t);


            foreach (var factory in factories)
            {
                factory.Register(browser, actions);
            }

            //var x = browser.ElementsOfType<TextFieldExtended>();
            //foreach (var textFieldExtended in x)
            //{

            //}

            return actions;
        }
    }
}
