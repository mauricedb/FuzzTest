using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace FuzzTest
{
    public class LinkAction : FuzzyAction
    {
        private readonly IWebElement _element;
        private readonly IWebDriver _browser;

        public LinkAction(IWebDriver browser, IWebElement element)
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
            var url = _element.GetAttribute("href");
            if (url == null)
            {
                return false;
            }

            if (url.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
            {
                // Do not click on mail links
                return false;
            }

            var uri = new Uri(_browser.Url);
            var rootUrl = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
            if (!url.StartsWith(rootUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                // Do not follow links that navigate to other domains
                return false;
            }

            return base.CanExecute();
        }

        public override void Execute()
        {
            var text = _element.Text;
            var id = _element.GetAttribute("id");

            Console.WriteLine("Clicking '{0}'", text ?? id);
            _element.Click();
        }

    }


    public class LinkActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {
            var links = browser.FindElements(By.TagName("a"));
            actions.AddRange(
                links.Select(link => new LinkAction(browser, link)));
        }
    }
}