using System.Collections.Generic;
using OpenQA.Selenium;

namespace FuzzTest
{
    public class RadioButtonAction : FuzzyAction
    {
        private readonly IWebElement _radioButton;

        public RadioButtonAction(IWebElement radioButton):base(radioButton)
        {
            _radioButton = radioButton;
        }

        public override void Execute()
        {
            _radioButton.Click();
        }
    }

    public class RadioButtonActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {
            foreach (var radioButton in browser.FindElements(By.TagName("input")))
            {
                if (radioButton.GetAttribute("type") == "radio")
                {
                    actions.Add(new RadioButtonAction(radioButton));
                }
            }
        }
    }
}