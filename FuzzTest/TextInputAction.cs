using System;
using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public class TextInputAction : FuzzyAction
    {
        private static Random _rnd = new Random(Environment.TickCount);
        private readonly TextField _textField;

        public TextInputAction(TextField textField)
            : base(textField)
        {
            _textField = textField;
        }

        public override int Weight
        {
            get { return string.IsNullOrWhiteSpace(_textField.Value) ? 10 : 3; }
        }

        public override void Execute()
        {
            var text = GetRandomText();
            Console.WriteLine("Typing '{0}' into {1}", text, _textField.Name);
            _textField.TypeText(text);
        }

        public override bool CanExecute()
        {
            if (_textField.ReadOnly)
            {
                return false;
            }

            var textType = _textField.GetAttributeValue("type");
            if (textType == "hidden")
            {
                return false;
            }

            return base.CanExecute();
        }

        private string GetRandomText()
        {
            var texts = new List<string>();

            var textType = _textField.GetAttributeValue("type");
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
                if (_textField.MaxLength < int.MaxValue)
                {
                    Console.WriteLine(_textField.MaxLength);
                    texts.Add("".PadRight(_textField.MaxLength, 'm'));
                }
                texts.Add(int.MaxValue.ToString());
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
        public void Register(Browser browser, List<FuzzyAction> actions)
        {
            foreach (var textField in browser.TextFields)
            {
                var type = textField.GetAttributeValue("type");
                if (type != "hidden")
                {
                    actions.Add(new TextInputAction(textField));
                }
            }
        }
    }   


}