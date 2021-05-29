using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGFRCalc.Objects
{
    public class JodDosprotokoll
    {
        #region Field Region Inställningar

        int dosprotokollID = 0;
        string dosprotokollProtokollNamn = String.Empty;
        double dosprotokollJodkoncentration = 0;
        double dosprotokollDosKg = 0;
        double dosprotokollInjektionstid = 0;
        double dosprotokollDoshastighet = 0;
        double dosprotokollMaxVikt = 0;
        string dosprotokollAnteckningar = String.Empty;

        #endregion

        #region Property Region Inställningar

        public int DosprotokollID
        {
            get { return dosprotokollID; }
            set { dosprotokollID = value; }
        }

        public string DosprotokollProtokollNamn
        {
            get { return dosprotokollProtokollNamn; }
            set { dosprotokollProtokollNamn = value; }
        }

        public double DosprotokollJodkoncentration
        {
            get { return dosprotokollJodkoncentration; }
            set { dosprotokollJodkoncentration = value; }
        }
        public double DosprotokollDosKg
        {
            get { return dosprotokollDosKg; }
            set { dosprotokollDosKg = value; }
        }

        public double DosprotokollInjektionstid
        {
            get { return dosprotokollInjektionstid; }
            set { dosprotokollInjektionstid = value; }
        }
        public double DosprotokollDoshastighet
        {
            get { return dosprotokollDoshastighet; }
            set { dosprotokollDoshastighet = value; }
        }
        public double DosprotokollMaxVikt
        {
            get { return dosprotokollMaxVikt; }
            set { dosprotokollMaxVikt = value; }
        }
        public string DosprotokollAnteckningar
        {
            get { return dosprotokollAnteckningar; }
            set { dosprotokollAnteckningar = value; }
        }

        #endregion

        public JodDosprotokoll()
        {

        }
    }
}
