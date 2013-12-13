using System;
using System.Collections.Generic;
using System.Threading;

using OpenQA.Selenium;

namespace FuzzTest
{
    public class ButtonAction : FuzzyAction
    {
        private readonly IWebElement _button;
        private readonly string _className;

        public ButtonAction(IWebElement button)
            : base(button)
        {
            _button = button;
            _className = _button.GetAttribute("class");
        }


        public override int Weight
        {
            get
            {
                return 10;
            }
        }

        public override bool CanExecute()
        {
            if (_className == "diagnoseButton" && Id == "diagnose")
            {
                return false;
            }

            return base.CanExecute();
        }

        public override void Execute()
        {
            var text = _button.Text;

            Console.WriteLine("Clicking '{0}'", text ?? Id);
            _button.Click();
        }
    }

    public class ButtonActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {

            foreach (var button in browser.FindElements(By.TagName("button")))
            {
                actions.Add(new ButtonAction(button));
            }

            var btns = browser.FindElements(By.ClassName("btn"));
            foreach (var btn in btns)
            {
                actions.Add(new ButtonAction(btn));
            }

            var clickable = browser.FindElements(By.CssSelector("[onclick]"));
            foreach (var btn in clickable)
            {
                actions.Add(new ButtonAction(btn));
            }

            var commands = browser.FindElements(By.CssSelector("[command]"));
            foreach (var btn in commands)
            {
                actions.Add(new ButtonAction(btn));
            }

        }
    }
}