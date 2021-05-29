using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Helper_Classes
{

    public class Calculate
    {
        public static int Ålder(DateTime bd) // En funktion för att räkna ut din ålder baserat på ditt födelsedatum och returnera det
        {
            //Gör om variabeln inputBirthDay som är en string, till en DateTime variabel med namnet "myBirthDay"

            DateTime n = DateTime.Now;  //sätter DateTime variabeln n till nu (dagens datum)
            int age = DateTime.Now.Year - bd.Year;  //min ålder (age) är dagens år - mitt födelseår

            //räknar ut om vi fyllt år ännu
            if (n.Month < bd.Month || ((n.Month == bd.Month) && (n.Day < bd.Day)))
            {
                age--;  //om inte så tar vi bort ett år
            }

            return age; //skickar tillbaks åldern
        }

        public static double BMI(double Height, double Weight) //En metod för att beräkna BMI
        {
            //BMI formel: BMI Calculation: VIKT ÷ (LÄNGD)2 = X
            //Exempel: 68 ÷ (1.65)2 = 24.98

            double height = Height / 100; // gör om längden till en double, och gör om det från cm till m
            double weight = Weight;    // gör om vikten till en double

            double bmi = weight / Math.Pow(height, 2);

            return bmi;
        }

        public static double BSA(double Height, double Weight)    //En metod för att räkna ut kroppsyta
        {
            //Formel för att beräkna kroppsyta
            //Kroppsyta i kvadratmeter beräknas ofta enligt Dubois formel från 1916:
            //Vikt (kg)(^0,425) x längd (cm)(^0,725) x 0,007184.

            double height = Height;
            double weight = Weight;
            double bodySurface = ((Math.Pow(weight, 0.425) * Math.Pow(height, 0.725)) * 0.007184);

            return bodySurface;
        }

        public static double IBW(double Height, PatientGender Sex)
        {
            //Formel för att beräkna Ideal Body Weight
            //Idealvikt baseras på längd i cm beräknas enligt Devines formel.
            //Kvinnor: 45,5+[2,3 x (längd/2,54-60)]
            //Män: 50,0+[2,3 x (längd/2,54-60)]

            double idealBodyWeight = 0;

            if (Sex == PatientGender.Kvinna)
            {
                idealBodyWeight = 45.5 + (2.3 * ((Height / 2.54) - 60));
            }
            if (Sex == PatientGender.Man)
            {
                idealBodyWeight = 50 + (2.3 * ((Height / 2.54) - 60));
            }

            return idealBodyWeight;
        }

        public static double LBM(double Height, double Weight, PatientGender Sex)
        {
            //Lean body mass enligt James (LBM-James)
            //Kvinnor: (1,07 x vikt) - 148 x (vikt2/längd2)
            //Män: (1,10 x vikt) - 120 x (vikt2/längd2)

            double leanBodyMass = 0;

            if (Sex == PatientGender.Kvinna)
            {
                leanBodyMass = (1.07 * Weight) - 148 * ((Math.Pow(Weight, 2) / Math.Pow(Height, 2)));
            }
            if (Sex == PatientGender.Man)
            {
                leanBodyMass = (1.10 * Weight) - 120 * ((Math.Pow(Weight, 2) / Math.Pow(Height, 2)));
            }

            return leanBodyMass;
        }

        public static double eGFRLMAbsolut(int Ålder, double Krea, double LBM)
        {
            //Lund-Malmö formel som ger absolut GFR i mL/min
            //eX - 0,0128 x ålder + 0,387 x ln(ålder) + 1,10 x ln(LBM-James)
            //X = –0,0111 x pKr                             (om pKr <150 μmol/L)
            //X = 3,55 + 0,0004 x pKr - 1,07 x ln(pKr)      (om pKr ≥150 μmol/L)

            double absoluteGFR = 0;
            double preCalc1 = 0;
            double preCalc2 = 0;
            double e = Math.E;

            if (Krea < 150)
            {
                preCalc1 = -0.0111 * Krea;
            }
            if (Krea >= 150)
            {
                preCalc1 = 3.55 + (0.0004 * Krea) - (1.07 * Math.Log(Krea, Math.E));
            }

            preCalc2 = preCalc1 - (0.0128 * Ålder) + (0.387 * Math.Log(Ålder, Math.E)) + (1.10 * Math.Log(LBM, Math.E));
            absoluteGFR = Math.Pow(e, preCalc2);

            return absoluteGFR;
        }

        public static double eGFRLMRelativtUtanLBM(int Ålder, double Krea, PatientGender Sex)
        {
            //Malmö-Lund som ger relativt GFR i mL/min/1,73 m2 (uträknat utan längd/vikt)
            //eX – 0,0124 x ålder+0,339 x ln(ålder) - 0,226 (om kvinna)
            //X = 4,62 – 0,0112 x pKr (om pKr <150 μmol/L)
            //X = 8,17 + 0,0005 x pKr - 1,07 x ln(pKr) (om pKr ≥150 μmol/L)

            double relativeGFR = 0;
            double preCalc1 = 0;
            double preCalc2 = 0;
            double e = Math.E;

            if (Krea < 150)
            {
                preCalc1 = 4.62 - (0.0112 * Krea);
            }
            if (Krea >= 150)
            {
                preCalc1 = 8.17 + (0.0005 * Krea) - (1.07 * Math.Log(Krea, Math.E));
            }

            preCalc2 = preCalc1 - (0.0124 * Ålder) + (0.339 * Math.Log(Ålder, Math.E));
            if (Sex == PatientGender.Kvinna)
            {
                preCalc2 = (preCalc2 - 0.226);
            }
            relativeGFR = Math.Pow(e, preCalc2);

            return relativeGFR;
        }

        public static double eGFRLMRelativtMedLBM(int Ålder, double Krea, double LBM)
        {
            //Lund-Malmö ekvationen med Lean Body Mass(LMLBM)(2)
            //Den baseras på P-kreatinin (pKrea), ålder och Lean Body Mass enligt James.
            //eGFR i ml/min/1.73m2 = eX – (0.00587 * ålder) + (0.00977 * Lean Body Mass)
            //X = 4.95 – 0.0110 * pKrea			(om pKrea < 150 µmol/l)
            //X = 8.58 + 0.0005 * pKrea – 1-08 * ln(pKr)		(om pKrea >= 150 µmol/l)
            double relativeGFR = 0;
            double preCalc1 = 0;
            double preCalc2 = 0;
            double e = Math.E;

            if (Krea < 150)
            {
                preCalc1 = 4.95 - (0.0110 * Krea);
            }
            if (Krea >= 150)
            {
                preCalc1 = 8.58 + (0.0005 * Krea) - (1.08 * Math.Log(Krea, Math.E));
            }
            preCalc2 = preCalc1 - (0.00578 * Ålder) + (0.00977 * LBM);

            relativeGFR = Math.Pow(e, preCalc2);

            return relativeGFR;
        }

        public static double eGFRGrubbRelativt(double Cystatin, PatientGender Sex)
        {
            //Grubb cystatin C ekvation (eGFR cystatom C)
            //GFR = 86.49 * CyC(^-1.686) * 0.948 (if female),
            //är likvärdigt med
            //GFR = e(^4.46 - 1.686 * ln(PKR) - 0.053) (if female)

            double relativeGFR = 0;
            double preCalc = 0;

            preCalc = 86.49 * Math.Pow(Cystatin, -1.686);   //alternativ 1
            //preCalc2 = 4.46 - (1.686 * 1.07) * Math.Log(PCY, Math.E) - 0.053;   //Alternativ 2
            if (Sex == PatientGender.Kvinna)
            {
                preCalc = preCalc * 0.948;
                //preCalc2 = preCalc2 - 0.053;
            }

            relativeGFR = preCalc; //Alternativ 1
            //relativeGFR2 = Math.Pow(e, preCalc2); //Alternativ 2

            return relativeGFR;
        }

        public static double eGFRAbsolutFrånRelativt(double BSA, double RelativeGFR)
        {
            //Räknar ut absolut GFR genom att räkna på patientens kroppsyta och relativt GFR
            //(Kroppsyta m2/1.73 m2)*Relativt GFR

            double absoluteGFR = (BSA / 1.73) * RelativeGFR;

            return absoluteGFR;
        }

        public static double eGFRRelativtFrånAbsolut(double BSA, double AbsoluteGFR)
        {
            //Räknar ut relativt GFR genom att räkna på patientens kroppsyta och absolut GFR
            //(1.73/kroppsyta m2) * Absolut GFR

            double relativeGFR = (1.73 / BSA) * AbsoluteGFR;

            return relativeGFR;
        }

        public static double ProcenskillnadeGFRLMGrubbRelativt(double GFRCystatinCRelativ, double GFRKreatininRelativ)
        {
            double GFRCystatinCKreatininRelativ = (GFRCystatinCRelativ + GFRKreatininRelativ) / 2;
            double mellanskillnad = GFRCystatinCRelativ - GFRKreatininRelativ;

            double procentSkillnad = mellanskillnad / GFRCystatinCKreatininRelativ;

            return procentSkillnad;
        }

        public static double PatientMaxDosJod(double AbsolutGFR, int JodKoncentration)
        {
            double patientMaxDosJod = (AbsolutGFR * 1000) / JodKoncentration;
            return patientMaxDosJod;
        }
    }
}
