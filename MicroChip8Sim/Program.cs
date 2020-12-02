using System;

namespace MicroChip8Sim
{
    class Program
    {
        static void Main(string[] args)
        {
            string tst = 
                "Add r0, r2\n" +
                "ADD R0, 00FF\n" +
                "DIS\n" +
                "GETDL rF\n" +
                "SETDL R5\n" +
                "SLL R0\n" +
                "SLR R1\n" +
                "SUB R8, R6\n" +
                "XOR R5, R6\n" +
                "AND R7, R8\n" +
                "OR R2, R3\n" +
                "ADD R1, 23\n" +
                "ADD R2, R3\n" +
                "LD R3, 23\n" +
                "LD R5, R4\n" +
                "JPE R6, 56\n" +
                "JPE R6, R2\n" +
                "JNE R6, 56\n" +
                "JNE R6, R2\n" +
                "JP 235";
            var b = tst.Split('\n');
            var a = Lexer.lex(b);
            var q = Assembler.Assemble(a.Item1);
            Console.WriteLine(tst);
        }
    }
}
