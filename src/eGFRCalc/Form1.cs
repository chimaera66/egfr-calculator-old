using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//XML
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//Project Classes
using eGFRCalc.Objects;
using Library.Objects;
using Library.Helper_Classes;


namespace eGFRCalc
{
    public partial class eGFRCalculatorMainForm : Form
    {
        //patientdata objektet
        PatientInstance patientInstance;
        SettingsMainSettings settings;
        JodDosprotokoll jodDosprotokoll;

        List<ProtokollJodkontrast> dosProtokollen = new List<ProtokollJodkontrast>();
        List<PVK> PVKmärken = new List<PVK>();

        bool patientFödelsedagValid = false;

        //string som innehåller sökvägen till programmet

        public eGFRCalculatorMainForm()
        {
            //Put all components on form
            InitializeComponent();

            //Position some components(center, mm)
            Positionera();

            //Hämtar sökvägen till exefilen
            //GetPath();

            //Läs från fil
            //Läser inställningar
            LäsInställningar();
            //Läser in dosprotokoll från fil
            LäsDosProtokoll();
            //Läser in PVKmärken från fil
            LäsPVKMärken();


            ResetAll();

            //Startar eventhanterare
            StartEventHandlers();
            StartEventHandlersJod();

            //Markerar födelsedags rutan
            this.maskedPatientIndataPatientDataBirthdate.SelectAll();
        }

        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Inställningar

        #region Läser från Fil
        private void LäsInställningar()   //Läser inställningar
        {
            List<Settings> tempSettings = new List<Settings>();
            tempSettings = XML.DeserializeFromXMLSettings("settings.xml");

            settings = new SettingsMainSettings();

            //skriver ut de nyinlästa objectlistorna från settings.xml
            tempSettings.ForEach(delegate(Settings p)
            {
                settings.Jodkoncentration = p.Jodkoncentration;
                settings.PVK = p.PVK;
                settings.BarnJodDosPerKilo = p.BarnJodDosPerKg;
                settings.BarnInjektionstid = p.BarnInjektionstid;
                settings.BarnMaxVikt = p.BarnMaxVikt;
                settings.BarnMaxÅlder = p.BarnMaxÅlder;

            });
            //comboBoxPVKMärke.SelectedIndex = comboBoxPVKMärke.FindString(Convert.ToString(Variables.InställningPVK));
        }

        //Läser in Joddosprotokollen från fil och stoppar dom i combolisten
        private void LäsDosProtokoll()
        {
            //Lägger till ett första tomt alternativ och väljer att det ska vara försinställt
            //Add item to ComboBox:
            comboBoxDosProtokoll.Items.Add(new ComboBoxItem("", 0));
            comboBoxDosProtokoll.SelectedIndex = 0;

            dosProtokollen = XML.DeserializeFromXMLDosProtokoll("joddosprotokoll.xml");

            dosProtokollen.ForEach(delegate(ProtokollJodkontrast p)
            {
                comboBoxDosProtokoll.Items.Add(new ComboBoxItem(p.ProtokollNamn, p.ID));
            });
        }

        private void LäsPVKMärken()
        {
            PVKmärken = XML.DeserializeFromXMLPVK("PVK.xml");

            #region skriver ut de nyinlästa objectlistorna från joddosprotokoll.xml
            PVKmärken.ForEach(delegate(PVK p)
            {
                comboBoxPVKMärke.Items.Add(new ComboBoxItem(p.namn, p.PVKID));
            });
            comboBoxPVKMärke.SelectedIndex = comboBoxPVKMärke.FindString(Convert.ToString(settings.PVK));
            #endregion
        }

        #endregion
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================


        //########################################################################
        //
        //Positionerar komponenter i MainForm
        //
        //########################################################################
        public void Positionera()
        {
            //Positionera Patientdata objekt
            this.labelPatientdata.Location = new System.Drawing.Point((this.panelPatientIndata.Size.Width / 2 - 2) - (this.labelPatientdata.Size.Width / 2), 9);
            this.panelPatientIndataPatientData.Location = new System.Drawing.Point((this.panelPatientIndata.Size.Width / 2 - 2) - (this.panelPatientIndataPatientData.Size.Width / 2), 37);
            this.panelPatientIndataPatientKreaCystatin.Location = new System.Drawing.Point((this.panelPatientIndata.Size.Width / 2 - 2) - (this.panelPatientIndataPatientKreaCystatin.Size.Width / 2), 143);
            this.panelPatientIndataSex.Location = new System.Drawing.Point((this.panelPatientIndata.Size.Width / 2 - 2) - (this.panelPatientIndataSex.Size.Width / 2), 204);
            this.panelPatientIndataCalculated.Location = new System.Drawing.Point((this.panelPatientIndata.Size.Width / 2 - 2) - (this.panelPatientIndataCalculated.Size.Width / 2), 235);
            this.buttonPatientIndataCalculate.Location = new System.Drawing.Point(this.panelPatientIndataCalculated.Location.X, 301);
            this.buttonPatientIndataClear.Location = new System.Drawing.Point(this.buttonPatientIndataCalculate.Location.X + 100, 301);
            this.labelUtveckladAv.Location = new System.Drawing.Point((this.panelPatientIndata.Size.Width / 2) - (this.labelUtveckladAv.Size.Width / 2), 386);

            //Positionera Riskfaktorer objekt
            this.panelRiskFaktorer.Location = new System.Drawing.Point(this.Width - this.panelRiskFaktorer.Width - 15, -1);
            this.labelRiskFaktorerRiskfaktorerCIN.Location = new System.Drawing.Point((this.panelRiskFaktorer.Size.Width / 2 - 2) - (this.labelRiskFaktorerRiskfaktorerCIN.Size.Width / 2), 9);
            this.panelRiskFaktorerFaktorer.Location = new System.Drawing.Point((this.panelRiskFaktorer.Size.Width / 2 - 2) - (this.panelRiskFaktorerFaktorer.Size.Width / 2), 37);
            this.labelRiskfaktorerKalkyleradCINRisk.Location = new System.Drawing.Point((this.panelRiskFaktorer.Size.Width / 2 - 2) - (this.labelRiskfaktorerKalkyleradCINRisk.Size.Width / 2), 319);
            this.panelRiskFaktorerRisknivå.Location = new System.Drawing.Point((this.panelRiskFaktorer.Size.Width / 2 - 2) - (this.panelRiskFaktorerRisknivå.Size.Width / 2), 347);
            this.labelRiskFaktorerCINRiskNivå.Location = new System.Drawing.Point((this.panelRiskFaktorerRisknivå.Size.Width / 2) - (this.labelRiskFaktorerCINRiskNivå.Size.Width / 2), (this.panelRiskFaktorerRisknivå.Size.Height / 2) - (this.labelRiskFaktorerCINRiskNivå.Size.Height / 2));


            //Positionera eGFR att använda sig av
            this.panelChooseGFRToUse.Size = new System.Drawing.Size(this.Size.Width - (this.panelPatientIndata.Width + this.panelRiskFaktorer.Width) - 12, 65);
            this.panelChooseGFRToUse.Location = new System.Drawing.Point(this.panelPatientIndata.Size.Width - 2, 347);
            this.labelChooseValue.Location = new System.Drawing.Point((this.panelChooseGFRToUse.Size.Width / 2) - (labelChooseValue.Size.Width / 2), 8);
            this.panelChooseGFRToUseChoices.Location = new System.Drawing.Point((this.panelChooseGFRToUse.Size.Width / 2 - 2) - (this.panelChooseGFRToUseChoices.Size.Width / 2), 27);
            this.radioButtonChooseGFRToUseMedel.Location = new System.Drawing.Point((this.panelChooseGFRToUseChoices.Size.Width / 4) - (this.radioButtonChooseGFRToUseMedel.Size.Width + 5), (this.panelChooseGFRToUseChoices.Size.Height / 2) - radioButtonChooseGFRToUseMedel.Size.Height / 2);
            this.radioButtonChooseGFRToUseMinsta.Location = new System.Drawing.Point((this.panelChooseGFRToUseChoices.Size.Width / 4 * 2) - (this.radioButtonChooseGFRToUseMinsta.Size.Width + 5), (this.panelChooseGFRToUseChoices.Size.Height / 2) - (radioButtonChooseGFRToUseMinsta.Size.Height / 2));
            this.radioButtonChooseGFRToUseKrea.Location = new System.Drawing.Point((this.panelChooseGFRToUseChoices.Size.Width / 4 * 3) - (this.radioButtonChooseGFRToUseKrea.Size.Width + 5), (this.panelChooseGFRToUseChoices.Size.Height / 2) - (radioButtonChooseGFRToUseKrea.Size.Height / 2));
            this.radioButtonChooseGFRToUseCysta.Location = new System.Drawing.Point((this.panelChooseGFRToUseChoices.Size.Width / 4 * 4) - (this.radioButtonChooseGFRToUseCysta.Size.Width + 5), (this.panelChooseGFRToUseChoices.Size.Height / 2) - (radioButtonChooseGFRToUseCysta.Size.Height / 2));


            //Positionera CT/MR Kontrastväljarpanielen
            this.panelChooseContrastType.Size = new System.Drawing.Size(this.Size.Width - (this.panelPatientIndata.Width + this.panelRiskFaktorer.Width) - 12, 43);
            this.panelChooseContrastType.Location = new System.Drawing.Point(this.panelPatientIndata.Size.Width - 2, -1);

            //Positionera Klassifikationspanelens grejjer
            //this.labelKlassifikationerBMI.Location = new System.Drawing.Point((this.panelKlassifikationerBMI.Size.Width / 2) - (this.labelKlassifikationerBMI.Size.Width / 2), 8);
            //this.labelKlassifikationerNjurfunktion.Location = new System.Drawing.Point((this.panelKlassifikationerNjurfunktion.Size.Width / 2) - (this.labelKlassifikationerNjurfunktion.Size.Width / 2), 8);
        }
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Nollställer information i MainForm
        //
        //########################################################################
        #region Nollställer information i MainForm

