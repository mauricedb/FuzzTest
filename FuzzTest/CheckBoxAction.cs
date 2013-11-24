using System.Collections.Generic;
using OpenQA.Selenium;
using WatiN.Core;

namespace FuzzTest
{
    public class CheckBoxAction : FuzzyAction
    {
        private readonly CheckBox _checkBox;

        public CheckBoxAction(CheckBox checkBox):base(checkBox)
        {
            _checkBox = checkBox;
        }

        public override void Execute()
        {
            _checkBox.Click();
        }
    }

    //public class CheckBoxActionFactory : IFuzzyActionFactory
    //{
    //    public void Register(IWebDriver browser, List<FuzzyAction> actions)
    //    {
    //        foreach (var checkBox in browser.CheckBoxes)
    //        {
    //            actions.Add(new CheckBoxAction(checkBox));
    //        }

    //    }
    //}
}