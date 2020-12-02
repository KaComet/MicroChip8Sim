using System;
using System.Collections.Generic;
using System.Text;

namespace MicroChip8Sim
{
    public class MC8Sim
    {
        const int MaxProgramSize = 0x1000;
        private char[] Registers;
        private char PC;
        private char[] program;
        private bool[,] display; //[x, y]
        private double DelayTimer;

        public MC8Sim()
        {
            Registers = new char[0x10];
            PC = new char();
            DelayTimer = new double();
            program = new char[MaxProgramSize];
            display = new bool[0x100, 0x100];
        }

        public void setMem(char address, char contents)
        {
            if (address < MaxProgramSize)
            {
                program[address] = contents;
            }
        }

        public bool pixel(int x, int y)
        {
            if ((x > 255) || (x < 0))
                return false;

            if ((y > 255) || (y < 0))
                return false;

            return display[x, y];
        }

        public void reset()
        {
            for (var i = 0; i < 0x10; i++)
                Registers[i] = (char)0;

            for (var i = 0; i < 0x100; i++)
                for (var j = 0; j < 0x100; j++)
                    display[i, j] = false;
        }

        public void tick(double Dt)
        {
            DelayTimer -= Dt;
            if (Dt < 0)
                DelayTimer = 0;

            char IR = program[PC];
            char N1, N2, N3, N4, NNN, NN;
            N1 = (char)((IR >> 12) & 0xF); // MSN
            N2 = (char)((IR >>  8) & 0xF);
            N3 = (char)((IR >>  4) & 0xF);
            N4 = (char)((IR >>  0) & 0xF);
            NNN = (char)(IR & 0xFFF);
            NN = (char)(IR & 0xFF);



            switch (N1, N2, N3, N4)
            {
                case ((char)0x1, _, _, _): gotoNNN(NNN); break;         //I-1
                case ((char)0x3, _, _, _): ifVx_e_NN(N2, NN); break;   //I-2
                case ((char)0x4, _, _, _): ifVx_ne_NN(N2, NN); break;   //I-3
                case ((char)0x5, _, _, (char)0): ifVx_e_Vy(N2, N3); break;   //I-4
                case ((char)0x6, _, _, _): LD_Vx_NN(N2, NN); break;  //I-5
                case ((char)0x7, _, _, _): ADD_Vx_NN(N2, NN); break; //I-6
                case ((char)0x8, _, _, (char)0x0): LD_Vx_Vy(N2, N3); break; //I-7
                case ((char)0x8, _, _, (char)0x1): OR_Vx_Vy(N2, N3); break; //I-8
                case ((char)0x8, _, _, (char)0x2): AND_Vx_Vy(N2, N3); break; //I-9
                case ((char)0x8, _, _, (char)0x3): XOR_Vx_Vy(N2, N3); break; //I-10
                case ((char)0x8, _, _, (char)0x4): ADD_Vx_Vy(N2, N3); break; //I-11
                case ((char)0x8, _, _, (char)0x5): SUB_Vx_Vy(N2, N3); break; //I-12
                case ((char)0x8, _, _, (char)0x6): SLR_Vx(N2); break; //I-13
                case ((char)0x8, _, _, (char)0x7): SUB_Vy_Vx(N2, N3); break; //I-14
                case ((char)0x8, _, _, (char)0xE): SLL_Vx(N2); break; //I-15
                case ((char)0x9, _, _, (char)0): ifVx_ne_Vy(N2, N3); break; //I-16
                case ((char)0xB, _, _, _): LD_PC_V0_NNN(NNN); break; //I-17
                case ((char)0xC, (char)0, (char)0, (char)0): LD_PC_VD_VE(); break; //I-18
                case ((char)0xF, _, (char)0, (char)7): LD_Vx_Delay(N2); break; //I-19
                case ((char)0xF, _, (char)1, (char)5): LD_Delay_Vx(N2); break; //I-20
                case ((char)0xF, (char)0, (char)0, (char)0): DIS(); break; //I-21
                default: NOP(); break;
            }
        }

