using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using WatiN.Core;
using WatiN.Core.DialogHandlers;
using WatiN.Core.Native;

namespace FuzzTest
{
    class Program
    {
        static private readonly Random rnd = new Random(Environment.TickCount);

        static private readonly List<string> _deadEnds = new List<string>();

        [STAThread]
        private static void Main(string[] args)
        {
            TextFieldExtended.Register();


            for (int i = 0; i < 250; i++)
            {
                var sw = Stopwatch.StartNew();

                var browser = new IE("http://localhost:1662/Home/VariousControls");
                {
                    var stack = new StringBuilder();
                    ExecuteAction(browser, 0, stack);
                    //browser.TextField(Find.ByName("q")).TypeText("WatiN");
                    //browser.Button(Find.ByName("btnG")).Click();

                    //Assert.IsTrue(browser.ContainsText("WatiN"));
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

        private static void ExecuteAction(Browser browser, int nesting, StringBuilder stack)
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
                        else if (nesting < 25)
                        {
                            var newStack = new StringBuilder(stack.ToString());
                            ExecuteAction(browser, nesting + 1, newStack);
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

            var x = browser.ElementsOfType<TextFieldExtended>();
            foreach (var textFieldExtended in x)
            {
                
            }
            
            return actions;
        }
    }

    //[ElementTag("input", InputType = "text", Index = 0)]
    //[ElementTag("input", InputType = "password", Index = 1)]
    //[ElementTag("input", InputType = "textarea", Index = 2)]
    [ElementTag("input", InputType = "date", Index = 3)]
    [ElementTag("input", InputType = "datetime", Index = 4)]
    //[ElementTag("textarea", Index = 4)]
    [ElementTag("input", InputType = "email", Index = 5)]
    [ElementTag("input", InputType = "url", Index = 6)]
    [ElementTag("input", InputType = "number", Index = 7)]
    [ElementTag("input", InputType = "range", Index = 8)]
    [ElementTag("input", InputType = "search", Index = 9)]
    [ElementTag("input", InputType = "color", Index = 10)]
    public class TextFieldExtended : TextField
    {
        public TextFieldExtended(DomContainer domContainer, INativeElement element)
            : base(domContainer, element)
        {
        }

        public TextFieldExtended(DomContainer domContainer, ElementFinder finder)
            : base(domContainer, finder)
        {
        }

        public static void Register()
        {
            var typeToRegister = typeof(TextFieldExtended);
            ElementFactory.RegisterElementType(typeToRegister);
        }
    }
}
