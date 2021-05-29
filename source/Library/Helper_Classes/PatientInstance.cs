using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Helper_Classes
{
    public enum PatientGender { Man, Kvinna }

    public class PatientInstance
    {
        #region Field Region PATIENTUPPGIFTER

        DateTime birthDate;
        int age;
        double height;
        double weight;
        PatientGender sex;

        #endregion
        #region Property Region PATIENT UPPGIFTER

        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = (double)value; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = (double)value; }
        }

        public PatientGender Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        #endregion



        #region Field Region KROPPSBERÄKNINGAR

        double bmi;
        double bsa;
        double ibw;
        double lbm;

        #endregion
        #region Property Region KROPPSBERÄKNINGAR

        public double BMI
        {
            get { return bmi; }
            set { bmi = (double)value; }
        }
        public double BSA
        {
            get { return bsa; }
            set { bsa = (double)value; }
        }
        public double IBW
        {
            get { return ibw; }
            set { ibw = (double)value; }
        }
        public double LBM
        {
            get { return lbm; }
            set { lbm = (double)value; }
        }

        #endregion



        #region Field Region PATIENTLABUPPGIFTER

        double kreatinin;
        double cystatin;

        #endregion
        #region Property Region PATIENTLABUPPGIFTER

        public double Kreatinin
        {
            get { return kreatinin; }
            set { kreatinin = (double)value; }
        }

        public double Cystatin
        {
            get { return cystatin; }
            set { cystatin = (double)value; }
        }

        #endregion



        #region Field Region GFR KREATININ

        double egfrLMAbsolut;
        double egfrLMRelativtUtanLBM;
        double egfrLMRelativtMedLBM;

        #endregion
        #region Property Region GFR KREATININ

        public double EGFRLMAbsolut
        {
            get { return egfrLMAbsolut; }
            set { egfrLMAbsolut = value; }
        }

        public double EGFRLMRelativtUtanLBM
        {
            get { return egfrLMRelativtUtanLBM; }
            set { egfrLMRelativtUtanLBM = value; }
        }

        public double EGFRLMRelativtMedLBM
        {
            get { return egfrLMRelativtMedLBM; }
            set { egfrLMRelativtMedLBM = value; }
        }

        #endregion



        #region Field Region GFR CYSTATIN

        double egfrGrubbRelativt;
        double egfrGrubbAbsolut;

        #endregion
        #region Property Region GFR CYSTATIN

        public double EGFRGrubbRelativt
        {
            get { return egfrGrubbRelativt; }
            set { egfrGrubbRelativt = value; }
        }

        public double EGFRGrubbAbsolut
        {
            get { return egfrGrubbAbsolut; }
            set { egfrGrubbAbsolut = value; }
        }

        #endregion



        #region Field Region GFR KREATININ+CYSTATIN

        double egfrLMGrubbAbsolut;
        double egfrLMGrubbRelativt;
        double egfrLMGrubbMellanskillnad;

        #endregion
        #region Property Region GFR KREATININ+CYSTATIN

        public double EGFRLMGrubbAbsolut
        {
            get { return egfrLMGrubbAbsolut; }
            set { egfrLMGrubbAbsolut = value; }
        }

        public double EGFRLMGrubbRelativt
        {
            get { return egfrLMGrubbRelativt; }
            set { egfrLMGrubbRelativt = value; }
        }

        public double EGFRLMGrubbMellanskillnad
        {
            get { return egfrLMGrubbMellanskillnad; }
            set { egfrLMGrubbMellanskillnad = value; }
        }

        #endregion



        #region Field Region ANVÄNDT GFR

        double usedGFRAbsolute;
        double usedGFRRelative;

        #endregion
        #region Property Region ANVÄNDT GFR

        public double UsedGFRAbsolute
        {
            get { return usedGFRAbsolute; }
            set { usedGFRAbsolute = value; }
        }

        public double UsedGFRRelative
        {
            get { return usedGFRRelative; }
            set { usedGFRRelative = value; }
        }

        #endregion




        #region Field Region CIN ANTAL RISKFAKTORER

        int antalCINRiskfaktorer;
        bool riskFaktorCIN = false;

        #endregion
        #region Property Region CIN ANTAL RISKFAKTORER
        public int AntalCINRiskFaktorer
        {
            get { return antalCINRiskfaktorer; }
            set { antalCINRiskfaktorer = value; }
        }

        public bool RiskFaktorCIN
        {
            get { return riskFaktorCIN; }
            set { riskFaktorCIN = value; }
        }
        #endregion




        #region Field Region MAX JOD DOS

        double maxJodDos;
        double maxJodDosGram;

        #endregion
        #region Property Region MAX JOD DOS

        public double MaxJodDos
        {
            get { return maxJodDos; }
            set { maxJodDos = value; }
        }
        public double MaxJodDosGram
        {
            get { return maxJodDosGram; }
            set { maxJodDosGram = value; }
        }

        #endregion




        #region Field Region JODKONTRASTDOS

        double jodKontrastDos;
        double jodKontrastHastighet;

        #endregion
        #region Property Region JODKONTRASTDOS

        public double JodKontrastDos
        {
            get { return jodKontrastDos; }
            set { jodKontrastDos = value; }
        }

        public double JodKontrastHastighet
        {
            get { return jodKontrastHastighet; }
            set { jodKontrastHastighet = value; }
        }

        #endregion




        #region Field Region PVK

        string pvkFärg;

        #endregion
        #region Property Region PVK

        public string PVKFärg
        {
            get { return pvkFärg; }
            set { pvkFärg = value; }
        }

        #endregion




        #region Constructor Region

        public PatientInstance()
        {
        }

        #endregion
    }
}
