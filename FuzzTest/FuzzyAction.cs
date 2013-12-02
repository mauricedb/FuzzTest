
using System.Collections.Generic;
using OpenQA.Selenium;
using WatiN.Core;

namespace FuzzTest
{
    public abstract class FuzzyAction
    {
        private readonly IWebElement _webElement;
        private readonly Element _element;

        protected FuzzyAction(Element element)
        {
            _element = element;
        }

        protected FuzzyAction(IWebElement webElement)
        {
            _webElement = webElement;
        }

        public Element Element
        {
            get { return _element; }
        }

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

            if (_webElement.GetAttribute("data-fuzz-enabled") == "false")
            {
                return false;
            }

            return true;
        }

        public abstract void Execute();

    }

    public interface IFuzzyActionFactory
    {
        void Register(IWebDriver browser, List<FuzzyAction> actions);
    }

}