using System;

namespace DatoveStruktury
{
    class SpojovySeznam<T> where T : IComparable
    {
        Uzel zacatek;
        class Uzel
        {
            public T data;
            public Uzel dalsi;
        }

        public void VlozNaZacatek(T vstup)
        {
            Uzel novy = new Uzel();
            novy.data = vstup;
            if (zacatek == null)
            {
                zacatek = novy;
            }
            else
            {
                novy.dalsi = zacatek;
                zacatek = novy;
            }
        }

        public void VlozNaKonec(T vstup)
        {
            Uzel novy = new Uzel();
            novy.data = vstup;

            Uzel docasny = zacatek;
            
            if (zacatek == null)
            {
                zacatek = novy;
                return;
            }
            while (docasny.dalsi != null)
            {
                docasny = docasny.dalsi;
            }

            docasny.dalsi = novy;
            novy.dalsi = null;
        }

        public void VypisPrvky()
        {
            Uzel novy = new Uzel();
            novy = zacatek;
            if (zacatek != null)
            {
                if (zacatek.dalsi == null)
                {
                    Console.WriteLine(zacatek.data);
                }
                else
                {
                    while (novy.dalsi != null)
                    {
                        Console.WriteLine(novy.data);
                        novy = novy.dalsi;
                    }
                    Console.WriteLine(novy.data);
                }
            }
        }

        public void OdebraniPodleHodnoty(T hodnota)
        {
            Uzel uzel = zacatek;
            Uzel predchozi = new Uzel();
            while (hodnota.CompareTo(uzel.data) != 0)
            {
                if (uzel.dalsi != null)
                {
                    predchozi = uzel;
                    uzel = uzel.dalsi;
                }
                else
                {
                    throw new Exception();
                }
            }
            predchozi.dalsi = uzel.dalsi;
        }

        public void OdebraniPodleIndexu(int index)
        {
            if (index == 0)
            {
                zacatek = zacatek.dalsi;
            }
            else
            {
                Uzel pointer = zacatek;
                int counter = 1;

                while (pointer.dalsi != null)
                {
                    if (counter == index)
                    {
                        pointer.dalsi = pointer.dalsi.dalsi;
                        break;
                    }

                    counter += 1;
                    pointer = pointer.dalsi;
                }
            }
        }

        public void VlozNaIndex(int index, T vstup)
        {
            if (index == 0)
            {
                VlozNaZacatek(vstup);
            }
            else
            {
                Uzel novy = new Uzel();
                novy.data = vstup;

                Uzel pointer = zacatek;
                int counter = 1;

                while (pointer.dalsi != null)
                {
                    if (counter == index)
                    {
                        novy.dalsi = pointer.dalsi;
                        pointer.dalsi = novy;
                        break;
                    }

                    counter += 1;
                    pointer = pointer.dalsi;
                }
            }
        }

        public void VlozSerazene(T vstup)
        {
            Uzel novy = new Uzel();
            novy.data = vstup;
            if (zacatek == null || vstup.CompareTo(zacatek.data) < 0)
            {
                novy.dalsi = zacatek;
                zacatek = novy;
            }
            else
            {
                Uzel temp = zacatek;
                while (temp.dalsi != null && vstup.CompareTo(temp.dalsi.data) > 0)
                {
                    temp = temp.dalsi;
                }
                novy.dalsi = temp.dalsi;
                temp.dalsi = novy;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SpojovySeznam<int> seznam = new SpojovySeznam<int>();
            //seznam.VlozNaZacatek(1);
            //seznam.VlozNaZacatek(2);
            //seznam.VlozNaZacatek(3);

            //seznam.VlozNaKonec(1);
            //seznam.VlozNaKonec(2);
            //seznam.VlozNaKonec(3);

            //seznam.VlozNaIndex(2, 4);
            //seznam.VlozNaIndex(4, 5);
            //seznam.VlozNaIndex(6, 6);

            //seznam.OdebraniPodleIndexu(2);
            //seznam.OdebraniPodleHodnoty(4);

            seznam.VlozSerazene(5);
            seznam.VlozSerazene(9);
            seznam.VlozSerazene(1);
            seznam.VlozSerazene(3);
            seznam.VlozSerazene(12);

            seznam.VypisPrvky();
        }
    }
}
