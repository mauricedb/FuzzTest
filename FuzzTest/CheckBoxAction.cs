using System.Collections.Generic;
using OpenQA.Selenium;

namespace FuzzTest
{
    public class CheckBoxAction : FuzzyAction
    {
        private readonly IWebElement _checkBox;

        public CheckBoxAction(IWebElement checkBox)
            : base(checkBox)
        {
            _checkBox = checkBox;
        }

        public override void Execute()
        {
            _checkBox.Click();
        }
    }

    public class CheckBoxActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {
            foreach (var checkBox in browser.FindElements(By.TagName("input")))
            {
                if (checkBox.GetAttribute("type") == "checkbox")
                {
                    actions.Add(new CheckBoxAction(checkBox));
                }
            }
        }
    }
}
