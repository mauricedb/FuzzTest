using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public class RadioButtonAction : FuzzyAction
    {
        private readonly RadioButton _radioButton;

        public RadioButtonAction(RadioButton radioButton):base(radioButton)
        {
            _radioButton = radioButton;
        }

        public override void Execute()
        {
            _radioButton.Click();
        }
    }

    //public class RadioButtonActionFactory : IFuzzyActionFactory
    //{
    //    public void Register(Browser browser, List<FuzzyAction> actions)
    //    {
    //        foreach (var radioButton in browser.RadioButtons)
    //        {
    //            actions.Add(new RadioButtonAction(radioButton));
    //        }
    //    }
    //}
}