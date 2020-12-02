using System;
using System.Collections.Generic;
using System.Text;

namespace MicroChip8Sim
{
    class Assembler
    {
        public enum ArgType
        {
            reg, literal, bad
        }
        public static (List<char>, List<string>) Assemble(List<Lex> lexerOutput)
        {
            List<char> result = new List<char>();
            List<string> errors = new List<string>();

            for (var i = 0; i < lexerOutput.Count; i++)
            {
                Lex line = lexerOutput[i];
                char ResultOpcode = (char)0x0000;
                int dest = new int();
                int src = new int();
                int lit = new int();
                switch (line.instructionType)
                {
                    case InstructionType.Add:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Destination must be a register.");
                            break;
                        }

                        if ((getArgType(line.arguments[1]) != ArgType.reg) && (getArgType(line.arguments[1]) != ArgType.literal))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: \"line.arguments[1]\" not recognized as a valid register/literal.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        if (getArgType(line.arguments[1]) == ArgType.reg)
                        {
                            src = getReg(line.arguments[1]) & 0xF;
                            ResultOpcode = (char)(0x8004 + (dest << 8) + (src << 4));
                        } else
                        {
                            lit = literalParse(line.arguments[1]) & 0xFF;
                            ResultOpcode = (char)(0x7000 + (dest << 8) + lit);
                        }
                        break;

                    case InstructionType.LD:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Destination must be a register.");
                            break;
                        }

                        if ((getArgType(line.arguments[1]) != ArgType.reg) && (getArgType(line.arguments[1]) != ArgType.literal))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: \"line.arguments[1]\" not recognized as a valid register/literal.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        if (getArgType(line.arguments[1]) == ArgType.reg)
                        {
                            src = getReg(line.arguments[1]) & 0xF;
                            ResultOpcode = (char)(0x8000 + (dest << 8) + (src << 4));
                        }
                        else
                        {
                            lit = literalParse(line.arguments[1]) & 0xFF;
                            ResultOpcode = (char)(0x6000 + (dest << 8) + lit);
                        }
                        break;

                    case InstructionType.JNE:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Destination must be a register.");
                            break;
                        }

                        if ((getArgType(line.arguments[1]) != ArgType.reg) && (getArgType(line.arguments[1]) != ArgType.literal))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: \"line.arguments[1]\" not recognized as a valid register/literal.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        if (getArgType(line.arguments[1]) == ArgType.reg)
                        {
                            src = getReg(line.arguments[1]) & 0xF;
                            ResultOpcode = (char)(0x9000 + (dest << 8) + (src << 4));
                        }
                        else
                        {
                            lit = literalParse(line.arguments[1]) & 0xFF;
                            ResultOpcode = (char)(0x4000 + (dest << 8) + lit);
                        }
                        break;

                    case InstructionType.JPE:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Destination must be a register.");
                            break;
                        }

                        if ((getArgType(line.arguments[1]) != ArgType.reg) && (getArgType(line.arguments[1]) != ArgType.literal))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: \"line.arguments[1]\" not recognized as a valid register/literal.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        if (getArgType(line.arguments[1]) == ArgType.reg)
                        {
                            src = getReg(line.arguments[1]) & 0xF;
                            ResultOpcode = (char)(0x5000 + (dest << 8) + (src << 4));
                        }
                        else
                        {
                            lit = literalParse(line.arguments[1]) & 0xFF;
                            ResultOpcode = (char)(0x3000 + (dest << 8) + lit);
                        }
                        break;

