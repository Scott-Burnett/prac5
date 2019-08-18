  // Do learn to insert your names and a brief description of what the program is supposed to do!

  // This is a skeleton program for developing a parser for Modula-2 declarations
  // P.D. Terry, Rhodes University
  using Library;
  using System;
  using System.Text;

  class Token {
    public int kind;
    public string val;

    public Token(int kind, string val) {
      this.kind = kind;
      this.val = val;
    }

  } // Token

  class Declarations {

    // +++++++++++++++++++++++++ File Handling and Error handlers ++++++++++++++++++++

    static InFile input;
    static OutFile output;

    static string NewFileName(string oldFileName, string ext) {
    // Creates new file name by changing extension of oldFileName to ext
      int i = oldFileName.LastIndexOf('.');
      if (i < 0) return oldFileName + ext; else return oldFileName.Substring(0, i) + ext;
    } // NewFileName

    static void ReportError(string errorMessage) {
    // Displays errorMessage on standard output and on reflected output
      Console.WriteLine(errorMessage);
      output.WriteLine(errorMessage);
    } // ReportError

    static void Abort(string errorMessage) {
    // Abandons parsing after issuing error message
      ReportError(errorMessage);
      output.Close();
      System.Environment.Exit(1);
    } // Abort

    // +++++++++++++++++++++++  token kinds enumeration +++++++++++++++++++++++++

    const int
      noSym = 0,
      EOFSym = 1,
      IdentSym = 2,
      NumSym = 3,
      TypeSym = 4,
      VarSym = 5,
      SetSym = 6,
      OfSym = 7,
      ToSym = 8,
      ArraySym = 9,
      RecordSym = 10,
      PointerSym = 11,
      EndSym = 12,
      SemiSym = 13,
      ColonSym = 14,
      AssignSym = 15,
      PeriodSym = 16,
      RangeSym = 17,
      LBracketSym = 18,
      RBracketSym = 19,
      LParenSym = 20,
      RParenSym = 21;

      // and others like this

    // +++++++++++++++++++++++++++++ Character Handler ++++++++++++++++++++++++++

    const char EOF = '\0';
    static bool atEndOfFile = false;

    // Declaring ch as a global variable is done for expediency - global variables
    // are not always a good thing

    static char ch;    // look ahead character for scanner

    static void GetChar() {
    // Obtains next character ch from input, or CHR(0) if EOF reached
    // Reflect ch to output
      if (atEndOfFile) ch = EOF;
      else {
        ch = input.ReadChar();
        atEndOfFile = ch == EOF;
        if (!atEndOfFile) output.Write(ch);
      }
    } // GetChar

    // +++++++++++++++++++++++++++++++ Scanner ++++++++++++++++++++++++++++++++++

    // Declaring sym as a global variable is done for expediency - global variables
    // are not always a good thing

    static Token sym;

    static void GetSym() {
    // Scans for next sym from input
      while (ch > EOF && ch <= ' ') GetChar();

      StringBuilder symLex = new StringBuilder();
      int symKind = noSym;

        if (ch == '(')
        {
            GetChar();
            if (ch == '*')
            {
                while (true)
                {
                    GetChar();
                    if (ch == '*')
                    {
                        GetChar();
                        if (ch == ')')
                        {
                            GetChar();
                            break;
                        }
                    }

                }
            }
            else {
                symLex.Append(ch);
            }
        }

      while (ch > ' ') {
          symLex.Append(ch);
          GetChar();
      }

      string s = symLex.ToString();

      switch (s) {
            case "\0":
                symKind = EOFSym;
                break;
            case "TYPE":
                symKind = TypeSym;
                break;
            case "VAR":
                symKind = VarSym;
                break;
            case "SET":
                symKind = SetSym;
                break;
            case "OF":
                symKind = OfSym;
                break;
            case "TO":
                symKind = ToSym;
                break;
            case "ARRAY":
                symKind = ArraySym;
                break;
            case "RECORD":
                symKind = RecordSym;
                break;
            case "POINTER":
                symKind = PointerSym;
                break;
            case "END":
                symKind = EndSym;
                break;
            case ";":
                symKind = SemiSym;
                break;
            case ":":
                symKind = ColonSym;
                break;
            case "=":
                symKind = AssignSym;
                break;
            case ".":
                symKind = PeriodSym;
                break;
            case "..":
                symKind = RangeSym;
                break;
            case "[":
                symKind = LBracketSym;
                break;
            case "]":
                symKind = RBracketSym;
                break;
            case "(":
                symKind = LParenSym;
                break;
            case ")":
                symKind = RParenSym;
                break;
            default:
                if (char.IsDigit(symLex[0])) {

                    symKind = NumSym;

                    for (int i = 1; i < symLex.Length; i++)
                        if (!char.IsDigit(symLex[i]))
                        {
                            symKind = noSym;
                            break;
                        }
                }
                else if (char.IsLetter(symLex[0])) {

                    symKind = IdentSym;

                    for (int i = 1; i < symLex.Length; i++)
                        if (!char.IsLetterOrDigit(symLex[i]))
                        {
                            symKind = noSym;
                            break;
                        }
                }            
                break;      
      }
 
      sym = new Token(symKind, symLex.ToString());
    } // GetSym

  /*  ++++ Commented out for the moment

    // +++++++++++++++++++++++++++++++ Parser +++++++++++++++++++++++++++++++++++

    static void Accept(int wantedSym, string errorMessage) {
    // Checks that lookahead token is wantedSym
      if (sym.kind == wantedSym) GetSym(); else Abort(errorMessage);
    } // Accept

    static void Accept(IntSet allowedSet, string errorMessage) {
    // Checks that lookahead token is in allowedSet
      if (allowedSet.Contains(sym.kind)) GetSym(); else Abort(errorMessage);
    } // Accept

    static void Mod2Decl() {}

  ++++++ */

    // +++++++++++++++++++++ Main driver function +++++++++++++++++++++++++++++++

    public static void Main(string[] args) {
      // Open input and output files from command line arguments
      if (args.Length == 0) {
        Console.WriteLine("Usage: Declarations FileName");
        System.Environment.Exit(1);
      }
      input = new InFile(args[0]);
      output = new OutFile(NewFileName(args[0], ".out"));

      GetChar();                                  // Lookahead character

  //  To test the scanner we can use a loop like the following:

      do {
        GetSym();                                 // Lookahead symbol
        OutFile.StdOut.Write(sym.kind, 3);
        OutFile.StdOut.WriteLine(" " + sym.val);  // See what we got
      } while (sym.kind != EOFSym);

  /*  After the scanner is debugged we shall substitute this code:

      GetSym();                                   // Lookahead symbol
      Mod2Decl();                                 // Start to parse from the goal symbol
      // if we get back here everything must have been satisfactory
      Console.WriteLine("Parsed correctly");

  */
      output.Close();
    } // Main

  } // Declarations
