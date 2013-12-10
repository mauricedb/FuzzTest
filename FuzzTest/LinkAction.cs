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
        private string _href;
        private string _onclick;

        public LinkAction(IWebDriver browser, IWebElement element)
            : base(element)
        {
            _browser = browser;
            _element = element;
            _href = _element.GetAttribute("href");
            _onclick = _element.GetAttribute("onclick");
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

            if (!string.IsNullOrEmpty(_onclick))
            {
                return base.CanExecute(); 
            }

            var url = _href;
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

            Console.WriteLine("Clicking '{0}'", text ?? Id);
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