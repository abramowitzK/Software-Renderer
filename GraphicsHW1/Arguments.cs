using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsHW.Util
{
    class Arguments : Dictionary<string, object>
    {
        string[] m_rawInput;

        public Arguments(string[] args)
        {
            //Setting default values in case the user doesn't input a parameter
            this["-f"] = "hw1.ps";
            this["-s"] = 1.0;
            this["-r"] = 0;
            this["-m"] = 0;
            this["-n"] = 0;
            this["-a"] = 0;
            this["-b"] = 0;
            this["-c"] = 499;
            this["-d"] = 499;
            m_rawInput = args;
            Parse();  
        }
        public string InputFile
        {
            get
            {
                return this["-f"] as string;
            }

        }
        public float Scale
        {
            get
            {
                return (float)this["-s"];
            }
        }
        public int Rotation
        {
            get
            {
                return (int)this["-r"];
            }
        }
        public int XTranslation
        {
            get
            {
                return (int)this["-m"];
            }
        }
        public int YTranslation
        {
            get
            {
                return (int)this["-n"];
            }
        }
        public int XLower
        {
            get
            {
                return (int)this["-a"];
            }
        }
        public int XUpper
        {
            get
            {
                return (int)this["-b"];
            }
        }
        public int YLower
        {
            get
            {
                return (int)this["-c"];
            }
        }
        public int YUpper
        {
            get
            {
                return (int)this["-d"];
            }
        }
        private void Parse()
        {
            for (int i = 0; i < m_rawInput.Length; i+=2)
            {
                switch (m_rawInput[i])
                {
                    case "-f":
                        {
                            this["-f"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-s":
                        {
                            this["-s"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-r":
                        {
                            this["-r"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-m":
                        {
                            this["-m"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-n":
                        {
                            this["-n"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-a":
                        {
                            this["-a"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-b":
                        {
                            this["-b"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-c":
                        {
                            this["-c"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-d":
                        {
                            this["-d"] = m_rawInput[i + 1];
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid argument: " + m_rawInput[i] + ". Skipping...");
                            break;
                        }
                }
            }
        }
    }
}
