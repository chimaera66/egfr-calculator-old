using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGFRCalc.Objects
{
    class ComboBoxItem
    {
            string namn;
            int id;

            //Constructor
            public ComboBoxItem(string d, int h)
            {
                namn = d;
                id = h;
            }

            //Accessor
            public int ID
            {
                get
                {
                    return id;
                }
            }

            //Override ToString method
            public override string ToString()
            {
                return namn;
            }
    }
}
