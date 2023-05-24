using System.Net;
using System.Runtime.CompilerServices;

internal class Program
{
    static void Main(string[] args)
    {
        // initialize variable
        string menoHraca1;
        string menoHraca2;
        int velkostPlochy = 1;

        Console.WriteLine("Vitaj v hre piskvorky!");
        menoHraca1 = zadajString("Zadajte meno hraca 1: ");
        menoHraca2 = zadajString("Zadajte meno hraca 2: ");
        velkostPlochy = zadajCislo("Zadajte velkost hracej plochy: ");

        // create array of size of velkostPlochy
        var plocha = new char[velkostPlochy, velkostPlochy];
        // fill game board with empty spaces
        for (int i=0; i < velkostPlochy; i++)
            for (int j=0; j < velkostPlochy; j++)
                plocha[i, j] = ' ';
        // create array of size of velkostPlochy for assessing moves
        var plochaHodnotenia = new int[velkostPlochy, velkostPlochy];

        // create two players, usable by functions
        Hrac hrac1 = new Hrac(menoHraca1, 'X'); 
        Hrac hrac2 = new Hrac(menoHraca2, 'O');

        // create game
        int stavHry = 0; // game condition

        // initialize coordinates
        int riadok = 0;
        int stlpec = 0;

        // initialize counters
        int pocetZnakov;
        int pocetCelkovy;
        bool kontrola;

        while (stavHry == 0)
        {
            ZobrazPlochu(plocha);
            Console.WriteLine("Na tahu je hrac {0}", hrac1.menoHraca);
            // fill assessment with 0
            for (int i = 0; i < velkostPlochy; i++)
                for (int j = 0; j < velkostPlochy; j++)
                    plochaHodnotenia[i, j] = 0;

            // ai to play the move
            if (hrac1.menoHraca == "pc")
                aiHraje();
            else
            {
                // enter coordinates until correctly entered
                bool spravneZadal = false;
                do
                {
                    Console.WriteLine("Zadajte suradnice - riadok: ");
                    do
                    {
                        riadok = zadajCislo("Zadaj cislo medzi 1 a " + velkostPlochy.ToString() + " = ") - 1;
                    } while ((riadok < 0) || (riadok >= velkostPlochy));

                    Console.WriteLine("Zadajte suradnice - stlpec: ");
                    do
                    {
                        stlpec = zadajCislo("Zadaj cislo medzi 1 a " + velkostPlochy.ToString() + " = ") - 1;
                    } while ((stlpec < 0) || (stlpec >= velkostPlochy));

                    // check if the field is empty
                    if (plocha[riadok, stlpec] == ' ')
                    {
                        plocha[riadok, stlpec] = hrac1.znak;
                        spravneZadal = true;
                    }
                    else
                    {
                        Console.WriteLine("Pole je obsadene!");
                    }
                } while (!spravneZadal);
            }

            // check for winning condition
            // check horizontal
            skontroluj(riadok, stlpec, 0, 1, hrac1.znak);
            // check if horizontal winning condition is met
            if (pocetCelkovy >= 5)
                winner();
            // check vertical
            skontroluj(riadok, stlpec, 1, 0, hrac1.znak);
            // check if vertical winning condition is set
            if (pocetCelkovy >= 5)
                winner();
            // check diagonal - left top to right bottom
            skontroluj(riadok, stlpec, 1, 1, hrac1.znak);
            // check for 1st diagonal winning condition
            if (pocetCelkovy >= 5)
                winner();
            // check diagonal - left bottom to right top
            skontroluj(riadok, stlpec, 1, -1, hrac1.znak);
            // check 2nd diagonal winning condition
            if (pocetCelkovy >= 5)
                winner();

            // if no winning condition, change player
            if (stavHry == 0)
            {
                Hrac temp = hrac1;
                hrac1 = hrac2;
                hrac2 = temp;
            }

            /*
            Console.WriteLine("Kontrolny vypocet:");
            Console.Write("Horizontalne = ");
            skontroluj(0, 1, hrac1.znak);
            Console.WriteLine(pocetCelkovy);
            Console.Write("Vertikalne = ");
            skontroluj(1, 0, hrac1.znak);
            Console.WriteLine(pocetCelkovy);
            Console.Write("Diagonalne 1 = ");
            skontroluj(1, 1, hrac1.znak);
            Console.WriteLine(pocetCelkovy);
            Console.Write("Diagonalne 2 = ");
            skontroluj(1, -1, hrac1.znak);
            Console.WriteLine(pocetCelkovy);
            */
        }

        // function to check winning condition (well, rather count of player symbols in a row)
        void skontroluj(int akyRiadok, int akyStlpec, int ciRiadok, int ciStlpec, char ciZnak)
        {
            // check diagonal - left top to right bottom
            pocetCelkovy = 1;
            // one direction
            pocetZnakov = 1;
            kontrola = true;
            while (kontrola)
            {
                if (((akyRiadok - pocetZnakov * ciRiadok) >= 0) && ((akyStlpec - pocetZnakov * ciStlpec) >= 0) && ((akyRiadok - pocetZnakov * ciRiadok) < velkostPlochy) && ((akyStlpec - pocetZnakov * ciStlpec) < velkostPlochy))
                {
                    if (plocha[akyRiadok - pocetZnakov * ciRiadok, akyStlpec - pocetZnakov * ciStlpec] == ciZnak)
                    {
                        pocetZnakov++;
                        pocetCelkovy++;
                    }
                    else
                        kontrola = false;
                }
                else
                    kontrola = false;
            }

            // opposite direction
            pocetZnakov = 1;
            kontrola = true;
            while (kontrola)
            {
                if (((akyRiadok + pocetZnakov * ciRiadok) < velkostPlochy) && ((akyStlpec + pocetZnakov * ciStlpec) < velkostPlochy) && ((akyRiadok + pocetZnakov * ciRiadok) >= 0) && ((akyStlpec + pocetZnakov * ciStlpec) >= 0))
                {
                    if (plocha[akyRiadok + pocetZnakov * ciRiadok, akyStlpec + pocetZnakov * ciStlpec] == ciZnak)
                    {
                        pocetZnakov++;
                        pocetCelkovy++;
                    }
                    else
                        kontrola = false;
                }
                else
                    kontrola = false;
            }

        }

        // check for potential winning condition - so also count empty cells
        void potencial(int akyRiadok, int akyStlpec, int ciRiadok, int ciStlpec, char ciZnak)
        {
            // check diagonal - left top to right bottom
            pocetCelkovy = 1;
            char potencialnyZnak;
            // one direction
            pocetZnakov = 1;
            kontrola = true;
            while (kontrola)
            {
                if (((akyRiadok - pocetZnakov * ciRiadok) >= 0) && ((akyStlpec - pocetZnakov * ciStlpec) >= 0) && ((akyRiadok - pocetZnakov * ciRiadok) < velkostPlochy) && ((akyStlpec - pocetZnakov * ciStlpec) < velkostPlochy))
                {
                    potencialnyZnak = plocha[akyRiadok - pocetZnakov * ciRiadok, akyStlpec - pocetZnakov * ciStlpec];
                    if ((potencialnyZnak == ciZnak) || (potencialnyZnak == ' '))
                    {
                        pocetZnakov++;
                        pocetCelkovy++;
                    }
                    else
                        kontrola = false;
                }
                else
                    kontrola = false;
            }

            // opposite direction
            pocetZnakov = 1;
            kontrola = true;
            while (kontrola)
            {
                if (((akyRiadok + pocetZnakov * ciRiadok) < velkostPlochy) && ((akyStlpec + pocetZnakov * ciStlpec) < velkostPlochy) && ((akyRiadok + pocetZnakov * ciRiadok) >= 0) && ((akyStlpec + pocetZnakov * ciStlpec) >= 0))
                {
                    potencialnyZnak = plocha[akyRiadok + pocetZnakov * ciRiadok, akyStlpec + pocetZnakov * ciStlpec];
                    if ((potencialnyZnak == ciZnak) || (potencialnyZnak == ' '))
                    {
                        pocetZnakov++;
                        pocetCelkovy++;
                    }
                    else
                        kontrola = false;
                }
                else
                    kontrola = false;
            }

        }

        void aiHraje()
        {
            // Assessment priorities + improve assignment of assessment to avoid weaker move to override stronger move
            // avoid moves which cannot lead to winning position when there is a board border or opponent's stone
            // avoid moves which cannot lead to winning position as above, and include empty fields
            // optimize the code - evaluate conditions within the loop

            // 1. check if there is a winning move
            // for every field on the board check if there is a winning move
            // if there is, make the move
            // if there is not, continue with the next step
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac1.znak);
                        /*
                        switch (pocetCelkovy)
                        {
                            case 4:
                                plochaHodnotenia[i, j] += 400;
                                break;
                            case 3:
                                plochaHodnotenia[i, j] += 16;
                                break;
                            case 2:
                                plochaHodnotenia[i, j] += 2;
                                break;
                            case 1:
                                plochaHodnotenia[i, j] += 1;
                                break;
                            default:
                                plochaHodnotenia[i, j] += 10000;
                                break;
                        }
                        */
                        /*
                        skontroluj(i, j, 0, 1, hrac2.znak);
                        switch (pocetCelkovy)
                        {
                            case 4:
                                plochaHodnotenia[i, j] += 80;
                                break;
                            case 3:
                                plochaHodnotenia[i, j] += 16;
                                break;
                            case 2:
                                plochaHodnotenia[i, j] += 2;
                                break;
                            case 1:
                                plochaHodnotenia[i, j] += 1;
                                break;
                            default:
                                plochaHodnotenia[i, j] += 2000;
                                break;
                        }
                         
                        */

                        // check if horizontal winning condition is set
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 10000;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac1.znak);
                        // check if vertical winning condition is set
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 10000;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac1.znak);
                        // check if 1st diagonal winning condition is set
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 10000;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac1.znak);
                        // check if 2nd diagonal winning condition is set
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 10000;
                        }
                    }

                }
            }
            // 2. check if there is a blocking move
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac2.znak);
                        // block the 2nd player if win threat is there
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 2200;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac2.znak);
                        // block the 2nd player if win threat is there
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 2200;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac2.znak);
                        // block the 2nd player if win threat is there
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 2200;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac2.znak);
                        // block the 2nd player if win threat is there
                        if (pocetCelkovy >= 5)
                        {
                            plochaHodnotenia[i, j] += 2200;
                        }
                    }
                }
            }
            // 3. check if there is a move that creates a fork (two winning ways), also winning - 1 (set of 4)
            for (int i = 0;i < velkostPlochy;i++)
            {
                for (int j = 0; j < velkostPlochy;j++)
                {
                    if (plocha[i,j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac1.znak);
                        // check for a fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 440;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac1.znak);
                        // check for a fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 440;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac1.znak);
                        // check for a fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 440;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac1.znak);
                        // check for a fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 440;
                        }
                    }
                }
            }

            // 4. check if there is a move that blocks a fork (two winning ways), also blocking - 1 (set of 4)
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac2.znak);
                        // check for 2nd player fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 88;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac2.znak);
                        // check for 2nd player fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 88;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac2.znak);
                        // check for 2nd player fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 88;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac2.znak);
                        // check for 2nd player fork
                        if (pocetCelkovy >= 4)
                        {
                            plochaHodnotenia[i, j] += 88;
                        }
                    }
                }
            }

            // 5. check if there is a move that creates winning - 2 (set of 3)
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac1.znak);
                        // check for a winning - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac1.znak);
                        // check for a winning - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac1.znak);
                        // check for a winning - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac1.znak);
                        // check for a winning - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                    }
                }
            }

            // 6. check if there is a move that blocks a winning - 2 (set of 3)
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac2.znak);
                        // check for a blocking - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac2.znak);
                        // check for a blocking - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac2.znak);
                        // check for a blocking - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac2.znak);
                        // check for a blocking - 2
                        if (pocetCelkovy >= 3)
                        {
                            plochaHodnotenia[i, j] += 18;
                        }
                    }
                }
            }

            // 7. check if there is a move that creates winning - 3 (set of 2)
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac1.znak);
                        // check for a winning - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 3;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac1.znak);
                        // check for a winning - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 3;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac1.znak);
                        // check for a winning - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 3;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac1.znak);
                        // check for a winning - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 3;
                        }
                    }
                }
            }

            // 8. check if there is a move that blocks a winning - 3 (set of 2)
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        skontroluj(i, j, 0, 1, hrac2.znak);
                        // check for a blocking - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 2;
                        }
                        // check vertical
                        skontroluj(i, j, 1, 0, hrac2.znak);
                        // check for a blocking - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 2;
                        }
                        // check 1st diagonal
                        skontroluj(i, j, 1, 1, hrac2.znak);
                        // check for a blocking - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 2;
                        }
                        // check 2nd diagonal
                        skontroluj(i, j, 1, -1, hrac2.znak);
                        // check for a blocking - 3
                        if (pocetCelkovy >= 2)
                        {
                            plochaHodnotenia[i, j] += 2;
                        }
                    }
                }
            }

            // penalize moves which cannot create winning condition
            //   logic = consider also empty cells
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        potencial(i, j, 0, 1, hrac1.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 300;
                        }
                        // check vertical
                        potencial(i, j, 1, 0, hrac1.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 300;
                        }
                        // check 1st diagonal
                        potencial(i, j, 1, 1, hrac1.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 300;
                        }
                        // check 2nd diagonal
                        potencial(i, j, 1, -1, hrac1.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 300;
                        }
                    }
                }
            }

            // penalize moves which cannot block winning condition
            //   logic = consider also empty cells
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plocha[i, j] == ' ')
                    {
                        // check horizontal
                        potencial(i, j, 0, 1, hrac2.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 30;
                        }
                        // check vertical
                        potencial(i, j, 1, 0, hrac2.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 30;
                        }
                        // check 1st diagonal
                        potencial(i, j, 1, 1, hrac2.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 30;
                        }
                        // check 2nd diagonal
                        potencial(i, j, 1, -1, hrac2.znak);
                        // lower evaluation if no winning opportunity
                        if (pocetCelkovy < 5)
                        {
                            plochaHodnotenia[i, j] -= 30;
                        }
                    }
                }
            }

            // how to optimize the evaluation function?
            //   - create matrix for each cell that will contain the number of potential active player play combinations that the cell is part of
            //   - create matrix for each cell that will contain the number of blocking combinations that the cell is part of
            //   - optimize the evaluation function to use these matrices
            // TODO

            // check for the best move
            int maxHodnotenie = 0;
            for (int i = 0; i < velkostPlochy; i++)
            {
                for (int j = 0; j < velkostPlochy; j++)
                {
                    if (plochaHodnotenia[i, j] > maxHodnotenie)
                    {
                        maxHodnotenie = plochaHodnotenia[i, j];
                        riadok = i;
                        stlpec = j;
                    }
                }
            }

            // 9. play random move - potentially this can be improved by playing the move that is closest to the center of the board
            if (maxHodnotenie == 0)
            {
                Random nahodneCislo = new Random();
                riadok = nahodneCislo.Next(0, velkostPlochy);
                stlpec = nahodneCislo.Next(0, velkostPlochy);
                while (plocha[riadok, stlpec] != ' ')
                {
                    riadok = nahodneCislo.Next(0, velkostPlochy);
                    stlpec = nahodneCislo.Next(0, velkostPlochy);
                }
            }

            // play the move
            plocha[riadok, stlpec] = hrac1.znak;
            Console.WriteLine("Pocitac zahral na poziciu {0}, {1}.", riadok + 1, stlpec + 1);

        }

        // string entry
        string zadajString(string vstupnyText)
        {
            bool kontrolaVstupu = true;
            string vstup = "";
            while (kontrolaVstupu)
            {
                Console.Write(vstupnyText);
                vstup = Console.ReadLine();
                if (vstup == "")
                {
                    Console.WriteLine("Zadajte prosím neprázdny reťazec znakov.");
                }
                else
                {
                    // check if all characters are letters
                    for (int i = 0; i < vstup.Length; i++)
                    {
                        if (!char.IsLetter(vstup[i]))
                        {
                            Console.WriteLine("Zadajte prosím reťazec znakov obsahujúci iba písmená.");
                        }
                        else
                        {
                            kontrolaVstupu = false;
                        }
                    }
                }
            }
            return vstup;
        }

        // digit entry
        int zadajCislo(string vstupnyText)
        {
            bool kontrolaVstupu = true;
            int vstup = 0;
            while (kontrolaVstupu)
            {
                Console.Write(vstupnyText);
                try
                {
                    vstup = int.Parse(Console.ReadLine());
                    kontrolaVstupu = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Zadajte prosím celé číslo.");
                }
            }
            return vstup;
        }

        // winner announcement
        void winner()
        {
             ZobrazPlochu(plocha);
             Console.WriteLine("Hrac {0} vyhral!", hrac1.menoHraca);
             stavHry = 1;
        }
    }

    static void ZobrazPlochu(char[,] plocha)
    {
        // show column numbers
        Console.Write("   "); // leading space for row numbers
        for (int i = 0; i < plocha.GetLength(1); i++)
        {
            Console.Write(i + 1);
            Console.Write(" ");
            if (i < 9)
            {
                Console.Write(" ");
            }
        }
        Console.WriteLine();
        // show rows
        for (int i = 0; i < plocha.GetLength(0); i++)
        {
            Console.Write(i + 1);
            Console.Write(" ");
            if (i < 9)
            {
                Console.Write(" ");
            }
            for (int j = 0; j < plocha.GetLength(1); j++)
            {
                Console.Write(plocha[i, j] + "  ");
            }
            Console.WriteLine();
        }
    }

    public class Hrac
    {
        public string menoHraca;
        public char znak;

        public Hrac(string meno, char v)
        {
            this.menoHraca = meno;
            this.znak = v;
        }
    }

}

