using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsHW.Util
{
    /*
    Class used for parsing and storing arguments
    */
    class Arguments : Dictionary<string, object>
    {
        string[] m_rawInput;

        public Arguments(string[] args)
        {
            //Setting default values in case the user doesn't input a parameter
            this["-f"] = "bound-sprellpsd.smf";
            this["-g"] = null;
            this["-i"] = null;
            this["-s"] = 1.0;
            this["-r"] = 0.0;
            this["-m"] = 0;
            this["-n"] = 0;
            this["-a"] = 0;
            this["-b"] = 0;
            this["-c"] = 500;
            this["-d"] = 500;
            this["-j"] = 0.0;
            this["-k"] = 0.0;
            this["-o"] = 500.0;
            this["-p"] = 500.0;
            this["-x"] = 0.0;
            this["-y"] = 0.0;
            this["-z"] = 1.0;
            this["-X"] = 0.0;
            this["-Y"] = 0.0;
            this["-Z"] = 0.0;
            this["-q"] = 0.0;
            this["-r"] = 0.0;
            this["-w"] = -1.0;
            this["-Q"] = 0.0;
            this["-R"] = 1.0;
            this["-W"] = 0.0;
            this["-u"] = -0.7;
            this["-v"] = -0.7;
            this["-U"] = 0.7;
            this["-V"] = 0.7;
            this["-P"] = false;
            this["-F"] = 0.6;
            this["-B"] = -0.6;
            m_rawInput = args;
            Parse();  
        }
        public double VP_XLower
        {
            get
            {
                return Convert.ToDouble(this["-j"]);
            }
        }
        public double VP_YLower
        {
            get
            {
                return Convert.ToDouble(this["-k"]);
            }
        }
        public double VP_XUpper
        {
            get
            {
                return Convert.ToDouble(this["-o"]);
            }
        }
        public double VP_YUpper
        {
            get
            {
                return Convert.ToDouble(this["-p"]);
            }
        }
        public string FirstModel
        {
            get
            {
                return this["-f"] as string;
            }

        }
        public double Scale
        {
            get
            {
                return Convert.ToSingle(this["-s"]);
            }
        }
        //public double Rotation
        //{
        //    get
        //    {
        //        return Convert.ToDouble(this["-r"]);
        //    }
        //}
        public int XTranslation
        {
            get
            {
                return Convert.ToInt32(this["-m"]);
            }
        }
        public int YTranslation
        {
            get
            {
                return Convert.ToInt32(this["-n"]);
            }
        }
        public int XLower
        {
            get
            {
                return Convert.ToInt32(this["-a"]);
            }
        }
        public int XUpper
        {
            get
            {
                return Convert.ToInt32(this["-c"]);
            }
        }
        public int YLower
        {
            get
            {
                return Convert.ToInt32(this["-b"]);
            }
        }
        public int YUpper
        {
            get
            {
                return Convert.ToInt32(this["-d"]);
            }
        }
        public double PRP_X
        {
            get
            {
                return Convert.ToDouble(this["-x"]);
            }
        }
        public double PRP_Y
        {
            get
            {
                return Convert.ToDouble(this["-y"]);
            }
        }
        public double PRP_Z
        {
            get
            {
                return Convert.ToDouble(this["-z"]);
            }
        }
        public double VRP_X
        {
            get
            {
                return Convert.ToDouble(this["-X"]);
            }
        }
        public double VRP_Y
        {
            get
            {
                return Convert.ToDouble(this["-Y"]);
            }
        }
        public double VRP_Z
        {
            get
            {
                return Convert.ToDouble(this["-Z"]);
            }
        }
        public double VPN_X
        {
            get
            {
                return Convert.ToDouble(this["-q"]);
            }
        }
        public double VPN_Y
        {
            get
            {
                return Convert.ToDouble(this["-r"]);
            }
        }
        public double VPN_Z
        {
            get
            {
                return Convert.ToDouble(this["-w"]);
            }
        }
        public double VUP_X
        {
            get
            {
                return Convert.ToDouble(this["-Q"]);
            }
        }
        public double VUP_Y
        {
            get
            {
                return Convert.ToDouble(this["-R"]);
            }
        }
        public double VUP_Z
        {
            get
            {
                return Convert.ToDouble(this["-W"]);
            }
        }
        public double VRC_UMIN
        {
            get
            {
                return Convert.ToDouble(this["-u"]);
            }
        }
        public double VRC_VMIN
        {
            get
            {
                return Convert.ToDouble(this["-v"]);
            }
        }
        public double VRC_UMAX
        {
            get
            {
                return Convert.ToDouble(this["-U"]);
            }
        }
        public double VRC_VMAX
        {
            get
            {
                return Convert.ToDouble(this["-V"]);
            }
        }
        public bool IsParallelProjection
        {
            get
            {
                return Convert.ToBoolean(this["-P"]);
            }
        }
        public double FrontPlaneVRC
        {
            get
            {
                return Convert.ToDouble(this["-F"]);
            }
        }
        public double BackPlaneVRC
        {
            get
            {
                return Convert.ToDouble(this["-B"]);
            }
        }
        public string SecondFile
        {
            get
            {
                return this["-g"] as string;
            }
        }
        public string ThirdFile
        {
            get
            {
                return this["-i"] as string;
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
                    //case "-r":
                    //    {
                    //        this["-r"] = m_rawInput[i + 1];
                    //        break;
                    //    }
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
                    case "-j":
                        {
                            this["-j"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-k":
                        {
                            this["-k"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-o":
                        {
                            this["-o"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-p":
                        {
                            this["-p"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-x":
                        {
                            this["-x"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-y":
                        {
                            this["-y"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-z":
                        {
                            this["-z"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-X":
                        {
                            this["-X"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-Y":
                        {
                            this["-Y"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-Z":
                        {
                            this["-Z"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-q":
                        {
                            this["-q"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-r":
                        {
                            this["-r"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-w":
                        {
                            this["-w"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-Q":
                        {
                            this["-Q"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-R":
                        {
                            this["-R"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-W":
                        {
                            this["-W"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-u":
                        {
                            this["-u"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-v":
                        {
                            this["-v"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-U":
                        {
                            this["-U"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-V":
                        {
                            this["-V"] = m_rawInput[i + 1];
                            break;
                        }
                    case "-P":
                        {
                            this["-P"] = true;
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
