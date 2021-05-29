using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGFRCalc.Objects
{
    public class SettingsMainSettings
    {

        #region Field Region Inställningar

        int jodkoncentration = 350;
        string pvk = "BD Venflon Pro Safety";

        double barnJodDosPerKilo = 700;
        double barnInjektionstid = 20;
        int barnMaxÅlder = 15;
        int barnMaxVikt = 45;

        #endregion

        #region Property Region Inställningar

        public int Jodkoncentration
        {
            get { return jodkoncentration; }
            set { jodkoncentration = value; }
        }

        public string PVK
        {
            get { return pvk; }
            set { pvk = value; }
        }
        
        public double BarnJodDosPerKilo
        {
            get { return barnJodDosPerKilo; }
            set { barnJodDosPerKilo = value; }
        }

        public double BarnInjektionstid
        {
            get { return barnInjektionstid; }
            set { barnInjektionstid = value; }
        }

        public int BarnMaxÅlder
        {
            get { return barnMaxÅlder; }
            set { barnMaxÅlder = value; }
        }

        public int BarnMaxVikt
        {
            get { return barnMaxVikt; }
            set { barnMaxVikt = value; }
        }

        #endregion

        #region Constructor Region
        public SettingsMainSettings()
        {

        }
        #endregion
    }
}
