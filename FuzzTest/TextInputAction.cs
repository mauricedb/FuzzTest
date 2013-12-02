using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace FuzzTest
{
    public class TextInputAction : FuzzyAction
    {
        private static readonly Random _rnd = new Random(Environment.TickCount);
        private readonly IWebElement _textField;
        private readonly string _text;

        public TextInputAction(IWebElement textField)
            : base(textField)
        {
            _textField = textField;
            _text = _textField.Text;
        }

        public override int Weight
        {
            get
            {
                return string.IsNullOrWhiteSpace(_text) ? 10 : 3;
            }
        }

        public override void Execute()
        {
            var text = GetRandomText();
            Console.WriteLine("Typing '{0}' into {1}", text, _textField.GetAttribute("name") ?? _textField.GetAttribute("id"));
            _textField.Clear();
            _textField.SendKeys(text);
        }

        public override bool CanExecute()
        {
            if (!string.IsNullOrEmpty(_textField.GetAttribute("readonly")))
            {
                return false;
            }

            var textType = _textField.GetAttribute("type");
            if (textType == "hidden")
            {
                return false;
            }

            return base.CanExecute();
        }

        private string GetRandomText()
        {
            var texts = new List<string>();

            var textType = _textField.GetAttribute("type");
            if (textType == "password")
            {
                texts.Add("bla");
                texts.Add("abl");
                for (int i = 0; i < 100; i++)
                {
                    texts.Add("blabla");
                }
            }
            else
            {
                texts.Add(".");
                for (int i = 0; i < 50; i++)
                {
                    texts.Add("".PadRight(10, 'a'));
                }
                var maxLengthStr = _textField.GetAttribute("maxlength");
                var maxLength = 0;
                if (int.TryParse(maxLengthStr, out maxLength))
                {
                    if (maxLength < int.MaxValue)
                    {
                        Console.WriteLine(maxLength);
                        texts.Add("".PadRight(maxLength, 'm'));
                    }
                    texts.Add(int.MaxValue.ToString());
                }

                texts.Add("1");
                texts.Add("0");
                texts.Add("-1");
                texts.Add(int.MinValue.ToString());

                //texts.Add("".PadRight(100, 'a'));
                //texts.Add("".PadRight(1000, 'a'));
                texts.Add("<script>alert('Some injected script')</script>");
            }

            return texts[_rnd.Next(texts.Count)];
        }
    }

    public class TextInputActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {
            foreach (var textField in browser.FindElements(By.TagName("input")))
            {
                var type = textField.GetAttribute("type");
                if (type == "text" || type == "password")
                {
                    actions.Add(new TextInputAction(textField));
                }
            }
        }
    }


}