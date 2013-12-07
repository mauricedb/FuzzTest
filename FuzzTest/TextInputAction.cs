using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;

namespace FuzzTest
{
    public class TextInputAction : FuzzyAction
    {
        private static readonly Random Rnd = new Random(Environment.TickCount);
        private readonly IWebElement _textField;
        private readonly string _text;
        private readonly string _readOnly;
        private readonly string _type;
        private readonly string _maxLength;

        public TextInputAction(IWebElement textField)
            : base(textField)
        {
            _textField = textField;
            _readOnly = _textField.GetAttribute("readonly");
            _type = _textField.GetAttribute("type");
            _maxLength = _textField.GetAttribute("maxlength");
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
            if (!string.IsNullOrEmpty(_readOnly))
            {
                return false;
            }

            if (_type == "hidden")
            {
                return false;
            }

            return base.CanExecute();
        }

        private string GetRandomText()
        {
            var texts = new List<string>();

            var textType = _type;
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
                int maxLength;
                if (int.TryParse(_maxLength, out maxLength))
                {
                    if (maxLength < int.MaxValue)
                    {
                        Console.WriteLine(maxLength);
                        texts.Add("".PadRight(maxLength, 'm'));
                    }
                    texts.Add(int.MaxValue.ToString(CultureInfo.InvariantCulture));
                }

                texts.Add("1");
                texts.Add("0");
                texts.Add("-1");
                texts.Add(int.MinValue.ToString(CultureInfo.InvariantCulture));

                //texts.Add("".PadRight(100, 'a'));
                //texts.Add("".PadRight(1000, 'a'));
                texts.Add("<script>alert('Some injected script')</script>");
            }

            return texts[Rnd.Next(texts.Count)];
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