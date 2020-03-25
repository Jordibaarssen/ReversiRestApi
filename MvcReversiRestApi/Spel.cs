using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public enum Kleur { Geen , Wit, Zwart };
    public enum SoortZet { L, R, B, O, LB, LO, RB, RO }

    public class Spel : ISpel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public ICollection<Speler> Spelers { get; set; }
        public Kleur[,] Bord { get; set; }
//        public IEnumerable<Positie> Posities { get; set; }
        public Kleur AandeBeurt { get; set; }
        //public IEnumerable<Positie> VolledigBord
        //{
        //    get
        //    {
        //        List<Positie> posities = new List<Positie>();



        //        return posities;
        //    }
        //}

        

        public SoortZet richting;

        public Spel()
        {
            this.Bord = new Kleur[8, 8];
 
            this.Bord[3, 3] = Kleur.Wit;
            this.Bord[3, 4] = Kleur.Zwart;
            this.Bord[4, 3] = Kleur.Zwart;
            this.Bord[4, 4] = Kleur.Wit;
        }

        public bool Pas()
        {
            for (int i = 0; i <= 7; i++)
            {
                for (int a = 0; a <= 7; a++)
                {
                    if (Bord[i, a] == Kleur.Geen)
                    {
                        if (ZetMogelijk(i, a))
                        {
                            return false;
                        }
                    }
                }
            }
            if(AandeBeurt == Kleur.Zwart)
            {
                AandeBeurt = Kleur.Wit;
            }
            else
            {
                AandeBeurt = Kleur.Zwart;
            }
            return true;
        }

        public bool Afgelopen()
        {
            for(int i = 0; i <= 7; i++)
            {
                for(int a = 0; a <= 7; a++)
                {
                    if (Bord[i, a] == Kleur.Geen) {
                        if (ZetMogelijk(i, a))
                        {
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
        public Kleur OverwegendeKleur()
        {
            int wit = 0;
            int zwart = 0;

            for (int i = 0; i <= 7; i++)
            {
                for (int a = 0; a <= 7; a++)
                {
                    if (Bord[i, a] == Kleur.Wit)
                    {
                        wit++;
                    }else
                    if (Bord[i, a] == Kleur.Zwart)
                    {
                        zwart++;
                    }
                }
            }

            if (zwart > wit)
            {
                return Kleur.Zwart;
            } else if (wit > zwart)
            {
                return Kleur.Wit;
            }
            else
            {
                return Kleur.Geen;
            }
        }
        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            //is de positie op het bord
            if (rijZet > 7 || kolomZet > 7)
            {
                return false;
            }

            bool ZitAndereKleurTussen = false;
            bool EigenKleurGevonden = false;

            int rijMin = rijZet - 1;
            int rijPlus = rijZet + 1;
            int kolomMin = kolomZet - 1;
            int kolomPlus = kolomZet + 1;
            int minCheck = rijZet - 1;
            int plusCheck = rijZet + 1;

            if(rijZet == 7)
            {
                rijPlus = rijZet;
                plusCheck = rijZet;
            }else if(rijZet == 0)
            {
                rijMin = rijZet;
                minCheck = rijZet;
            }

            if(kolomZet == 7)
            {
                kolomPlus = kolomZet;
            }else if(kolomZet == 0){
                kolomMin = kolomZet;
            }

            Kleur Beurt;
            Kleur NietBeurt;

            if (AandeBeurt == Kleur.Zwart)
            {
                Beurt = Kleur.Zwart;
                NietBeurt = Kleur.Wit;
            }
            else
            {
                Beurt = Kleur.Wit;
                NietBeurt = Kleur.Zwart;

            }

            

            //Zit er onder de gekozen positie de "koppelkleur"?
            for (int i = rijPlus; i <= 7; i++)
            {
                if (Bord[i, kolomZet] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[i, kolomZet] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[i, kolomZet] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.O;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }

            //Zit er boven de gekozen positie de "koppelsteen"?
            for (int i = rijMin; i >= 0; i--)
            {
                if (Bord[i, kolomZet] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[i, kolomZet] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[i, kolomZet] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.B;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }

            //Zit er links van de gekozen positie de "koppelsteen"?
            for (int i = kolomMin; i >= 0; i--)
            {
                if (Bord[rijZet, i] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[rijZet, i] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[rijZet, i] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.L;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }

            //Zit er rechts van de gekozen positie de "koppelsteen"?
            
            for (int i = kolomPlus; i <= 7; i++)
            {
                if (Bord[rijZet, i] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[rijZet, i] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[rijZet, i] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.R;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }

            //Zit er rechtsboven van de gekozen positie de "koppelsteen"?
            try { 
            for (int i = kolomPlus; i <= 7; i++)
            {
                
                if (Bord[rijMin, i] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[rijMin, i] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[rijMin, i] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
                rijMin--;
            }
            }
            catch (IndexOutOfRangeException e)
            {

            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.RB;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }
            rijMin = minCheck;

            //Zit er rechsonder van de gekozen positie de "koppelsteen"?
            try { 
            for (int i = kolomPlus; i <= 7; i++)
            {
                
                if (Bord[rijPlus, i] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[rijPlus, i] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[rijPlus, i] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
                rijPlus++;
            }
            }
            catch (IndexOutOfRangeException e)
            {

            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.RO;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }
            rijPlus = plusCheck;

            //Zit er linksboven van de gekozen positie de "koppelsteen"?
            try{
                for (int i = kolomMin; i >= 0; i--)
                {

                    if (Bord[rijMin, i] == NietBeurt)
                    {
                        ZitAndereKleurTussen = true;
                    }
                    else if (Bord[rijMin, i] == Kleur.Geen)
                    {
                        break;
                    }
                    else if (Bord[rijMin, i] == Beurt)
                    {
                        EigenKleurGevonden = true;
                        break;
                    }
                    rijMin--;
                }
            }catch(IndexOutOfRangeException e)
            {

            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.LB;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }
            rijMin = minCheck;

            //Zit er linksonder van de gekozen positie de "koppelsteen"?
            try { 
            for (int i = kolomMin; i >= 0; i--)
            {
                
                if (Bord[rijPlus, i] == NietBeurt)
                {
                    ZitAndereKleurTussen = true;
                }
                else if (Bord[rijPlus, i] == Kleur.Geen)
                {
                    break;
                }
                else if (Bord[rijPlus, i] == Beurt)
                {
                    EigenKleurGevonden = true;
                    break;
                }
                rijPlus++;
            }
            }
            catch (IndexOutOfRangeException e)
            {

            }
            if (ZitAndereKleurTussen && EigenKleurGevonden)
            {
                richting = SoortZet.LO;
                return true;
            }
            else
            {
                ZitAndereKleurTussen = false;
                EigenKleurGevonden = false;
            }
            rijPlus = plusCheck;
            return false;
        }
        public bool DoeZet(int rijZet, int kolomZet)
        {
            if(ZetMogelijk(rijZet, kolomZet))
            {
                
                Kleur NietBeurt;

                if (AandeBeurt == Kleur.Zwart)
                {
                   
                    NietBeurt = Kleur.Wit;
                }
                else
                {
                    NietBeurt = Kleur.Zwart;
                }

                
                Bord[rijZet, kolomZet] = AandeBeurt;

                if(richting == SoortZet.L)
                {
                    for (int i = kolomZet - 1; i >= 0; i--)
                    {
                        if (Bord[rijZet, i] == NietBeurt)
                        {
                            Bord[rijZet, i] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.R)
                {
                    for (int i = kolomZet + 1; i <= 7; i++)
                    {
                        if (Bord[rijZet, i] == NietBeurt)
                        {
                            Bord[rijZet, i] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.B)
                {
                    for (int i = rijZet - 1; i >= 0; i--)
                    {
                        if (Bord[i, kolomZet] == NietBeurt)
                        {
                            Bord[i, kolomZet] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.O)
                {
                    for (int i = rijZet + 1; i <= 7; i++)
                    {
                        if (Bord[i, kolomZet] == NietBeurt)
                        {
                            Bord[i, kolomZet] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.RB)
                {
                    for (int i = kolomZet + 1; i <= 7; i++)
                    {
                        rijZet--;
                         if (Bord[rijZet, i] == NietBeurt)
                        {
                            Bord[rijZet, i] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.RO)
                {
                    for (int i = kolomZet + 1; i <= 7; i++)
                    {
                        rijZet++;
                        if (Bord[rijZet, i] == NietBeurt)
                        {
                            Bord[rijZet, i] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.LB)
                {
                    for (int i = kolomZet - 1; i >= 0; i--)
                    {
                        rijZet--;
                        if (Bord[rijZet, i] == NietBeurt)
                        {
                            Bord[rijZet, i] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (richting == SoortZet.LO)
                {
                    for (int i = kolomZet - 1; i >= 0; i--)
                    {
                        rijZet++;
                        if (Bord[rijZet, i] == NietBeurt)
                        {
                            Bord[rijZet, i] = AandeBeurt;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (AandeBeurt == Kleur.Zwart)
                {
                    AandeBeurt = Kleur.Wit;
                }
                else
                {
                    AandeBeurt = Kleur.Zwart;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
