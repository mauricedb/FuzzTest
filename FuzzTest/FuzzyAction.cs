﻿
using System.Collections.Generic;
using OpenQA.Selenium;

namespace FuzzTest
{
    public abstract class FuzzyAction
    {
        private readonly IWebElement _webElement;
        private string _dataFuzzEnabed;

        protected FuzzyAction(IWebElement webElement)
        {
            _webElement = webElement;
            if (_webElement != null)
            {
                _dataFuzzEnabled = _webElement.GetAttribute("data-fuzz-enabled");
                Id = _webElement.GetAttribute("id");
            }
        }

        public string Id { get; private set; }

        public virtual string StackId
        {
            get
            {
                return _webElement.Text;
            }
        }

        public virtual int Weight
        {
            get { return 10; }
        }

        public virtual bool CanExecute()
        {

            if (!_webElement.Enabled)
            {
                return false;
            }

            if (!_webElement.Displayed)
            {
                return false;
            }

            if (_dataFuzzEnabed == "false")
            {
                return false;
            }

            return true;
        }

        public abstract void Execute();


        public string _dataFuzzEnabled { get; set; }
    }

    public interface IFuzzyActionFactory
    {
        void Register(IWebDriver browser, List<FuzzyAction> actions);
    }

}