        private void NOP() //I-0
        {
            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void gotoNNN(char nnn) //I-1
        {
            PC = nnn;
            PC %= (char)MaxProgramSize;
        }

        private void ifVx_e_NN(char VXn, char nn) //I-2
        {
            if (Registers[VXn] == nn)
                PC += (char)2;
            else
                PC += (char)1;

            PC %= (char)MaxProgramSize;
        }

        private void ifVx_ne_NN(char VXn, char nn) //I-3
        {
            if (Registers[VXn] != nn)
                PC += (char)2;
            else
                PC += (char)1;

            PC %= (char)MaxProgramSize;
        }

        private void ifVx_e_Vy(char VXn, char VYn) //I-4
        {
            if (Registers[VXn] == Registers[VYn])
                PC += (char)2;
            else
                PC += (char)1;

            PC %= (char)MaxProgramSize;
        }

        private void LD_Vx_NN(char VXn, char NN) //I-5
        {
            Registers[VXn] = NN;

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void ADD_Vx_NN(char VXn, char NN) //I-6
        {
            Registers[VXn] = (char)((Registers[VXn] + NN) & 0xFF);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void LD_Vx_Vy(char VXn, char VYn) //I-7
        {
            Registers[VXn] = Registers[VYn];

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void OR_Vx_Vy(char VXn, char VYn) //I-8
        {
            Registers[VXn] = (char)(Registers[VYn] | Registers[VXn]);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void AND_Vx_Vy(char VXn, char VYn) //I-9
        {
            Registers[VXn] = (char)(Registers[VYn] & Registers[VXn]);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void XOR_Vx_Vy(char VXn, char VYn) //I-10
        {
            Registers[VXn] = (char)(Registers[VYn] ^ Registers[VXn]);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void ADD_Vx_Vy(char VXn, char VYn) //I-11
        {
            char tmp, masked;
            tmp = (char)(Registers[VXn] + Registers[VYn]);
            masked = (char)(tmp & 0xFF);
            Registers[VXn] = masked;

            if (masked != tmp)
                Registers[0xF] = (char)1;
            else
                Registers[0xF] = (char)0;

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void SUB_Vx_Vy(char VXn, char VYn) //I-12
        {
            char tmp, masked;
            tmp = (char)(Registers[VXn] - Registers[VYn]);
            masked = (char)(tmp & 0xFF);
            Registers[VXn] = masked;

            if (Registers[VXn] < Registers[VYn])
                Registers[0xF] = (char)1;
            else
                Registers[0xF] = (char)0;

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void SLR_Vx(char VXn) //I-13
        {
            Registers[0xF] = (char)(Registers[VXn] & 0x01);
            Registers[VXn] = (char)(Registers[VXn] >> 1);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void SUB_Vy_Vx(char VXn, char VYn) //I-14
        {
            char tmp, masked;
            tmp = (char)(Registers[VYn] - Registers[VXn]);
            masked = (char)(tmp & 0xFF);
            Registers[VXn] = masked;

            if (Registers[VXn] < Registers[VYn])
                Registers[0xF] = (char)1;
            else
                Registers[0xF] = (char)0;

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void SLL_Vx(char VXn) //I-15
        {
            Registers[0xF] = (char)(Registers[VXn] & 0x80);
            Registers[VXn] = (char)(Registers[VXn] << 1);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void ifVx_ne_Vy(char VXn, char VYn) //I-16
        {
            if (Registers[VXn] != Registers[VYn])
                PC += (char)2;
            else
                PC += (char)1;

            PC %= (char)MaxProgramSize;
        }

        private void LD_PC_V0_NNN(char nnn) //I-17
        {
            PC = (char)((Registers[0] + nnn) % MaxProgramSize);
        }

        private void LD_PC_VD_VE() //I-18
        {
            PC = (char)(((Registers[0xD] << 8) + Registers[0xE]) % MaxProgramSize);
        }

        private void LD_Vx_Delay(char Vx) //I-19
        {
            Registers[Vx] = (char)((char)DelayTimer & 0xFF);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void LD_Delay_Vx(char Vx) //I-20
        {
            DelayTimer = (char)(Registers[Vx] & 0xFF);

            PC++;
            PC %= (char)MaxProgramSize;
        }

        private void DIS() //I-21
        {
            if ((Registers[0xF] & 0b1) == 1)
                display[Registers[0xD], Registers[0xE]] = true;
            else
                display[Registers[0xD], Registers[0xE]] = false;

            PC++;
            PC %= (char)MaxProgramSize;
        }
    }
}