                    case InstructionType.SUB:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Destination must be a register.");
                            break;
                        }

                        if (getArgType(line.arguments[1]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Source must be a register.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        src = getReg(line.arguments[1]) & 0xF;
                        ResultOpcode = (char)(0x8005 + (dest << 8) + (src << 4));
                        break;

                    case InstructionType.AND:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if ((getArgType(line.arguments[0]) != ArgType.reg) || (getArgType(line.arguments[1]) != ArgType.reg))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operands must be registers.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        src = getReg(line.arguments[1]) & 0xF;

                        ResultOpcode = (char)(0x8002 + (dest << 8) + (src << 4));

                        break;

                    case InstructionType.OR:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if ((getArgType(line.arguments[0]) != ArgType.reg) || (getArgType(line.arguments[1]) != ArgType.reg))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operands must be registers.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        src = getReg(line.arguments[1]) & 0xF;

                        ResultOpcode = (char)(0x8001 + (dest << 8) + (src << 4));

                        break;

                    case InstructionType.XOR:
                        if (line.arguments.Count != 2)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if ((getArgType(line.arguments[0]) != ArgType.reg) || (getArgType(line.arguments[1]) != ArgType.reg))
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operands must be registers.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;
                        src = getReg(line.arguments[1]) & 0xF;

                        ResultOpcode = (char)(0x8003 + (dest << 8) + (src << 4));

                        break;

                    case InstructionType.SLL:
                        if (line.arguments.Count != 1)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operand must be a register.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;

                        ResultOpcode = (char)(0x800E + (dest << 8));

                        break;

                    case InstructionType.SLR:
                        if (line.arguments.Count != 1)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operand must be a register.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;

                        ResultOpcode = (char)(0x8006 + (dest << 8));

                        break;

                    case InstructionType.DIS:
                        if (line.arguments.Count > 0)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: No operands are needed for DIS.");
                            break;
                        }

                        ResultOpcode = (char)(0xF000);

                        break;

                    default: break;

                    case InstructionType.SETDL:
                        if (line.arguments.Count != 1)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operand must be a register.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;

                        ResultOpcode = (char)(0xF015 + (dest << 8));

                        break;

                    case InstructionType.GETDL:
                        if (line.arguments.Count != 1)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.reg)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operand must be a register.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xF;

                        ResultOpcode = (char)(0xF007 + (dest << 8));

                        break;

                    case InstructionType.JP:
                        if (line.arguments.Count != 1)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Bad number of operands.");
                            break;
                        }

                        if (getArgType(line.arguments[0]) != ArgType.literal)
                        {
                            ResultOpcode = (char)0x0000;
                            errors.Add($"Line {line.lineNumber}: Operand must be a literal.");
                            break;
                        }

                        dest = getReg(line.arguments[0]) & 0xFFF;

                        ResultOpcode = (char)(0x1000 + dest);

                        break;
                }

                result.Add(ResultOpcode);
            }

            return (result, errors);
        }

        static int literalParse(string str)
        {
            return (char)int.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }

        static ArgType getArgType(string arg) {
            if (arg.Length == 2)
            {
                if (arg[0] == 'R')
                {
                    if (IsHex(arg[1]))
                    {
                        return ArgType.reg;
                    }

                    return ArgType.bad;
                }
            }

            if (StrIsHex(arg))
            {
                return ArgType.literal;
            }

            return ArgType.bad;
        }
        static bool IsHex(char c)
        {
            return (c >= '0' && c <= '9') ||
                     (c >= 'a' && c <= 'f') ||
                     (c >= 'A' && c <= 'F');
        }

        static bool StrIsHex(string str)
        {
            bool isGood;
            foreach (var c in str)
            {
                isGood = IsHex(c);

                if (!isGood)
                    return false;
            }
            return true;
        }

        static int getReg(string str)
        {
            switch (str[1])
            {
                case '0': return 0x0;
                case '1': return 0x1;
                case '2': return 0x2;
                case '3': return 0x3;
                case '4': return 0x4;
                case '5': return 0x5;
                case '6': return 0x6;
                case '7': return 0x7;
                case '8': return 0x8;
                case '9': return 0x9;
                case 'A': return 0xA;
                case 'B': return 0xB;
                case 'C': return 0xC;
                case 'D': return 0xD;
                case 'E': return 0xE;
                case 'F': return 0xF;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
