using System;
using System.Collections.Generic;
using System.Text;

namespace MicroChip8Sim
{
    enum InstructionType
    {
        JP,
        JNE,
        JPE,
        LD,
        Add,
        OR,
        AND,
        XOR,
        SUB,
        SLR,
        SLL,
        SETDL,
        GETDL,
        DIS
    }
    struct Lex
    {
        public InstructionType instructionType;
        public List<string> arguments;
        public int lineNumber;
    }
    class Lexer
    {
        public static (List<Lex>, List<string>) lex(string[] input)
        {
            List<Lex> result = new List<Lex>();
            List<string> encounteredErrors = new List<string>();
            for (var i = 0; i < input.Length; i++)
            {
                string line = input[i];
                string[] lineContents = line.Split(new Char[] { ' ', ',' });
                if (lineContents.Length <= 0)
                    continue;

                bool isValidCommand = true;
                Lex thisLex = new Lex();
                thisLex.arguments = new List<string>();

                switch (lineContents[0].ToUpper())
                {
                    case "JP": thisLex.instructionType = InstructionType.JP; break;
                    case "JNE": thisLex.instructionType = InstructionType.JNE; break;
                    case "JPE": thisLex.instructionType = InstructionType.JPE; break;
                    case "LD": thisLex.instructionType = InstructionType.LD; break;
                    case "ADD": thisLex.instructionType = InstructionType.Add; break;
                    case "OR": thisLex.instructionType = InstructionType.OR; break;
                    case "AND": thisLex.instructionType = InstructionType.AND; break;
                    case "XOR": thisLex.instructionType = InstructionType.XOR; break;
                    case "SUB": thisLex.instructionType = InstructionType.SUB; break;
                    case "SLR": thisLex.instructionType = InstructionType.SLR; break;
                    case "SLL": thisLex.instructionType = InstructionType.SLL; break;
                    case "SETDL": thisLex.instructionType = InstructionType.SETDL; break;
                    case "GETDL": thisLex.instructionType = InstructionType.GETDL; break;
                    case "DIS": thisLex.instructionType = InstructionType.DIS; break;
                    default:
                        encounteredErrors.Add($"Line {i.ToString()}: {lineContents[0]} not recognized as a command.");
                        isValidCommand = false;
                        break;
                }

                if (isValidCommand)
                {
                    bool frstArg = true;
                    foreach (string arg in lineContents)
                    {
                        if (frstArg || (arg.Length <= 0))
                        {
                            frstArg = false;
                            continue;
                        }

                        thisLex.arguments.Add(arg.ToUpper());
                    }

                    thisLex.lineNumber = i;

                    result.Add(thisLex);
                }
            }

            return (result, encounteredErrors);
        }
    }
}
