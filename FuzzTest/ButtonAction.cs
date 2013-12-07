using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace FuzzTest
{
    public class ButtonAction : FuzzyAction
    {
        private readonly IWebElement _button;
        private readonly string _className;
        private readonly string _id;

        public ButtonAction(IWebElement button)
            : base(button)
        {
            _button = button;
            _id = _button.GetAttribute("id");
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
            if (_className == "diagnoseButton" && _id == "diagnose")
            {
                return false;
            }

            return base.CanExecute();
        }

        public override void Execute()
        {
            var text = _button.Text;

            Console.WriteLine("Clicking '{0}'", text ?? _id);
            _button.Click();
            //Thread.Sleep(500);
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


            //var btns = browser.Elements.Filter(el => el.ClassName != null && el.ClassName.Contains("btn"));
            var btns = browser.FindElements(By.ClassName("btn"));
            foreach (var btn in btns)
            {
                actions.Add(new ButtonAction(btn));
            }

            //var clickable = browser.Elements.Filter(el => !string.IsNullOrEmpty(el.GetAttributeValue("onclick")));
            //foreach (var btn in clickable)
            //{
            //    actions.Add(new ButtonAction(btn));
            //}

            //var commands = browser.Elements.Filter(el => !string.IsNullOrEmpty(el.GetAttributeValue("command")));
            //foreach (var btn in commands)
            //{
            //    actions.Add(new ButtonAction(btn));
            //}

        }
    }
}