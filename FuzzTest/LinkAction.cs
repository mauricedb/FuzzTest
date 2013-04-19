using System;
using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public class LinkAction : FuzzyAction
    {
        private readonly Link _element;
        private Browser _browser;

        public LinkAction(Browser browser, Link element)
            : base(element)
        {
            _browser = browser;
            _element = element;
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
            if (_element.Url.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var rootUrl = _browser.Uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
            if (!_element.Url.StartsWith(rootUrl, StringComparison.InvariantCultureIgnoreCase))
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
                actions.Add(new LinkAction(browser, link));
            }
        }
    }
}