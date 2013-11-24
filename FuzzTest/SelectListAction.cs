using System;
using System.Collections.Generic;
using WatiN.Core;

namespace FuzzTest
{
    public class SelectListAction : FuzzyAction
    {
        private static Random _rnd = new Random(Environment.TickCount);
        private readonly SelectList _selectList;

        public SelectListAction(SelectList selectList)
            : base(selectList)
        {
            _selectList = selectList;
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
                Option option = _selectList.Options[item];
                if (option.Selected)
                {
                    Console.WriteLine("Clearing option {0} for '{1}'", option, _selectList);
                    option.Clear();
                }
                else
                {
                    Console.WriteLine("Selecting option {0} for '{1}'", option, _selectList);
                    option.Select();
                }
            }
        }
    }

    //public class SelectListActionFactory : IFuzzyActionFactory
    //{
    //    public void Register(Browser browser, List<FuzzyAction> actions)
    //    {
    //        foreach (var selectList in browser.SelectLists)
    //        {
    //            actions.Add(new SelectListAction(selectList));
    //        }

    //    }
    //}
}