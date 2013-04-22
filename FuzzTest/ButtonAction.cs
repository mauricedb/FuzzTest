using System;
using System.Collections.Generic;
using System.Threading;

using WatiN.Core;

namespace FuzzTest
{
    public class ButtonAction : FuzzyAction
    {
        private readonly Element _button;

        public ButtonAction(Element button)
            : base(button)
        {
            _button = button;
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
            if (_button.ClassName == "diagnoseButton" && _button.Id == "diagnose")
            {
                return false;
            }

            return base.CanExecute();
        }

        public override void Execute()
        {
            Console.WriteLine("Clicking '{0}'", _button);
            _button.Click();
            Thread.Sleep(500);
        }
    }

    public class ButtonActionFactory : IFuzzyActionFactory
    {
        public void Register(Browser browser, List<FuzzyAction> actions)
        {
            foreach (var button in browser.Buttons)
            {
                actions.Add(new ButtonAction(button));
            }

            var btns = browser.Elements.Filter(el => el.ClassName != null && el.ClassName.Contains("btn"));
            foreach (var btn in btns)
            {
                actions.Add(new ButtonAction(btn));
            }

            var clickable = browser.Elements.Filter(el => !string.IsNullOrEmpty(el.GetAttributeValue("onclick")));
            foreach (var btn in clickable)
            {
                actions.Add(new ButtonAction(btn));
            }

            var commands = browser.Elements.Filter(el => !string.IsNullOrEmpty(el.GetAttributeValue("command")));
            foreach (var btn in commands)
            {
                actions.Add(new ButtonAction(btn));
            }

            var x = browser.Elements.Filter(el => el.Id != null && el.Id.EndsWith("MainButton"));
            if (x.Count != 0)
            {
                var y = 0;
            }

            var frames = browser.Frames;
            if (frames.Count > 1)
            {
                var y = 0;
                
            }
        }
    }
}