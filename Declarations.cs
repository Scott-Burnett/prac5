// Do learn to insert your names and a brief description of what the program is supposed to do!

// This is a skeleton program for developing a parser for Modula-2 declarations
// P.D. Terry, Rhodes University Addaped by Redy van DYk && Scott Burnett
using Library;
using System;
using System.Text;

class Token
{
    public int kind;
    public string val;

    public Token(int kind, string val)
    {
        this.kind = kind;
        this.val = val;
    }

} // Token

class Declarations
{

    // +++++++++++++++++++++++++ File Handling and Error handlers ++++++++++++++++++++

    static InFile input;
    static OutFile output;
    static int errorCnt;

    static string NewFileName(string oldFileName, string ext)
    {
        // Creates new file name by changing extension of oldFileName to ext
        int i = oldFileName.LastIndexOf('.');
        if (i < 0) return oldFileName + ext; else return oldFileName.Substring(0, i) + ext;
    } // NewFileName

    static void ReportError(string errorMessage)
    {
        // Displays errorMessage on standard output and on reflected output
        Console.WriteLine(errorMessage);
        output.WriteLine(errorMessage);
        errorCnt++;
    } // ReportError

    static void ReportError(string errorMessage, string token)      //eddited.
    {
        // Displays errorMessage on standard output and on reflected output
        Console.WriteLine(token + "<---- " + errorMessage);
        output.WriteLine(token + "<---- " + errorMessage);
        errorCnt++;
    } // ReportError

    static void Abort(string errorMessage)
    {
        // reports error and exits
        ReportError(errorMessage);
        output.Close();
        System.Environment.Exit(1);

    } // Abort