        public void ResetAll()
        {
            //Reset all labels and indataboxes
            ResetPatientInData();
            ResetGFRVal();
            ResetGFR();
            ResetKropp();
            ResetCINRiskFaktorer();
            ResetCINRisk();

            //Reset Jodkontrastmedel
            ResetAllJod();
        }
        public void ResetPatientInData() //Nollställer all indata
        {
            patientInstance = new PatientInstance();

            this.maskedPatientIndataPatientDataBirthdate.Text = DateTime.Today.ToString();
            patientFödelsedagValid = false;
            this.labelPatientIndataPatientDataAge.Text = "0";
            this.textBoxPatientIndataPatientDataHeight.Text = String.Empty;
            this.textBoxPatientIndataPatientDataWeight.Text = String.Empty;

            this.textBoxPatientIndataPatientDataKreatinin.Text = String.Empty;
            this.textBoxPatientIndataPatientDataCystatin.Text = String.Empty;

            this.radioPatientIndataSexKvinna.Checked = true;
        }
        public void ResetGFRVal()   //Nollställer eGFR att gå efter (medel, minsta, krea, cystatin)
        {
            this.panelChooseGFRToUseChoices.Enabled = false;
            this.radioButtonChooseGFRToUseMinsta.Checked = true;
        }
        public void ResetGFR()  //Nollställer alla labels i eGFR tabben
        {
            this.labelGFRLMAbsolut.Text = String.Empty;
            this.labelGFRLMULBMRelativt.Text = String.Empty;
            this.labelGFRLMMLBMRelativt.Text = String.Empty;

            this.labelGFRGrubbAbsolut.Text = String.Empty;
            this.labelGFRGrubbRelativt.Text = String.Empty;

            this.labelGFRLMGrubbAbsolut.Text = String.Empty;
            this.labelGFRLMGrubbRelativt.Text = String.Empty;

            this.labelPatientIndataCalculatedeGFR.Text = String.Empty;
            this.labelPatientIndataCalculatedNjurFunktion.Text = String.Empty;
        }
        public void ResetKropp()    //Nollställer alla labels i kroppsdata tabben
        {
            this.labelKroppBMI.Text = String.Empty;
            this.labelKroppBSA.Text = String.Empty;
            this.labelKroppIBW.Text = String.Empty;
            this.labelKroppLBM.Text = String.Empty;

            this.labelPatientIndataCalculatedBMI.Text = String.Empty;
        }
        public void ResetCINRiskFaktorer()  //Nollställer CIN Risk faktorer
        {
            patientInstance.AntalCINRiskFaktorer = 0;
            patientInstance.RiskFaktorCIN = false;

            this.checkBoxRiskFaktorerFaktorerGFR60.Checked = false;
            this.checkBoxRiskFaktorerFaktorerAge65.Checked = false;
            this.checkBoxRiskFaktorerFaktorerDiabetes.Checked = false;
            this.checkBoxRiskFaktorerFaktorerSvikt.Checked = false;
            this.checkBoxRiskFaktorerFaktorerGdJod.Checked = false;
            this.checkBoxRiskFaktorerFaktorerKirurgisktIngrepp.Checked = false;
            this.checkBoxRiskFaktorerFaktorerNSAIDCOX.Checked = false;
        }
        public void ResetCINRisk()  //Nollställer CIN Risken
        {
            this.labelRiskFaktorerCINRiskNivå.Text = String.Empty;
            this.labelRiskFaktorerCINRiskNivå.ForeColor = Color.Black;
            patientInstance.RiskFaktorCIN = false;
        }
        #endregion
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Beräkna information
        //
        //########################################################################
        #region Calculate Things
        private void AddPatientInfo()
        {
            patientInstance = new PatientInstance();


            if (this.labelPatientIndataPatientDataAge.Text != String.Empty)
                patientInstance.Age = Int32.Parse(this.labelPatientIndataPatientDataAge.Text);
            if (this.textBoxPatientIndataPatientDataHeight.Text != String.Empty)
            {
                try
                {
                    patientInstance.Height = Int32.Parse(this.textBoxPatientIndataPatientDataHeight.Text);
                }
                catch (Exception f)
                {
                    MessageBox.Show("Felaktig längd!",
                    "Viktigt Meddelande!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
            }
            if (this.textBoxPatientIndataPatientDataWeight.Text != String.Empty)
            {
                try
                {
                    patientInstance.Weight = Int32.Parse(this.textBoxPatientIndataPatientDataWeight.Text);
                }
                catch (Exception f)
                {
                    MessageBox.Show("Felaktig vikt!",
                    "Viktigt Meddelande!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
            }
            if (this.textBoxPatientIndataPatientDataKreatinin.Text != String.Empty)
            {
                try
                {
                    patientInstance.Kreatinin = Double.Parse(this.textBoxPatientIndataPatientDataKreatinin.Text);
                }
                catch (Exception f)
                {
                    MessageBox.Show("Felaktigt Kreatinin!",
                    "Viktigt Meddelande!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
            }
            if (this.textBoxPatientIndataPatientDataCystatin.Text != String.Empty)
            {
                try
                {
                    patientInstance.Cystatin = Double.Parse(this.textBoxPatientIndataPatientDataCystatin.Text);
                }
                catch (Exception f)
                {
                    MessageBox.Show("Felaktig Cystatin C!",
                    "Viktigt Meddelande!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
            }

            #region Patientkön
            if (radioPatientIndataSexKvinna.Checked)
                patientInstance.Sex = PatientGender.Kvinna;
            else if (radioPatientIndataSexMan.Checked)
                patientInstance.Sex = PatientGender.Man;
            #endregion
        }
        private void CalculateBodyInfo()
        {
            try
            {
                patientInstance.BMI = Calculate.BMI(patientInstance.Height, patientInstance.Weight);
            }
            catch (Exception f)
            {
                MessageBox.Show("Felaktig vid beräkning av BMI!",
                "Viktigt Meddelande!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }

            try
            {
                patientInstance.BSA = Calculate.BSA(patientInstance.Height, patientInstance.Weight);
            }
            catch (Exception f)
            {
                MessageBox.Show("Felaktig vid beräkning av BSA!",
                "Viktigt Meddelande!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }

            try
            {
                patientInstance.IBW = Calculate.IBW(patientInstance.Height, patientInstance.Sex);
            }
            catch (Exception f)
            {
                MessageBox.Show("Felaktig vid beräkning av IBW!",
                "Viktigt Meddelande!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }

            try
            {
                patientInstance.LBM = Calculate.LBM(patientInstance.Height, patientInstance.Weight, patientInstance.Sex);
            }
            catch (Exception f)
            {
                MessageBox.Show("Felaktig vid beräkning av LBM!",
                "Viktigt Meddelande!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }
        }
        private void CalculateGFR()
        {
            if (patientInstance.Kreatinin != 0)
            {
                patientInstance.EGFRLMAbsolut = Calculate.eGFRLMAbsolut(patientInstance.Age, patientInstance.Kreatinin, patientInstance.LBM);
                patientInstance.EGFRLMRelativtUtanLBM = Calculate.eGFRLMRelativtUtanLBM(patientInstance.Age, patientInstance.Kreatinin, patientInstance.Sex);
                patientInstance.EGFRLMRelativtMedLBM = Calculate.eGFRLMRelativtMedLBM(patientInstance.Age, patientInstance.Kreatinin, patientInstance.LBM);
            }
            else
            {
                patientInstance.EGFRLMAbsolut = 0;
                patientInstance.EGFRLMRelativtUtanLBM = 0;
                patientInstance.EGFRLMRelativtMedLBM = 0;
                this.labelPatientIndataCalculatedeGFR.Text = String.Empty;
            }

            if (patientInstance.Cystatin != 0)
            {
                patientInstance.EGFRGrubbRelativt = Calculate.eGFRGrubbRelativt(patientInstance.Cystatin, patientInstance.Sex);
                patientInstance.EGFRGrubbAbsolut = Calculate.eGFRAbsolutFrånRelativt(patientInstance.BSA, patientInstance.EGFRGrubbRelativt);
            }
            else
            {
                patientInstance.EGFRGrubbRelativt = 0;
                patientInstance.EGFRGrubbAbsolut = 0;
            }

            if (patientInstance.Kreatinin != 0 && patientInstance.Cystatin != 0)
            {
                if (patientInstance.Sex == PatientGender.Man)
                {
                    if (patientInstance.BMI > 20 && patientInstance.BMI < 30)
                    {
                        patientInstance.EGFRLMGrubbRelativt = (patientInstance.EGFRGrubbRelativt + patientInstance.EGFRLMRelativtUtanLBM) / 2;
                        patientInstance.EGFRLMGrubbMellanskillnad = Calculate.ProcenskillnadeGFRLMGrubbRelativt(patientInstance.EGFRGrubbRelativt, patientInstance.EGFRLMRelativtUtanLBM);
                    }
                    else
                    {
                        patientInstance.EGFRLMGrubbRelativt = (patientInstance.EGFRLMRelativtMedLBM + patientInstance.EGFRGrubbRelativt) / 2;
                        patientInstance.EGFRLMGrubbMellanskillnad = Calculate.ProcenskillnadeGFRLMGrubbRelativt(patientInstance.EGFRGrubbRelativt, patientInstance.EGFRLMRelativtMedLBM);
                    }

                }
                else
                {
                    patientInstance.EGFRLMGrubbRelativt = (patientInstance.EGFRLMRelativtUtanLBM + patientInstance.EGFRGrubbRelativt) / 2;
                    patientInstance.EGFRLMGrubbMellanskillnad = Calculate.ProcenskillnadeGFRLMGrubbRelativt(patientInstance.EGFRGrubbRelativt, patientInstance.EGFRLMRelativtUtanLBM);
                }

                patientInstance.EGFRLMGrubbAbsolut = (patientInstance.EGFRLMAbsolut + patientInstance.EGFRGrubbAbsolut) / 2;

            }
            else
            {
                patientInstance.EGFRLMGrubbAbsolut = 0;
                patientInstance.EGFRLMGrubbRelativt = 0;
                patientInstance.EGFRLMGrubbMellanskillnad = 0;
            }

            if (patientInstance.Cystatin == 0 && patientInstance.Kreatinin == 0)
            {
                MessageBox.Show("Du måste fylla i ett P-Kreatin eller P-Cystatin C värde för att kunna beräkna eGFR",
                "Viktigt Meddelande!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }
        }
        private void CalculateUsedGFR()
        {
            // Om bara kreatinin är ifyllt
            if (patientInstance.Kreatinin != 0 && patientInstance.Cystatin == 0)
            {
                if (patientInstance.Sex == PatientGender.Man)
                {
                    // Beräkna relativt eGFR med LM metoden utan LBM om patienten har ett BMI över 20 och under 30
                    if (patientInstance.BMI > 20 && patientInstance.BMI < 30)
                    {
                        patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtUtanLBM;
                    }
                    //Annars räknas det relativa eGFR ut med LM metoden och med LBM
                    else
                    {
                        patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtMedLBM;
                    }
                }
                else
                {
                    // relativt eGFR är LM metoden utan BLM
                    patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtUtanLBM;

                }

                //Sätt det absoluta eGFR till LM metoden
                patientInstance.UsedGFRAbsolute = patientInstance.EGFRLMAbsolut;
                //Stäng av panelen för vilket eGFR som kan väljas
                panelChooseGFRToUseChoices.Enabled = false;

            }

            //om endast Cystatin C är ifyllt
            else if (patientInstance.Kreatinin == 0 && patientInstance.Cystatin != 0)
            {
                //Sätter Absolut och Relativt eGFR till Grubbs ekvation
                patientInstance.UsedGFRAbsolute = patientInstance.EGFRGrubbAbsolut;
                patientInstance.UsedGFRRelative = patientInstance.EGFRGrubbRelativt;

                //Stäng av panelen för vilket eGFR som kan väljas
                panelChooseGFRToUseChoices.Enabled = false;
            }

            //Om både kreatinin och cystatin C är ifyllt
            else if (patientInstance.Kreatinin != 0 && patientInstance.Cystatin != 0)
            {

                this.panelChooseGFRToUseChoices.Enabled = true;
                this.radioButtonChooseGFRToUseMedel.Enabled = false;

                if (patientInstance.EGFRLMGrubbMellanskillnad < 0.4 && patientInstance.EGFRLMGrubbMellanskillnad > -0.4)
                {
                    this.radioButtonChooseGFRToUseMedel.Enabled = true;
                }
                if (patientInstance.EGFRLMGrubbMellanskillnad > 0.4 && patientInstance.EGFRLMGrubbMellanskillnad < -0.4)
                {
                    this.radioButtonChooseGFRToUseMedel.Enabled = false;
                }

                if (this.radioButtonChooseGFRToUseMedel.Checked && !this.radioButtonChooseGFRToUseMedel.Enabled)
                    this.radioButtonChooseGFRToUseMinsta.Checked = true;

                if (radioButtonChooseGFRToUseMedel.Checked)
                {
                    patientInstance.UsedGFRAbsolute = (patientInstance.EGFRLMAbsolut + patientInstance.EGFRGrubbAbsolut) / 2;

                    if (patientInstance.Sex == PatientGender.Man)
                    {
                        if (patientInstance.BMI > 20 && patientInstance.BMI < 30)
                        {
                            patientInstance.UsedGFRRelative = (patientInstance.EGFRLMRelativtUtanLBM + patientInstance.EGFRGrubbRelativt) / 2;
                        }
                        else
                        {
                            patientInstance.UsedGFRRelative = (patientInstance.EGFRLMRelativtMedLBM + patientInstance.EGFRGrubbRelativt) / 2;
                        }
                    }
                    else
                    {
                        patientInstance.UsedGFRRelative = (patientInstance.EGFRLMRelativtMedLBM + patientInstance.EGFRGrubbRelativt) / 2;
                    }
                }

                else if (radioButtonChooseGFRToUseMinsta.Checked)
                {
                    if (patientInstance.EGFRLMAbsolut < patientInstance.EGFRGrubbAbsolut)
                    {
                        patientInstance.UsedGFRAbsolute = patientInstance.EGFRLMAbsolut;
                    }
                    else if (patientInstance.EGFRLMAbsolut > patientInstance.EGFRGrubbAbsolut)
                    {
                        patientInstance.UsedGFRAbsolute = patientInstance.EGFRGrubbAbsolut;
                    }
                    if (patientInstance.Sex == PatientGender.Man)
                    {
                        if (patientInstance.BMI > 20 && patientInstance.BMI < 30)
                        {
                            if (patientInstance.EGFRLMRelativtUtanLBM < patientInstance.EGFRGrubbRelativt)
                            {
                                patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtUtanLBM;
                            }
                            else if (patientInstance.EGFRLMRelativtUtanLBM > patientInstance.EGFRGrubbRelativt)
                            {
                                patientInstance.UsedGFRRelative = patientInstance.EGFRGrubbRelativt;
                            }
                        }
                        else
                        {
                            if (patientInstance.EGFRLMRelativtMedLBM < patientInstance.EGFRGrubbRelativt)
                            {
                                patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtMedLBM;
                            }
                            else if (patientInstance.EGFRLMRelativtMedLBM > patientInstance.EGFRGrubbRelativt)
                            {
                                patientInstance.UsedGFRAbsolute = patientInstance.EGFRGrubbRelativt;
                            }
                        }
                    }
                    else
                    {
                        if (patientInstance.EGFRLMRelativtMedLBM < patientInstance.EGFRGrubbRelativt)
                        {
                            patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtMedLBM;
                        }
                        else if (patientInstance.EGFRLMRelativtMedLBM > patientInstance.EGFRGrubbRelativt)
                        {
                            patientInstance.UsedGFRRelative = patientInstance.EGFRGrubbRelativt;
                        }
                    }
                }

                else if (radioButtonChooseGFRToUseKrea.Checked)
                {
                    if (patientInstance.Sex == PatientGender.Man)
                    {
                        if (patientInstance.BMI > 20 && patientInstance.BMI < 30)
                        {
                            patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtUtanLBM;
                        }
                        else
                        {
                            patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtMedLBM;
                        }
                    }
                    else
                    {
                        patientInstance.UsedGFRRelative = patientInstance.EGFRLMRelativtUtanLBM;
                    }

                    patientInstance.UsedGFRAbsolute = patientInstance.EGFRLMAbsolut;

                }

                else if (radioButtonChooseGFRToUseCysta.Checked)
                {
                    patientInstance.UsedGFRAbsolute = patientInstance.EGFRGrubbAbsolut;
                    patientInstance.UsedGFRRelative = patientInstance.EGFRGrubbRelativt;
                }

            }
        }
        private void CalculateCINRiskFactors()
        {
            #region CIN Riskfaktorer

            patientInstance.AntalCINRiskFaktorer = 0;

            if (this.checkBoxRiskFaktorerFaktorerGFR60.Checked)
                patientInstance.AntalCINRiskFaktorer += 1;

            if (this.checkBoxRiskFaktorerFaktorerAge65.Checked)
                //patientInstance.AntalCINRiskFaktorer += 1;

            if (this.checkBoxRiskFaktorerFaktorerDiabetes.Checked)
            {
                if (patientInstance.UsedGFRAbsolute < 90)
                    patientInstance.AntalCINRiskFaktorer += 1;
            }

            if (this.checkBoxRiskFaktorerFaktorerSvikt.Checked)
                patientInstance.AntalCINRiskFaktorer += 1;

            if (this.checkBoxRiskFaktorerFaktorerGdJod.Checked)
                patientInstance.AntalCINRiskFaktorer += 1;

            if (this.checkBoxRiskFaktorerFaktorerKirurgisktIngrepp.Checked)
                patientInstance.AntalCINRiskFaktorer += 1;

            if (this.checkBoxRiskFaktorerFaktorerNSAIDCOX.Checked)
                patientInstance.AntalCINRiskFaktorer += 1;

            //labelUtveckladAv.Text = Convert.ToString(patientInstance.AntalCINRiskFaktorer);
            #endregion
        }
        #endregion
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Uppdaterar information i MainForm
        //
        //########################################################################
        #region Uppdaterar information i MainForm
        public void UpdateGFR()
        {
            if (patientInstance.EGFRLMAbsolut != 0)
                this.labelGFRLMAbsolut.Text = Convert.ToString(Math.Round(patientInstance.EGFRLMAbsolut, 1));
            else
                this.labelGFRLMAbsolut.Text = String.Empty;

            if (patientInstance.EGFRLMRelativtUtanLBM != 0)
                this.labelGFRLMULBMRelativt.Text = Convert.ToString(Math.Round(patientInstance.EGFRLMRelativtUtanLBM, 1));
            else
                this.labelGFRLMMLBMRelativt.Text = String.Empty;

            if (patientInstance.EGFRLMRelativtMedLBM != 0)
                this.labelGFRLMMLBMRelativt.Text = Convert.ToString(Math.Round(patientInstance.EGFRLMRelativtMedLBM, 1));
            else
                this.labelGFRLMMLBMRelativt.Text = String.Empty;



            if (patientInstance.EGFRGrubbAbsolut != 0)
                this.labelGFRGrubbAbsolut.Text = Convert.ToString(Math.Round(patientInstance.EGFRGrubbAbsolut, 1));
            else
                this.labelGFRGrubbAbsolut.Text = String.Empty;

            if (patientInstance.EGFRGrubbRelativt != 0)
                this.labelGFRGrubbRelativt.Text = Convert.ToString(Math.Round(patientInstance.EGFRGrubbRelativt, 1));
            else
                this.labelGFRGrubbRelativt.Text = String.Empty;


            if (patientInstance.EGFRLMGrubbAbsolut != 0)
                this.labelGFRLMGrubbAbsolut.Text = Convert.ToString(Math.Round(patientInstance.EGFRLMGrubbAbsolut, 1));
            else
                this.labelGFRLMGrubbAbsolut.Text = String.Empty;

            if (patientInstance.EGFRLMGrubbRelativt != 0)
                this.labelGFRLMGrubbRelativt.Text = Convert.ToString(Math.Round(patientInstance.EGFRLMGrubbRelativt, 1));
            else
                this.labelGFRLMGrubbRelativt.Text = String.Empty;

            if (patientInstance.UsedGFRAbsolute != 0)
                this.labelPatientIndataCalculatedeGFR.Text = Convert.ToString(Math.Round(patientInstance.UsedGFRAbsolute, MidpointRounding.AwayFromZero));
            else
                this.labelPatientIndataCalculatedeGFR.Text = String.Empty;
        }
        public void UpdateKropp()
        {
            if (patientInstance.BMI != 0)
                this.labelKroppBMI.Text = Convert.ToString(Math.Round(patientInstance.BMI, 1));
            else
                this.labelKroppBMI.Text = String.Empty;

            if (patientInstance.BSA != 0)
                this.labelKroppBSA.Text = Convert.ToString(Math.Round(patientInstance.BSA, 2));
            else
                this.labelKroppBSA.Text = String.Empty;

            if (patientInstance.IBW != 0)
                this.labelKroppIBW.Text = Convert.ToString(Math.Round(patientInstance.IBW, 1));
            else
                this.labelKroppIBW.Text = String.Empty;

            if (patientInstance.LBM != 0)
                this.labelKroppLBM.Text = Convert.ToString(Math.Round(patientInstance.LBM, 1));
            else
                this.labelKroppLBM.Text = String.Empty;
        }
        public void UpdateUsedGFR()
        {
            CalculateUsedGFR();     //Beräknar GFR som ska användas
            BeräknaNjurFunktion();  //Beräknar vilken Njurfunktionsklass patienten befinner sig i

            UpdateGFR();
        }
        public void UpdateCINRiskFaktorer()
        {
            if (Math.Round(patientInstance.UsedGFRAbsolute, MidpointRounding.AwayFromZero) < 60 && patientInstance.UsedGFRAbsolute != 0)
                this.checkBoxRiskFaktorerFaktorerGFR60.Checked = true;
            else
                this.checkBoxRiskFaktorerFaktorerGFR60.Checked = false;

            if (patientInstance.Age >= 65)
                this.checkBoxRiskFaktorerFaktorerAge65.Checked = true;
            else
                this.checkBoxRiskFaktorerFaktorerAge65.Checked = false;
        }
        #endregion
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Beräknar Olika Klasser
        //
        //########################################################################
        private void BeräknaBMIKlass()
        {
            //Beräknar vilken BMI klass patienten tillhör och ger den en färg
            if (patientInstance.BMI < 18.49)
            {
                labelPatientIndataCalculatedBMI.Text = "Undervikt";   //underviktig
                labelPatientIndataCalculatedBMI.ForeColor = Color.Goldenrod;
            }
            else if (patientInstance.BMI >= 18.5 && patientInstance.BMI <= 24.99)
            {
                labelPatientIndataCalculatedBMI.Text = "Normalt";   //normalviktig
                labelPatientIndataCalculatedBMI.ForeColor = Color.Black;
            }
            else if (patientInstance.BMI >= 25.0 && patientInstance.BMI <= 29.99)
            {
                labelPatientIndataCalculatedBMI.Text = "Övervikt";   //överviktig
                labelPatientIndataCalculatedBMI.ForeColor = Color.Goldenrod;
            }
            else if (patientInstance.BMI >= 30.0 && patientInstance.BMI <= 34.99)
            {
                labelPatientIndataCalculatedBMI.Text = "Fetma grad 1";   //fetma grad 1
                labelPatientIndataCalculatedBMI.ForeColor = Color.Orange;
            }
            else if (patientInstance.BMI >= 35.0 && patientInstance.BMI <= 39.99)
            {
                labelPatientIndataCalculatedBMI.Text = "Fetma grad 2";   //fetma grad 2
                labelPatientIndataCalculatedBMI.ForeColor = Color.OrangeRed;
            }
            else if (patientInstance.BMI >= 40)
            {
                labelPatientIndataCalculatedBMI.Text = "Fetma grad 3";   //fetma grad 3
                labelPatientIndataCalculatedBMI.ForeColor = Color.Red;
            }
            else
            {
                labelPatientIndataCalculatedBMI.Text = String.Empty;
                labelPatientIndataCalculatedBMI.ForeColor = Color.Black;
            }
        }
        private void BeräknaNjurFunktion()
        {
            // Räknar ut vilken njurfunktion patienten har
            if (patientInstance.UsedGFRRelative >= 90.0)
            {
                labelPatientIndataCalculatedNjurFunktion.Text = "Normal";
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.Black;
            }
            else if (patientInstance.UsedGFRRelative <= 89.9 && patientInstance.UsedGFRRelative >= 60.0)
            {
                labelPatientIndataCalculatedNjurFunktion.Text = "Lätt nedsatt";
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.Goldenrod;
            }
            else if (patientInstance.UsedGFRRelative <= 59.9 && patientInstance.UsedGFRRelative >= 30.0)
            {
                labelPatientIndataCalculatedNjurFunktion.Text = "Måttligt nedsatt";
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.Orange;
            }
            else if (patientInstance.UsedGFRRelative <= 29.9 && patientInstance.UsedGFRRelative >= 15.0)
            {
                labelPatientIndataCalculatedNjurFunktion.Text = "Kraftigt nedsatt";
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.OrangeRed;
            }
            else if (patientInstance.UsedGFRRelative <= 14.9 && patientInstance.UsedGFRRelative > 0)
            {
                labelPatientIndataCalculatedNjurFunktion.Text = "Uremi";
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.Red;
            }
            else if (patientInstance.UsedGFRRelative == 0)
            {
                labelPatientIndataCalculatedNjurFunktion.Text = String.Empty;
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.Black;
            }
            else
            {
                labelPatientIndataCalculatedNjurFunktion.Text = String.Empty;
                labelPatientIndataCalculatedNjurFunktion.ForeColor = Color.Black;
            }
        }
        private void BeräknaCINRisk()
        {
            if (patientInstance.AntalCINRiskFaktorer != 0)
            {
                if (patientInstance.UsedGFRAbsolute != 0 && patientInstance.UsedGFRAbsolute <= 45)
               {
                   this.labelRiskFaktorerCINRiskNivå.Text = "Hög\r\n(Bör hydreras!)";
                   this.labelRiskFaktorerCINRiskNivå.ForeColor = Color.Red;
                   patientInstance.RiskFaktorCIN = true;

               }
                else if (patientInstance.AntalCINRiskFaktorer > 1)
               {
                   this.labelRiskFaktorerCINRiskNivå.Text = "Hög\r\n(Bör hydreras!)";
                   this.labelRiskFaktorerCINRiskNivå.ForeColor = Color.Red;
                   patientInstance.RiskFaktorCIN = true;
               }
                else if (patientInstance.UsedGFRAbsolute > 45 && patientInstance.UsedGFRAbsolute <= 60 && patientInstance.AntalCINRiskFaktorer < 2)
               {
                   this.labelRiskFaktorerCINRiskNivå.Text = "Förhöjd";
                   this.labelRiskFaktorerCINRiskNivå.ForeColor = Color.Goldenrod;
                   patientInstance.RiskFaktorCIN = false;
               }
                else if (patientInstance.AntalCINRiskFaktorer < 2)
               {
                   this.labelRiskFaktorerCINRiskNivå.Text = "Förhöjd";
                   this.labelRiskFaktorerCINRiskNivå.ForeColor = Color.Goldenrod;
                   patientInstance.RiskFaktorCIN = false;
               }
               else
               {
                   ResetCINRisk();
               }
            }
            else
            {
                ResetCINRisk();
            }
        }
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Eventhanterare
        //
        //########################################################################
        #region Eventhanterare

        //###############
        //###############
        private void StartEventHandlers()
        {
            //Hanterar så att alla beräkningar försvinner och att de stämmer om man ändrar i rutorna
            this.maskedPatientIndataPatientDataBirthdate.ValidatingType = typeof(System.DateTime);
            this.maskedPatientIndataPatientDataBirthdate.TypeValidationCompleted += new TypeValidationEventHandler(maskedPatientIndataPatientDataBirthdate_TypeValidationCompleted);
            this.maskedPatientIndataPatientDataBirthdate.MouseClick += new MouseEventHandler(maskedPatientIndataPatientDataBirthdate_Click);
            this.maskedPatientIndataPatientDataBirthdate.TextChanged += new System.EventHandler(this.txtBox_TextChanged);

            this.textBoxPatientIndataPatientDataHeight.KeyPress += new KeyPressEventHandler(this.txtBox_KeyPress);
            this.textBoxPatientIndataPatientDataHeight.TextChanged += new System.EventHandler(this.txtBox_TextChanged);

            this.textBoxPatientIndataPatientDataWeight.KeyPress += new KeyPressEventHandler(this.txtBoxComma_KeyPress);
            this.textBoxPatientIndataPatientDataWeight.TextChanged += new System.EventHandler(this.txtBox_TextChanged);

            this.textBoxPatientIndataPatientDataKreatinin.KeyPress += new KeyPressEventHandler(this.txtBox_KeyPress);
            this.textBoxPatientIndataPatientDataKreatinin.TextChanged += new System.EventHandler(this.txtBox_TextChanged);

            this.textBoxPatientIndataPatientDataCystatin.KeyPress += new KeyPressEventHandler(this.txtBoxComma_KeyPress);
            this.textBoxPatientIndataPatientDataCystatin.TextChanged += new System.EventHandler(this.txtBox_TextChanged);

            this.radioPatientIndataSexKvinna.CheckedChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.radioPatientIndataSexMan.CheckedChanged += new System.EventHandler(this.txtBox_TextChanged);

            //Hanterar ändring av eGFR att använda när både krea + cystatin används
            this.radioButtonChooseGFRToUseMedel.CheckedChanged += new System.EventHandler(this.radioButton_eGFRChoiceChanged);
            this.radioButtonChooseGFRToUseMinsta.CheckedChanged += new System.EventHandler(this.radioButton_eGFRChoiceChanged);
            this.radioButtonChooseGFRToUseKrea.CheckedChanged += new System.EventHandler(this.radioButton_eGFRChoiceChanged);
            this.radioButtonChooseGFRToUseCysta.CheckedChanged += new System.EventHandler(this.radioButton_eGFRChoiceChanged);

            //Hanterar ändring av CIN Riskfaktoror Checkboxarna
            this.checkBoxRiskFaktorerFaktorerGFR60.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
            this.checkBoxRiskFaktorerFaktorerAge65.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
            this.checkBoxRiskFaktorerFaktorerDiabetes.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
            this.checkBoxRiskFaktorerFaktorerSvikt.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
            this.checkBoxRiskFaktorerFaktorerGdJod.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
            this.checkBoxRiskFaktorerFaktorerKirurgisktIngrepp.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
            this.checkBoxRiskFaktorerFaktorerNSAIDCOX.CheckedChanged += new System.EventHandler(this.checkBox_CINRiskFactorChanged);
        }
        private void StartEventHandlersJod()
        {
            this.textBoxJodGramPerKilo.KeyPress += new KeyPressEventHandler(this.txtBoxComma_KeyPress);
            this.textBoxJodTotalGram.KeyPress += new KeyPressEventHandler(this.txtBoxComma_KeyPress);

            //Kontrollerar om ett dosprotokoll eller jodkontrastdossen ändrats
            this.radioButtonJodVikt.CheckedChanged += new System.EventHandler(this.comboBoxJodkoncentration_SelectionChangeCommitted);
            this.radioButtonJodIBW.CheckedChanged += new System.EventHandler(this.comboBoxJodkoncentration_SelectionChangeCommitted);
            this.comboBoxJodkoncentration.SelectionChangeCommitted += new System.EventHandler(this.comboBoxJodkoncentration_SelectionChangeCommitted);
            this.comboBoxDosProtokoll.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDosProtokoll_SelectionChangeCommitted);
            this.comboBoxPVKMärke.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPVKMärke_SelectionChangeCommitted);
        }
        //###############
        //###############



        //###############
        //###############
        #region eventhanterare för födelsedagen
        private void maskedPatientIndataPatientDataBirthdate_TypeValidationCompleted(object sender, TypeValidationEventArgs y)
        {
                if (!y.IsValidInput)
                {
                    MessageBox.Show("Felaktigt datum! Rätt format: år-månad-dag (ex: 2012-03-11)",
                                    "Viktigt Meddelande!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation,
                                    MessageBoxDefaultButton.Button1);
                    this.maskedPatientIndataPatientDataBirthdate.Text = DateTime.Now.ToString();
                    this.labelPatientIndataPatientDataAge.Text = "0";
                    patientFödelsedagValid = false;
                }
                else if (this.maskedPatientIndataPatientDataBirthdate.Text != "")
                {
                    //Now that the type has passed basic type validation, enforce more specific type rules.
                    DateTime userDate = (DateTime)y.ReturnValue;
                    DateTime n = DateTime.Now;
                    if (userDate >= n)
                    {
                        MessageBox.Show("Felaktigt datum! Datumet måste vara före dagens datum!",
                                        "Viktigt Meddelande!",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button1);
                        this.maskedPatientIndataPatientDataBirthdate.Text = DateTime.Now.ToString();
                        this.labelPatientIndataPatientDataAge.Text = "0";
                        patientFödelsedagValid = false;
                    }
                    else
                    {
                        this.labelPatientIndataPatientDataAge.Text = Convert.ToString(Calculate.Ålder(userDate));
                        patientFödelsedagValid = true;
                    }
                }
        }


        private void maskedPatientIndataPatientDataBirthdate_Click(object sender, EventArgs e)
        {
            this.maskedPatientIndataPatientDataBirthdate.SelectAll();
        }

        #endregion
        //###############
        //###############



        //###############
        //###############
        #region Eventhanterare för när texten i inmatningsrutorna ändras
        private void txtBox_KeyPress(object sender, KeyPressEventArgs e) //Ser till att endas siffror skrivs in
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtBoxComma_KeyPress(object sender, KeyPressEventArgs e) //Ser till att endas siffror skrivs in
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == ','
                && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtBox_TextChanged(Object sender, EventArgs e)  //Eventhanterare som hanterar vad som ska hända när användaren ändrar på användarinmatad data
        {
            //Om värdena i textboxarna ändras, nollställ beräknad data
            patientInstance = new PatientInstance();
            ResetKropp();
            ResetGFRVal();
            ResetGFR();
            ResetCINRisk();
            ResetCINRiskFaktorer();

            //Reset Jodkontrastmedel
            ResetAllJod();
        }

        #endregion


        #region eventhanterare för ändring i dosprotokoll, jodkoncentrationsrullistan & PVKListan
        private void comboBoxJodkoncentration_SelectionChangeCommitted(Object sender, EventArgs e)  //Eventhanterare som hanterar vad som ska hända när användaren ändrar födeledag
        {
            if (patientInstance.Weight != 0)
            {
                //Om användare ändrar jodkontrastkoncentration så updateras alla beräkningar
                AddDosProtokollInfo();

                //Beräknar maxdosen för jodkontrast
                CalculateMaxDosJod();
                UpdateMaxDosJod();

                //Beräknar Kontrastdos via dosprotokoll
                CalculateJodKontrastDos();
                UpdateJodKontrastDos();
                BeräknaPVKFärg();
                UpdatePVKFärg();
            }
        }

        private void comboBoxDosProtokoll_SelectionChangeCommitted(Object sender, EventArgs e)  //Eventhanterare som hanterar vad som ska hända när användaren ändrar födeledag
        {
            if (patientInstance.Weight != 0)
            {
                //Om användaren ändrar dosprotokollet så uppdateras alla beräkningar
                //HämtaJodDosProtokoll();
                AddDosProtokollInfo();
                //Variables.jodkoncentration = Convert.ToInt32(Variables.dosprotokollJodkoncentration);
                if (jodDosprotokoll.DosprotokollID != 0)
                    this.comboBoxJodkoncentration.SelectedIndex = comboBoxJodkoncentration.FindString(Convert.ToString(jodDosprotokoll.DosprotokollJodkoncentration));

                //Beräknar maxdosen för jodkontrast
                CalculateMaxDosJod();
                UpdateMaxDosJod();

                //Beräknar Kontrastdos via dosprotokoll
                CalculateJodKontrastDos();
                UpdateJodKontrastDos();
                BeräknaPVKFärg();
                UpdatePVKFärg();
            }
        }
        private void comboBoxPVKMärke_SelectionChangeCommitted(Object sender, EventArgs e)  //Eventhanterare som hanterar vad som ska hända när användaren ändrar födeledag
        {
            if (patientInstance.Weight != 0)
            {
                //Beräknar PVK Färgen
                BeräknaPVKFärg();
                UpdatePVKFärg();
            }
        }
        #endregion


        private void radioButton_eGFRChoiceChanged(object sender, EventArgs e)
        {
            UpdateUsedGFR();
            UpdateCINRiskFaktorer();
            CalculateCINRiskFactors();  //Beräknar antal riskfaktorer patienten har
            BeräknaCINRisk();       //Beräknar vilken risk för CIN patienten har

            //Jodkontrastmedel
            CalculateMaxDosJod();
            UpdateMaxDosJod();
            UpdateJodKontrastDos();
            BeräknaPVKFärg();
            UpdatePVKFärg();
        }

        private void checkBox_CINRiskFactorChanged(object sender, EventArgs e)
        {
            if (patientFödelsedagValid)
            {
                if (patientInstance.Weight != 0 && patientInstance.Height != 0)
                {
                    CalculateCINRiskFactors();  //Beräknar antal riskfaktorer patienten har
                    BeräknaCINRisk();           //Beräknar vilken risk för CIN patienten har

                    //Jodkontrastmedel
                    CalculateMaxDosJod();
                    UpdateMaxDosJod();
                    UpdateJodKontrastDos();
                    BeräknaPVKFärg();
                    UpdatePVKFärg();
                }
            }
        }
        //###############
        //###############



        //###############
        //###############
        #region Eventhanterare för när man trycker på Beräkna samt Rensa
        private void buttonPatientIndataCalculate_Click(object sender, EventArgs e)
        {
            if (patientFödelsedagValid)
            {
                //Lägger till all information i patientinstansen
                AddPatientInfo();

                if (patientInstance.Weight != 0 && patientInstance.Height != 0)
                {
                    //Beräknar kroppsinformation (IBW,LSA mm)
                    CalculateBodyInfo();
                    //Beräknar vilken BMI klass patienten befinner sig i
                    BeräknaBMIKlass();

                    //Beräknar GFR
                    CalculateGFR();
                    CalculateUsedGFR();     //Beräknar GFR som ska användas

                    BeräknaNjurFunktion();  //Beräknar vilken Njurfunktionsklass patienten befinner sig i
                    UpdateCINRiskFaktorer();
                    CalculateCINRiskFactors();  //Beräknar antal riskfaktorer patienten har
                    BeräknaCINRisk();       //Beräknar vilken risk för CIN patienten har

                    //Jodkontrastmedel
                    CalculateMaxDosJod();
                    UpdateMaxDosJod();
                }
                else
                {
                    MessageBox.Show("Du måste fylla i längd/vikt och krea eller cystatin samt ett födelsedatum för att kunna beräkna eGFR",
                    "Viktigt Meddelande!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);

                    patientInstance = new PatientInstance();
                }
            }

            //Updaterar Forms
            UpdateKropp();
            UpdateGFR();
        }
        private void buttonPatientIndataClear_Click(object sender, EventArgs e)
        {
            ResetAll();
            ResetKontrastDosEnligtGram();
        }

        //För att beräkna kontrastdos med gram jod
        private void buttonBeräknaJodGKG_Click(object sender, EventArgs e)
        {
            if (this.textBoxPatientIndataPatientDataWeight.Text != String.Empty && this.textBoxJodGramPerKilo.Text != String.Empty)
            {
                double jodGKG = Convert.ToDouble(this.textBoxJodGramPerKilo.Text);
                double weight = Convert.ToDouble(this.textBoxPatientIndataPatientDataWeight.Text);
                double Jodkoncentration = Convert.ToDouble(this.comboBoxJodkoncentration.Text);

                this.labelJodKontrastDos.Text = Convert.ToString(Math.Round((jodGKG * weight) * 1000 / Jodkoncentration, MidpointRounding.AwayFromZero));
            }

        }

        private void buttonBeräknaJodG_Click(object sender, EventArgs e)
        {
            if (this.textBoxJodTotalGram.Text != String.Empty)
            {
                double jodG = Convert.ToDouble(this.textBoxJodTotalGram.Text);
                double Jodkoncentration = Convert.ToDouble(this.comboBoxJodkoncentration.Text);

                this.labelJodKontrastDos.Text = Convert.ToString(Math.Round((jodG * 1000) / Jodkoncentration, MidpointRounding.AwayFromZero));
            }
        }
        #endregion
        //###############
        //###############

        //###############
        //###############
        #region Eventhanterare för när man byter panel (GFR, Gd, Jod mm)
        private void buttonPanelJodkontrast_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPageJodKontrast;
        }

        private void buttonPanelGadoliniumkontrast_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPageGdKontrast;
        }

        private void buttonPanelKropp_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPageKropp;
        }

        private void buttonPanelGFR_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPageGFR;
        }

        private void buttonPanelKlassifikationer_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPageKlassifikationer;
        }
        #endregion

        //###############
        //###############

        #endregion
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Jodkontrastmedel



        //########################################################################
        //
        //Nollställer information i Jodkontrasttabben
        //
        //########################################################################
        #region Nollställer informationen under jodkontrasttabben
        public void ResetAllJod()
        {
            //Reset Jodkontrastmedel
            ResetMaxPatientDos();
            ResetKontrastDosEnligtDosProtokoll();
            //ResetKontrastDosEnligtGram();
            ResetJodkontrastParametrar();
            ResetJodKontrastDos();
            ResetPVKFärg();
        }
        public void ResetMaxPatientDos()
        {
            patientInstance.MaxJodDos = 0;
            patientInstance.MaxJodDosGram = 0;

            this.labelMaxJodDosML.Text = String.Empty;
            this.labelMaxJodDosG.Text = String.Empty;
            this.labelMaxJodDosHalveradDos.Visible = false;
        }
        public void ResetKontrastDosEnligtDosProtokoll()
        {
            this.radioButtonJodVikt.Checked = true;

            this.labelJodKontrastDos.Text = String.Empty;
            this.labelJodKontrastHastighet.Text = String.Empty;

        }
        public void ResetKontrastDosEnligtGram()
        {
            this.textBoxJodGramPerKilo.Text = String.Empty;
            this.textBoxJodTotalGram.Text = String.Empty;
        }
        public void ResetJodkontrastParametrar()
        {
            this.comboBoxDosProtokoll.SelectedIndex = 0;
            this.comboBoxJodkoncentration.SelectedIndex = comboBoxJodkoncentration.FindString(Convert.ToString(settings.Jodkoncentration));
            this.comboBoxPVKMärke.SelectedIndex = comboBoxPVKMärke.FindString(Convert.ToString(settings.PVK));
            this.radioButtonJodVikt.Checked = true;
            this.labelPVKFärg.Text = String.Empty;
        }
        public void ResetJodKontrastDos()
        {
            patientInstance.JodKontrastDos = 0;
            patientInstance.JodKontrastHastighet = 0;
            this.labelJodKontrastDos.Text = String.Empty;
            this.labelJodKontrastDos.ForeColor = Color.Black;
            this.labelJodKontrastHastighet.Text = String.Empty;
        }
        public void ResetPVKFärg()
        {
            this.labelPVKFärg.Text = String.Empty;
            this.labelPVKFärg.ForeColor = Color.Black;
            this.labelPVKFärg.BackColor = Color.White;
            patientInstance.PVKFärg = String.Empty;
        }
        #endregion
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Beräkna information
        //
        //########################################################################
        #region Beräknar information under jodkontrasttabben
        public void AddDosProtokollInfo()
        {
            jodDosprotokoll = new JodDosprotokoll();

            if (comboBoxDosProtokoll.SelectedIndex != 0)    //om rulllistans alternativ inte är 0 = tomt
            {
                int selectedID = ((ComboBoxItem)comboBoxDosProtokoll.SelectedItem).ID;
                #region skriver ut de nyinlästa objectlistorna från joddosprotokoll.xml
                dosProtokollen.ForEach(delegate(ProtokollJodkontrast p)
                {
                    //if (p.ID == comboBoxDosProtokoll.SelectedIndex)
                    if (p.ID == selectedID)
                    {
                        jodDosprotokoll.DosprotokollID = p.ID;
                        jodDosprotokoll.DosprotokollProtokollNamn = p.ProtokollNamn;
                        jodDosprotokoll.DosprotokollJodkoncentration = p.JodKoncentration;
                        jodDosprotokoll.DosprotokollDosKg = p.DosPerKg;
                        jodDosprotokoll.DosprotokollInjektionstid = p.Injektionstid;
                        jodDosprotokoll.DosprotokollMaxVikt = p.MaxVikt;
                        jodDosprotokoll.DosprotokollAnteckningar = p.Anteckningar;
                    }

                });
                #endregion
            }
        }
        public void CalculateMaxDosJod()
        {
            if (patientInstance.UsedGFRAbsolute != 0)
            {
                if (!patientInstance.RiskFaktorCIN)
                {
                    patientInstance.MaxJodDos = Calculate.PatientMaxDosJod(patientInstance.UsedGFRAbsolute, Convert.ToInt32(this.comboBoxJodkoncentration.Text));
                    patientInstance.MaxJodDosGram = patientInstance.UsedGFRAbsolute;
                }
                if (patientInstance.RiskFaktorCIN)
                {
                    patientInstance.MaxJodDos = Calculate.PatientMaxDosJod(patientInstance.UsedGFRAbsolute, Convert.ToInt32(this.comboBoxJodkoncentration.Text)) / 2;
                    patientInstance.MaxJodDosGram = patientInstance.UsedGFRAbsolute / 2;
                }

            }
            else
            {
                ResetMaxPatientDos();
            }
        }
        public void CalculateJodKontrastDos()
        {
            if (patientInstance.Weight != 0 && patientInstance.BMI !=0)
            {
                //Räknar på Patientens Vikt
                if (this.radioButtonJodVikt.Checked)
                {
                    if (patientInstance.Age <= settings.BarnMaxÅlder && patientInstance.Weight  <= settings.BarnMaxVikt)
                    {
                        //Variables.patientKontrastDos = (Variables.patientVikt * Variables.barnJodDosPerKilo) / Variables.jodkoncentration;
                        patientInstance.JodKontrastDos = (patientInstance.Weight * settings.BarnJodDosPerKilo) / Convert.ToInt32(this.comboBoxJodkoncentration.Text);
                    }
                    else
                    {
                        //Om patientens vikt är mindre eller lika med dosprotokollets maxvikt så räknas på vikten
                        if (patientInstance.Weight <= jodDosprotokoll.DosprotokollMaxVikt)
                        {
                            //Variables.patientKontrastDos = (Variables.dosprotokollDosKg * Variables.patientVikt) / Variables.jodkoncentration;
                            patientInstance.JodKontrastDos = (patientInstance.Weight * jodDosprotokoll.DosprotokollDosKg) / Convert.ToInt32(this.comboBoxJodkoncentration.Text);
                        }
                        //Annars räknas på dosprotokollets maxdos
                        else
                        {
                            //Variables.patientKontrastDos = (Variables.dosprotokollDosKg * Variables.dosprotokollMaxVikt) / Variables.jodkoncentration;
                            patientInstance.JodKontrastDos = (jodDosprotokoll.DosprotokollMaxVikt * jodDosprotokoll.DosprotokollDosKg) / Convert.ToInt32(this.comboBoxJodkoncentration.Text);
                        }
                    }
                }
                //Räknar på IBW
                if (this.radioButtonJodIBW.Checked)
                {
                    if (patientInstance.Age <= settings.BarnMaxÅlder && patientInstance.IBW <= settings.BarnMaxVikt)
                    {
                        //Variables.patientKontrastDos = (Variables.patientIBW * Variables.barnJodDosPerKilo) / Variables.jodkoncentration;
                        patientInstance.JodKontrastDos = (patientInstance.IBW * settings.BarnJodDosPerKilo) / Convert.ToInt32(this.comboBoxJodkoncentration.Text);
                    }
                    else
                    {
                        //Om IBW är mindre eller lika med dosprotokollets maxvikt
                        //if (Variables.patientIBW <= Variables.dosprotokollMaxVikt)
                        if (patientInstance.IBW <= jodDosprotokoll.DosprotokollMaxVikt)
                        {
                            //Räkna på IBW
                            //Variables.patientKontrastDos = (Variables.dosprotokollDosKg * Variables.patientIBW) / Variables.jodkoncentration;
                            patientInstance.JodKontrastDos = (patientInstance.IBW * jodDosprotokoll.DosprotokollDosKg) / Convert.ToInt32(this.comboBoxJodkoncentration.Text);
                        }
                        else
                        {
                            //Annars räkna på protokollets Maxdos
                            //Variables.patientKontrastDos = (Variables.dosprotokollDosKg * Variables.dosprotokollMaxVikt) / Variables.jodkoncentration;
                            patientInstance.JodKontrastDos = (jodDosprotokoll.DosprotokollMaxVikt * jodDosprotokoll.DosprotokollDosKg) / Convert.ToInt32(this.comboBoxJodkoncentration.Text);
                        }
                    }
                }
                //Räknar ut injektionshastighet och doshastighet
                if (patientInstance.Age <= settings.BarnMaxÅlder && patientInstance.Weight <= settings.BarnMaxVikt)
                {
                    //Variables.patientInjektionshastighet = (Variables.patientKontrastDos / Variables.barnInjektionstid);
                    patientInstance.JodKontrastHastighet = (patientInstance.JodKontrastDos / settings.BarnInjektionstid);
                }
                else
                {
                    //Variables.patientInjektionshastighet = Variables.patientKontrastDos / Variables.dosprotokollInjektionstid;
                    patientInstance.JodKontrastHastighet = (patientInstance.JodKontrastDos / jodDosprotokoll.DosprotokollInjektionstid);
                }
                //Variables.dosprotokollDoshastighet = Variables.dosprotokollDosKg / Variables.dosprotokollInjektionstid;
            }
        }
        #endregion
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Uppdaterar information i MainForm
        //
        //########################################################################
        public void UpdateMaxDosJod()
        {
            if (patientInstance.UsedGFRAbsolute !=0 && patientInstance.MaxJodDos !=0)
            {
                this.labelMaxJodDosML.Text = Convert.ToString(Math.Round(patientInstance.MaxJodDos, 0));
                this.labelMaxJodDosG.Text = Convert.ToString(Math.Round(patientInstance.MaxJodDosGram, 0));


                if (patientInstance.RiskFaktorCIN)
                    this.labelMaxJodDosHalveradDos.Visible = true;
                if (!patientInstance.RiskFaktorCIN)
                    this.labelMaxJodDosHalveradDos.Visible = false;
            }
            else
            {
                ResetMaxPatientDos();
            }
        }
        public void UpdateJodKontrastDos()
        {
            if (patientInstance.JodKontrastDos != 0)
            {
                this.labelJodKontrastDos.Text = Convert.ToString(Math.Round(patientInstance.JodKontrastDos, 0));
                if (patientInstance.JodKontrastDos > patientInstance.MaxJodDos)
                    this.labelJodKontrastDos.ForeColor = Color.Red;
                else
                    this.labelJodKontrastDos.ForeColor = Color.Black;
            }
            else
            {
                ResetJodKontrastDos();
            }

            if (patientInstance.JodKontrastHastighet != 0)
            {
                this.labelJodKontrastHastighet.Text = Convert.ToString(Math.Round(patientInstance.JodKontrastHastighet, 1));
            }
            else
            {
                ResetJodKontrastDos();
            }
        }
        public void UpdatePVKFärg()
        {
            #region//PVKStorlek
            //Om injektionshastigheten inte är 0 så beräknas vilken PVKfärg som ska användas
            if (patientInstance.JodKontrastHastighet != 0)
            {
                this.labelPVKFärg.Text = patientInstance.PVKFärg;
                switch (patientInstance.PVKFärg)
                {
                    case "Gul":
                        this.labelPVKFärg.ForeColor = Color.Goldenrod;
                        this.labelPVKFärg.BackColor = Color.GhostWhite;
                        break;
                    case "Blå":
                        this.labelPVKFärg.ForeColor = Color.SteelBlue;
                        this.labelPVKFärg.BackColor = Color.GhostWhite;
                        break;
                    case "Rosa":
                        this.labelPVKFärg.ForeColor = Color.DeepPink;
                        this.labelPVKFärg.BackColor = Color.GhostWhite;
                        break;
                    case "Grön":
                        this.labelPVKFärg.ForeColor = Color.Green;
                        this.labelPVKFärg.BackColor = Color.GhostWhite;
                        break;
                    case "Vit":
                        this.labelPVKFärg.ForeColor = Color.White;
                        this.labelPVKFärg.BackColor = Color.Black;
                        break;
                    case "Grå":
                        this.labelPVKFärg.ForeColor = Color.Gray;
                        this.labelPVKFärg.BackColor = Color.Black;
                        break;
                    case "Orange":
                        this.labelPVKFärg.ForeColor = Color.Orange;
                        this.labelPVKFärg.BackColor = Color.GhostWhite;
                        break;
                    default:
                        this.labelPVKFärg.ForeColor = Color.SteelBlue;
                        this.labelPVKFärg.BackColor = Color.GhostWhite;
                        break;
                }
            }

            else //Annars nollställs allt
            {
                ResetPVKFärg();
            }
            #endregion
        }
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################



        //########################################################################
        //
        //Beräknar Olika Klasser
        //
        //########################################################################
        private void BeräknaPVKFärg()
        {
            bool done = false;
            int selectedID = ((ComboBoxItem)comboBoxPVKMärke.SelectedItem).ID;

            #region PVKStorlek
            if (patientInstance.JodKontrastHastighet !=0)
            {
                PVKmärken.ForEach(delegate(PVK p)
                {
                    //if (p.PVKID == comboBoxPVKMärke.SelectedIndex)
                    if (p.PVKID == selectedID)
                    {
                        List<PVKstorlek> q = p.storlek.ToList<PVKstorlek>();
                        q.ForEach(delegate(PVKstorlek a)
                        {
                            if (!done && a.maxflöde >= patientInstance.JodKontrastHastighet)
                            {
                                patientInstance.PVKFärg = a.färg;
                                done = true;

                                //Console.WriteLine(String.Format("{0} {1} {2} {3} {4}", a.storleksID, a.färg, a.g, a.ytterdiameter, a.maxflöde));
                            }
                        });
                    }

                });
            }
            else
            {
                patientInstance.PVKFärg = String.Empty;
            }
            if (!done)
            {
                patientInstance.PVKFärg = "Trycket för är högt";
            }
            #endregion
        }
        //########################################################################
        //########################################################################
        //########################################################################
        //########################################################################
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
    }
}

class YourTabControl : TabControl
{
    protected override void WndProc(ref Message m)
    {
        // Hide tabs by trapping the TCM_ADJUSTRECT message
        if (m.Msg == 0x1328 && !DesignMode)
            m.Result = (IntPtr)1;
        else
            base.WndProc(ref m);
    }
}