using System;
using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public class BrowserBackAction : FuzzyAction
    {
        private Browser _browser;

        public BrowserBackAction(Browser browser):base(null)
        {
            _browser = browser;
        }

        public override string StackId
        {
            get
            {
                return "BrowserBack";
            }
        }
        public override bool CanExecute()
        {
            return true;
        }

        public override void Execute()
        {
            Console.WriteLine("Clicking the browser Back button.");
            _browser.Back();
        }
    }

    public class BrowserActionFactory : IFuzzyActionFactory
    {
        public void Register(Browser browser, List<FuzzyAction> actions)
        {
            actions.Add(new BrowserBackAction(browser));
        }
    }

}