    static void Abort(string errorMessage, string token)           //Edited
    {
        // reports error and exits
        ReportError(errorMessage, token);
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
      CommaSym = 18,
      LBracketSym = 19,
      RBracketSym = 20,
      LParenSym = 21,
      RParenSym = 22;

    // and others like this
 
    // +++++++++++++++++++++++++++++ Character Handler ++++++++++++++++++++++++++

    const char EOF = '\0';
    static bool atEndOfFile = false;

    // Declaring ch as a global variable is done for expediency - global variables
    // are not always a good thing

    static char ch;    // look ahead character for scanner

    static void GetChar()
    {
        // Obtains next character ch from input, or CHR(0) if EOF reached
        // Reflect ch to output
        if (atEndOfFile) ch = EOF;
        else
        {
            ch = input.ReadChar();
            atEndOfFile = ch == EOF;
            if (!atEndOfFile) output.Write(ch);
        }
    } // GetChar

    // +++++++++++++++++++++++++++++++ Scanner ++++++++++++++++++++++++++++++++++

    // Declaring sym as a global variable is done for expediency - global variables
    // are not always a good thing

    static Token sym;

    static void GetSym()
    {
        // Scans for next sym from input
        while (ch > EOF && ch <= ' ') GetChar();

        StringBuilder symLex = new StringBuilder();
        int symKind = noSym;

        switch (ch)
        {
            case EOF:
                symKind = EOFSym;
                symLex.Append(ch);
                break;
            case 'T':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'O':
                        symLex.Append(ch);
                        GetChar();
                        symKind = ToSym;
                        break;
                    case 'Y':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'P':
                                symLex.Append(ch);
                                GetChar();
                                switch (ch)
                                {
                                    case 'E':
                                        symLex.Append(ch);
                                        GetChar();
                                        symKind = TypeSym; // make the Sym
                                        break;
                                    default:
                                        while (char.IsLetter(ch))
                                        {
                                            symLex.Append(ch);
                                            GetChar();
                                        }
                                        symKind = IdentSym;
                                        break;
                                }
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'V':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'A':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'R':
                                symLex.Append(ch);
                                GetChar();
                                symKind = VarSym;
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'S':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'E':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'T':
                                symLex.Append(ch);
                                GetChar();
                                symKind = SetSym;
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'O':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'F':
                        symLex.Append(ch);
                        GetChar();
                        symKind = OfSym;
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'A':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'R':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'R':
                                symLex.Append(ch);
                                GetChar();
                                switch (ch)
                                {
                                    case 'A':
                                        symLex.Append(ch);
                                        GetChar();
                                        switch (ch)
                                        {
                                            case 'Y':
                                                symLex.Append(ch);
                                                GetChar();
                                                symKind = ArraySym;
                                                break;
                                            default:
                                                while (char.IsLetter(ch))
                                                {
                                                    symLex.Append(ch);
                                                    GetChar();
                                                }
                                                symKind = IdentSym;
                                                break;
                                        }
                                        break;
                                    default:
                                        while (char.IsLetter(ch))
                                        {
                                            symLex.Append(ch);
                                            GetChar();
                                        }
                                        symKind = IdentSym;
                                        break;
                                }
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'R':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'E':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'C':
                                symLex.Append(ch);
                                GetChar();
                                switch (ch)
                                {
                                    case 'O':
                                        symLex.Append(ch);
                                        GetChar();
                                        switch (ch)
                                        {
                                            case 'R':
                                                symLex.Append(ch);
                                                GetChar();
                                                switch (ch)
                                                {
                                                    case 'D':
                                                        symLex.Append(ch);
                                                        GetChar();
                                                        symKind = RecordSym;
                                                        break;
                                                    default:
                                                        while (char.IsLetter(ch))
                                                        {
                                                            symLex.Append(ch);
                                                            GetChar();
                                                        }
                                                        symKind = IdentSym;
                                                        break;
                                                }
                                                break;
                                            default:
                                                while (char.IsLetter(ch))
                                                {
                                                    symLex.Append(ch);
                                                    GetChar();
                                                }
                                                symKind = IdentSym;
                                                break;
                                        }
                                        break;
                                    default:
                                        while (char.IsLetter(ch))
                                        {
                                            symLex.Append(ch);
                                            GetChar();
                                        }
                                        symKind = IdentSym;
                                        break;
                                }
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'P':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'O':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'I':
                                symLex.Append(ch);
                                GetChar();
                                switch (ch)
                                {
                                    case 'N':
                                        symLex.Append(ch);
                                        GetChar();
                                        switch (ch)
                                        {
                                            case 'T':
                                                symLex.Append(ch);
                                                GetChar();
                                                switch (ch)
                                                {
                                                    case 'E':
                                                        symLex.Append(ch);
                                                        GetChar();
                                                        switch (ch)
                                                        {
                                                            case 'R':
                                                                symLex.Append(ch);
                                                                GetChar();
                                                                symKind = PointerSym;
                                                                break;
                                                            default:
                                                                while (char.IsLetter(ch))
                                                                {
                                                                    symLex.Append(ch);
                                                                    GetChar();
                                                                }
                                                                symKind = IdentSym;
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        while (char.IsLetter(ch))
                                                        {
                                                            symLex.Append(ch);
                                                            GetChar();
                                                        }
                                                        symKind = IdentSym;
                                                        break;
                                                }
                                                break;
                                            default:
                                                while (char.IsLetter(ch))
                                                {
                                                    symLex.Append(ch);
                                                    GetChar();
                                                }
                                                symKind = IdentSym;
                                                break;
                                        }
                                        break;
                                    default:
                                        while (char.IsLetter(ch))
                                        {
                                            symLex.Append(ch);
                                            GetChar();
                                        }
                                        symKind = IdentSym;
                                        break;
                                }
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case 'E':
                symLex.Append(ch);
                GetChar();
                switch (ch)
                {
                    case 'N':
                        symLex.Append(ch);
                        GetChar();
                        switch (ch)
                        {
                            case 'D':
                                symLex.Append(ch);
                                GetChar();
                                symKind = EndSym;
                                break;
                            default:
                                while (char.IsLetter(ch))
                                {
                                    symLex.Append(ch);
                                    GetChar();
                                }
                                symKind = IdentSym;
                                break;
                        }
                        break;
                    default:
                        while (char.IsLetter(ch))
                        {
                            symLex.Append(ch);
                            GetChar();
                        }
                        symKind = IdentSym;
                        break;
                }
                break;
            case ';':
                symLex.Append(ch);
                GetChar();
                symKind = SemiSym;
                break;
            case ':':
                symLex.Append(ch);
                GetChar();
                symKind = ColonSym;
                break;
            case '=':
                symLex.Append(ch);
                GetChar();
                symKind = AssignSym;
                break;
            case '.':
                symLex.Append(ch);
                symKind = PeriodSym;
                GetChar();
                switch (ch)
                {
                    case '.':
                        symKind = RangeSym;
                        symLex.Append(ch);
                        GetChar();
                        break;
                    default:
                        // DO NOTHING!!
                        break;
                }
                break;
            case ',':
                symLex.Append(ch);
                GetChar();
                symKind = CommaSym;
                break;
            case '[':
                symLex.Append(ch);
                GetChar();
                symKind = LBracketSym;
                break;
            case ']':
                symLex.Append(ch);
                GetChar();
                symKind = RBracketSym;
                break;
            case '(':
                GetChar();
                symKind = LParenSym;
                if (ch == '*') {
                    symKind = -1;
                    while (true) {
                        GetChar();
                        if (ch == '*') {
                            GetChar();
                            if (ch == ')') {
                                GetChar();
                                break;
                            }
                        }
                    }
                }
                else
                    symLex.Append('(');
                break;
            case ')':
                symLex.Append(ch);
                GetChar();
                symKind = RParenSym;
                break;
            default:
                if (char.IsDigit(ch))
                {
                    symLex.Append(ch);
                    symKind = NumSym;
                    GetChar();
                    while (char.IsDigit(ch))
                    {
                        symLex.Append(ch);
                        GetChar();
                    }
                }
                else if (char.IsLetter(ch))
                {
                    symLex.Append(ch);
                    symKind = IdentSym;
                    GetChar();
                    while (char.IsLetterOrDigit(ch))
                    {
                        symLex.Append(ch);
                        GetChar();
                    }
                }
                else
                {
                    symLex.Append(ch);
                    GetChar();
                }
                break;
        }

        if (symKind == -1) GetSym(); //recursive call
        else sym = new Token(symKind, symLex.ToString()); 
    } // GetSym

    // +++++++++++++++++++++++++++++++ Parser +++++++++++++++++++++++++++++++++++

    static void Accept(int wantedSym, string errorMessage)
    {
        // Checks that lookahead token is wantedSym
        if (sym.kind == wantedSym) GetSym(); else Abort(errorMessage, sym.val);
    } // Accept

    static void Accept(IntSet allowedSet, string errorMessage)
    {
        // Checks that lookahead token is in allowedSet
        if (allowedSet.Contains(sym.kind)) GetSym(); else Abort(errorMessage, sym.val);
    } // Accept

    //Non-Terminal First Sets

    IntSet First_Mod2Decl = new IntSet();
    IntSet First_Declaration = new IntSet();
    IntSet First_TypeDecl = new IntSet();
    IntSet First_VarDecl = new IntSet();
    IntSet First_Type = new IntSet();
    IntSet First_SimpleType = new IntSet();
    IntSet First_QualIdent = new IntSet();
    IntSet First_Subrange = new IntSet();
    IntSet First_Constant = new IntSet();
    IntSet First_Enumeration = new IntSet();
    IntSet First_IdentList = new IntSet();
    IntSet First_ArrayType = new IntSet();
    IntSet First_RecordType = new IntSet();
    IntSet First_FieldLists = new IntSet();
    IntSet First_FieldList = new IntSet();
    IntSet First_SetType = new IntSet();
    IntSet First_PointerType = new IntSet();

    //Non-Terminal Follow Sets

    IntSet Follow_Mod2Decl = new IntSet();
    IntSet Follow_Declaration = new IntSet();
    IntSet Follow_TypeDecl = new IntSet();
    IntSet Follow_VarDecl = new IntSet();
    IntSet Follow_Type = new IntSet();
    IntSet Follow_SimpleType = new IntSet();
    IntSet Follow_QualIdent = new IntSet();
    IntSet Follow_Subrange = new IntSet();
    IntSet Follow_Constant = new IntSet();
    IntSet Follow_Enumeration = new IntSet();
    IntSet Follow_IdentList = new IntSet();
    IntSet Follow_ArrayType = new IntSet();
    IntSet Follow_RecordType = new IntSet();
    IntSet Follow_FieldLists = new IntSet();
    IntSet Follow_FieldList = new IntSet();
    IntSet Follow_SetType = new IntSet();
    IntSet Follow_PointerType = new IntSet();

    //Non Terminal Functions

    static void Mod2Decl() {
        while (First_Declaration.Contains(sym.kind)) Declaration();
    }

    static void Declaration() {
        if (First_Type.Contains(sym.kind)) {
            Accept(TypeSym, "TYPE expected");
            while (First_TypeDecl.Contains(sym.kind)) {
                TypeDecl();
                Accept(SemiSym, "; expected.");
            }
        }
        else if (First_Var.Contains(sym.kind)) {
            Accept(VarSym, "VAR expected");
            while (First_VarDecl.Contains(sym.kind)) {
                VarDecl();
                Accept(SemiSym, "; expected.");
            }
        }
        else
            ReportError("Invalid Declaration");
    }

    static void TypeDecl() {
        Accept(IdentSym, "Identifier expected");
        Accept(AssignSym, "= expected");
        if (First_Type.Contains(sym.kind)) Type(); else ReportError("Invalid Type declaration");
    }

    static void VarDecl() {
        if (firs_IdentList.Contains(sym.kind)) IdentList(); else ReportError("IdentList Expected");
        Accept(ColonSym, ": expected");
        if (First_Type.Contains(sym.kind)) Type(); else ReportError("Invalid Type declaration");
    }

    static void Type() {
        if (First_SimpleType.Contains(sym.kind)) SimpleType();
        else if (First_ArrayType.Contains(sym.kind)) ArrayType();
        else if (First_RecordType.Contains(sym.kind)) RecordType();
        else if (First_SetType.Contains(sym.kind)) SetType();
        else if (First_PointerType.Contains(sym.kind)) PointerType();
        else ReportError("Invalid Type");
    }

    static void SimpleType() {
        if (First_QualIdent.Contains(sym.kind))
        {
            QualIdent();
            if (First_Subrange.Contains(sym.kind)) Subrange(); //else do nothing
        }
        else if (First_Enumeration.Contains(sym.kind)) Enumeration();
        else if (First_Subrange.Contains(sym.kind)) Subrange();
        else ReportError("Invalid Simple Type");
    }

    static void QualIdent() {
        Accept(IdentSym, "Identifier expected");
        while (sym.kind == PeriodSym) {
            GetSym();
            Accept(IdentSym, "Identifier expected");
        }
    }

    static void Subrange() {
        Accept(LBracketSym, "[ expected");
        if (First_Constant.Contains(sym.kind)) Constant(); else ReportError("Invalid Constnt");
        Accept(RangeSym, ".. expected");
        if (First_Constant.Contains(sym.kind)) Constant(); else ReportError("Invalid Constnt");
        Accept(RBracketSym, "] expected");
    }

    static void Constant() {
        switch (sym.kind) {
            case NumSym:
            case IdentSym:
                GetChar();
                break;
            default:
                ReportError("Constant expected");
        }
    }

    static void Enumeration() {
        Accept(LParenSym, "( expected");
        if (First_IdenList.Contains(sym.kind)) IdentList(); else ReportError("Invalid Enumeration");
        Accept(RBracketSym, ") expected");
    }

    static void IdentList() {
        Accept(IdentSym, "Identifier expected");
        while (sym.kind == CommaSym) {
            GetSym();
            Accept(IdentSym, "Identifier expected");
        }
    }

    static void ArrayType() {
        Accept(ArraySym, "ARRAY expected");
        if (First_SimpleType.Containt(sym.kind)) SimpleType(); else ReportError("Invalid Array Type");
        while (sym.kind == PeriodSym) {
            GetSym();
            if (First_SimpleType.Containt(sym.kind)) SimpleType(); else ReportError("Invalid Array Type");
        }
        Accept(OfSym, "OF expected");
        if (First_Type.Contains(sym.kind)) Type; else ReportError("Invalid Array Type");
    }

    static void RecordType() {
        Accept(RecordSym, "RECCORD expected");
        if (First_FieldLists.Contains(sym.kind)) FieldLists(); else ReportError("Invalid record Type");
        Accept(EndSym, "END expected");
    }

    static void FieldLists() {
        if (First_FieldList.Contains(sym.kind)) FieldList(); else ReportError("Invalid FieldLists");
        while (sym.kind == SemiSym) {
            GetSym();
            if (First_FieldList.Contains(sym.kind)) FieldList(); else ReportError("Invalid FieldLists");
        }
    }

    static void FieldList() {
        if (sym.kind == IdentSym)
        {
            GetSym();
            Accept(ColonSym, ": expected");
            if (First_Type.Contains(sym.kind)) Type(); else ReportError("Invalid Field List");
        }
        else if (Follow_FieldList.Contains(sym.kind)) ; else ReportError("Invalid Field List");
    }

    static void SetType() {
        Accept(SetSym, "SET expected");
        Accept(OfSym, "OF expected");
        if (First_SimpleType.Contains(sym.kind)) SimpleType(); else ReportError("Invalid Set Type");
    }

    static void PointerType() {
        Accept(PointerSym, "POINTERR expected");
        Accept(ToSym, "TO expected");
        if (First_Type.Contains(sym.kind)) Type(); else ReportError("Invalid Pointer Type");
    }

    // +++++++++++++++++++++ Main driver function +++++++++++++++++++++++++++++++

    public static void Main(string[] args)
    {
        // Open input and output files from command line arguments
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Declarations FileName");
            System.Environment.Exit(1);
        }
        input = new InFile(args[0]);
        output = new OutFile(NewFileName(args[0], ".out"));

        GetChar();                                  // Lookahead character

        //  To test the scanner we can use a loop like the following:
       /* 
         do
         {
             GetSym();                                 // Lookahead symbol
             OutFile.StdOut.Write(sym.kind, 3);
             OutFile.StdOut.WriteLine(" " + sym.val);  // See what we got
         } while (sym.kind != EOFSym);

        /*  After the scanner is debugged we shall substitute this code: 
        input = new InFile(args[0]);
        output = new OutFile(NewFileName(args[0], ".out"));    // reseting it so we can test both at the same time*/

        do
        {
            GetSym();                                   // Lookahead symbol
            if (First_Mod2decl.contains(sym.kind)) Mod2Decl(); else ReportError ("Incorrect starting symbol");                                // Start to parse from the goal symbol
        } while (sym.kind != EOFSym);                                        // if we get back here everything must have been satisfactory

        (errorCnt == 0) ? Console.WriteLine("Parsed correctly") : Console.WriteLine("End of file reached/nError Count: " + errorCnt);
        output.Close();
    } // Main
} // Declarations
