﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FuzzTest
{
    public class SelectListAction : FuzzyAction
    {
        private static Random _rnd = new Random(Environment.TickCount);
        private readonly SelectElement _selectList;

        public SelectListAction(IWebElement selectList)
            : base(selectList)
        {
            _selectList = new SelectElement(selectList);
        }

        public override int Weight
        {
            get
            {
                return 10;
            }
        }

        public override void Execute()
        {

            //if (_selectList.Multiple)
            //{
            //}
            //else
            {
                var item = _rnd.Next(_selectList.Options.Count);
                var option = _selectList.Options[item];
                if (option.Selected)
                {
                    Console.WriteLine("Clearing option {0} for '{1}'", option, _selectList);
                    option.Click();
                }
                else
                {
                    Console.WriteLine("Selecting option {0} for '{1}'", option, _selectList);
                    option.Click();
                }
            }
        }
    }

    public class SelectListActionFactory : IFuzzyActionFactory
    {
        public void Register(IWebDriver browser, List<FuzzyAction> actions)
        {
            foreach (var selectList in browser.FindElements(By.TagName("select")))
            {
                actions.Add(new SelectListAction(selectList));
            }

        }
    }
}