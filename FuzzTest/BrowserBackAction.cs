using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using WatiN.Core;

namespace FuzzTest
{
    public class BrowserBackAction : FuzzyAction
    {
        private IWebDriver _browser;

        public BrowserBackAction(IWebDriver browser)
            : base((IWebElement)null)
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
            _browser.Navigate().Back();
        }
    }

    public class BrowserActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {
            actions.Add(new BrowserBackAction(browser));
        }
    }

}