
using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public abstract class FuzzyAction
    {
        private readonly Element _element;

        protected FuzzyAction(Element element)
        {
            _element = element;
        }

        public Element Element
        {
            get { return _element; }
        }

        public virtual string StackId
        {
            get { return _element.ToString(); }
        }

        public virtual int Weight
        {
            get { return 10; }
        }

        public virtual bool CanExecute()
        {
            if (!_element.Enabled)
            {
                return false;
            }

            if (_element.GetAttributeValue("data-fuzz-enabled") == "false")
            {
                return false;
            }

            if (!IsVisible())
            {
                return false;
            }

            return true;
        }

        public abstract void Execute();

        private bool IsVisible()
        {
            var element = _element;
            while (element != null)
            {
                if (_element.Style.Display == "none")
                {
                    return false;
                }
                element = element.Parent;
            }

            return true;
        }
    }

    public interface IFuzzyActionFactory
    {
        void Register(Browser browser, List<FuzzyAction> actions);
    }

}