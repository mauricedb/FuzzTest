using System;
using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public class LinkAction : FuzzyAction
    {
        private readonly Link _element;

        public LinkAction(Link element)
            : base(element)
        {
            _element = element;
        }

        public override int Weight
        {
            get
            {
                return 1;
            }
        }

        public override bool CanExecute()
        {
            if (_element.Url.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            if (!_element.Url.StartsWith("http://localhost:", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return base.CanExecute();
        }

        public override void Execute()
        {
            Console.WriteLine("Clicking '{0}'", _element);
            _element.Click();
        }

    }


    public class LinkActionFactory : IFuzzyActionFactory
    {
        public void Register(Browser browser, List<FuzzyAction> actions)
        {
            foreach (var link in browser.Links)
            {
                actions.Add(new LinkAction(link));
            }
        }
    }
}