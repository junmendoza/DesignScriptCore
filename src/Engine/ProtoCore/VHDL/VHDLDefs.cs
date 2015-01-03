
namespace ProtoCore.VHDL
{
    // http://www.xilinx.com/itp/xilinx10/isehelp/ite_r_vhdl_reserved_words.htm
    public struct Keyword
    {
        public const string Abs             = "abs";            // Arithmetic operator for absolute value. Unary operator, predefined for any numeric type.  
        public const string Access          = "access";         // A variety of data type whose values are pointers to (or links to, or addresses of) dynamically-allocated objects of some other type.  
        public const string After           = "after";          // Clause used to include delay information in a signal assignment. If there is no after clause, default delay of one simulation delta is assumed.  
        public const string Alias           = "alias";          // Declares an alternate name for all or part of an existing object.  
        public const string All             = "all";            // Suffix for identifying all declarations that are contained within the package or library denoted by the prefix.  
        public const string And             = "and";            // Logical operator for types bit and Boolean and for one-dimensional arrays of these types.  
        public const string Architecture    = "architecture";   // Statement that contains description of the design.  
        public const string Assert          = "assert";         // Statement that presents a condition to be evaluated. Often used in conjunction with reporting of error messages.  
        public const string Attribute       = "attribute";      // A named characteristic of items belonging to one of the following classes: Types, subtypes Procedures, functions Signals, variables, constants Entities, architectures, configurations, packages Components Statement labels An attribute declaration declares an attribute name and its type. An attribute specification associates an attribute with a name and assigns a value to the attribute. Predefined attributes exist for types, arrays, and signals.  
        public const string Begin	        = "begin";          // Marks the beginning of the statement portion (as opposed to the declarative portion) of a process statement or architecture body.  
        public const string Block	        = "block";          // Concurrent statement used to partition a design.  
        public const string Body	        = "body";           // Conjoined with package. A package body stores the definitions of functions, procedures, and the complete constant declarations for any deferred constants that appear in a corresponding package declaration. The name of the package body is the same as that of the package declaration to which it refers.  
        public const string Buffer	        = "buffer";         // A mode that enables a port to be read and updated within the entity model. A buffer port cannot have more than one source, and can be connected only to another buffer port or to a signal with no more than one source.  
        public const string Bus	            = "bus";            // A kind of signal that represents a hardware bus. When all drivers to the signal become disconnected, the signal’s value is determined by calling the resolution function with all the drivers off. Any previous value is lost. Bus signals may be either a port or locally declared signal.  
        public const string Case	        = "case";           // A form of conditional control that selects statements for execution based on the value of a given expression.  
        public const string Component	    = "component";      // Declaration made in a top level entity to instantiate lower-level entities.  
        public const string Configuration	= "configuration";  // Associates particular component instances with specific design entities, and associates entity declarations with specific architectures.  
        public const string Constant	    = "constant";       // A class of data object. Constants can hold a single value of a given type. If the value is not specified, the constant is a deferred constant, and can appear inside a package declaration only.  
        public const string Disconnect	    = "disconnect";     // Specifies the disconnect time for a guarded signal.  
        public const string Downto	        = "downto";         // Specifies direction in a range.  
        public const string Else	        = "else";           // Optional clause in an if statement. An else clause specifies alternative statements when the if clause and any elsif clauses evaluate false.  
        public const string Elsif	        = "elsif";          // Clause in an if statement that poses an alternative condition when the if clause evaluates to false.  
        public const string End	            = "end";            // Marks the end of a statement, subprogram, or declaration of a library unit.  
        public const string Entity	        = "entity";         // Specifies input and output definitions of the design.  
        public const string Exit	        = "exit";           // Causes execution to jump out of the innermost loop or the loop whose label is specified.  
        public const string File	        = "file";           // A category of data type. File types provide a way for a VHDL design to communicate with the host environment. File type is declared with a file type definition, while files are declared with a file declaration.  
        public const string For	            = "for";            // Used to iterate a predetermined number of replications in replicated logic, such as generate and loop statements. Also used in specifying blocks, components, and configurations, and in specifying time expression in a timeout clause.  
        public const string Function	    = "function";       // A subprogram used for computing a single value. Functions are always terminated by a return statement, which returns a value. Functions are specified with a subprogram specification.  
        public const string Generate	    = "generate";       // Replicates one or more concurrent statement. Can be in for or if format.  
        public const string Generic	        = "generic";        // Passes environment information to subcomponents; can be declared in the same constructs in which ports can be declared. Generics are of the object class constant. The declaration of a generic may also include a default value, which will be used if an actual value is missing in the generic map.  
        public const string Guarded	        = "guarded";        // Option for a concurrent signal assignment. The guarded option specifies that the signal assignment statement will execute only when the guard condition of the block statement that contains the assignment is true.  
        public const string If	            = "if";             // Conditional logic statement. Presents a condition to be evaluated as true or false.  
        public const string Impure  	    = "impure";         // Option for a function in a subprogram specification. Use of this reserved word extends the scope of variables and signals declared outside of the function to be available to that function, resulting in the possibility that the function may return different values when called multiple times with the same actual parameter values.  
        public const string In	            = "in";             // Port mode that allows the port to be read only. If no mode is specified, in is assumed.  
        public const string Inertial	    = "inertial";       // An option for specifying delay mechanism in a signal assignment statement. Inertial delay is characteristic of switching circuits: a pulse whose duration is shorter than the switching time of the circuit will not be transmitted or in the case that a pulse rejection limit is specified, a pulse whose duration is shorter than that limit will not be transmitted.  
        public const string Inout	        = "inout";          // Port mode that allows a bidirectional port to be read and updated within the entity model.  
        public const string Is	            = "is";             // Reserved word that equates the identity portion to the definition portion of a declaration.  
        public const string Label	        = "label";          // An entity class, to be stated during attribute specification of user-defined attributes.  
        public const string Library	        = "library";        // A context clause that makes visible the logical names of design libraries that can be referenced within a design unit. The following library clause is implied for every design unit:library std, work 
        public const string Linkage	        = "linkage";        // A port mode similar to inout used to connect VHDL ports to non-VHDL ports.  
        public const string Literal	        = "literal";        // An entity class, to be stated during attribute specification of user-defined attributes.  
        public const string Loop	        = "loop";           // Statement used to iterate through a set of sequential statements.  
        public const string Map	            = "map";            // With port or generic, associates port names within a block (local) to names outside a block (external). A port of mode may be left unconnected either by omitting it from the port map, or by connecting it to the reserved word open. In either case, the corresponding port declaration must include a default value.  
        public const string Mod	            = "mod";            // Arithmetic operator for modulus. Modulus is predefined for any integer type; the operands and the result are of the same type. The result of a mod operator has the sign of the second operand and is defined (for some integer n) as: a mod b = a-b*n
        public const string Nand	        = "nand";           // Logical operator for types bit and Boolean and for one-dimensional arrays of these types. Complement of and.  
        public const string New	            = "new";            // An allocator that enables objects of a specific type to be created dynamically. These dynamically-created objects are accessed by access types.
        public const string Next	        = "next";           // Statement that causes the current iteration of the specified loop to be prematurely terminated, resuming execution with the next iteration of the loop.  
        public const string Nor	            = "nor";            //Logical operator for types bit and Boolean and for one-dimensional arrays of these types. Complement of or.  
        public const string Not	            = "not";            // Unary logical operator for types bit and Boolean.  
        public const string Null	        = "null";           // Sequential statement that causes no action to take place; execution continues with the next statement.  
        public const string Of	            = "of";             // Reserved word used to link an identifier to its entity name, and used when specifying type mark in a file type definition.  
        public const string On	            = "on";             // Used to introduce the sensitivity list in the sensitivity clause of a wait statement.  
        public const string Open	        = "open";           // An entity aspect, used as a binding indication to indicate that binding is not yet specified and that it is to be deferred.  
        public const string Or	            = "or";             // Logical operator for types bit and Boolean and for one-dimensional arrays of these types.  
        public const string Others	        = "others";         // When used as the last branch of case statement, used to cover all values not specified by when statements. Can also be used as part of the right-hand side of a signal or variable assignment statement for array types. This assigns values to array elements not otherwise assigned.  
        public const string Out	            = "out";            // Port mode that enables the port to be updated only. It cannot be read.  
        public const string Package	        = "package";        // Optional library unit for making shared definitions (usually type definitions). You must issue a use statement to make the package available to other parts of the design.  
        public const string Port	        = "port";           // Signals through which an entity communicates with the other models in its external environment.  
        public const string Postponed	    = "postponed";      // Option for a concurrent signal assignment or process statement.  
        public const string Procedure	    = "procedure";      // Subprogram used to partition large behavioral descriptions. Procedures can return zero or more values.  
        public const string Process	        = "process";        // A process represents a level of hierarchy in a design. The statements contained in the process_statement_partrun sequentially (from top to bottom) rather than concurrently. If the process includes the optional sensitivity_list, the process_statement_part is executed only when there is an event on one or more of the signals listed in the sensitivity_list. For simulation, the process_statement_part of all processes is executed once when the simulation initializes. Processes with sensitivity lists will not execute again until there is an event on one of the signals in the sensitivity_list. Processes without a sensitivity list will continue to re-execute their process_statement_part for the remainder of the simulation. This implies that the process_statement_part should include at least one wait statement. Otherwise, the simulation time will not advance and the simulator will appear to be frozen. For synthesis, processes may be used to infer either sequential (clocked) or combinational logic. Processes intended to infer combinational logic should include in the sensitivity_list all signals that affect the behavior of the process_statement_part. This includes not only signals appearing on the right-hand side of signal or variable assignment statements, but also signals or variables appearing as part of conditional statements such as if or case.  Processes that infer synchronous logic should include the clock signal and any asynchronous controls (asynchronous resets or presets) in the sensitivity list. In general, processes may or may not be synthesizable, depending on the details of how they are written. See the IEEE VHDL User Manual for details.
        public const string Pure	        = "pure";           // Option for a function in a subprogram specification.  A pure function will disallow the use of any signals or variables declared outside of the function.  All functions are pure unless specified as impure.  
        public const string Range	        = "range";          // Parameter used when specifying subtypes in an array type declaration.  
        public const string Record	        = "record";         // A composite data type in which the collection of values may belong to the same or different types.  
        public const string Register	    = "register";       // A kind of signal which models a latch. If all drivers to such a signal are disconnected, the signal retains its old value.  
        public const string Reject	        = "reject";         // An option for specifying delay mechanism in a signal assignment statement. Every inertially delayed signal assignment has a pulse rejection limit. If the delay mechanism specifies inertial delay, and if the reserved word reject followed by a time expression is present, then the time expression specifies the pulse rejection limit. In all other cases, the pulse rejection limit is specified by the time expression associated with the first waveform element. Not supported for synthesis.  
        public const string Rem	            = "rem";            // Arithmetic operator for remainder. Remainder is predefined for any integer type; the operands and the result are of the same type. The result of a rem operator has the sign of the first operand and is defined as: a rem b = a-(a/b)*b
        public const string Report	        = "report";         // Statement for generating report messages. Not supported for synthesis.  
        public const string Return	        = "return";         // Statement that causes a subprogram to terminate, returning control back to the calling object. All functions must have a return statement, and the value of the expression in the return statement is returned to the calling program. For procedures, objects of mode out and inout return their values to the calling program.  
        public const string Rol	            = "rol";            // Shift operator: rotate left. Shift operators are defined for any one-dimensional array type whose element type is either bit or Boolean.  The arguments of rol are the array that will be rotated and the amount by a which it will be rotated.  
        public const string Ror	            = "ror";            // Shift operator: rotate right. Shift operators are defined for any one-dimensional array type whose element type is either bit or Boolean.  The arguments of rol are the array that will be rotated and the amount by a which it will be rotated.  
        public const string Select	        = "select";         // Expression whose value determines different values for a target signal in a selected signal assignment statement.  
        public const string Severity	    = "severity";       // A predefined type in the language with values note, warning, error, and failure.
        public const string Shared	        = "shared";         // An type of variable that can be declared only in entities, architectures, and generates. A shared variable can be accessed by all three of the subprograms/processes local to the declarative region.  
        public const string Signal	        = "signal";         // Represents a wire or a placeholder for a value. Signals are assigned in signal assignment statements, and declared in signal declarations. Note that signal assignments always occur with some amount of delay. In the absence of the optional delay_mechanism, signal assignments will occur one delta delay after the signal assignment statement is executed. This fact has major implications when a signal assignment is executed as part of a block of sequential statements within a process. See the IEEE VHDL User Manual for details.  
        public const string Sla  	        = "sla";            // Shift operator: shift left arithmetic. Shift operators are defined for any one-dimensional array type whose element type is either bit or Boolean.  The arguments of sla are the array that will be shifted and the amount by a which it will be shifted.  This shift operator will fill with the leftmost bit.  
        public const string Sll	            = "sll";            // Shift operator: shift left logical. Shift operators are defined for any one-dimensional array type whose element type is either bit or Boolean.  The arguments of sll are the array that will be shifted and the amount by a which it will be shifted.  This shift operator will fill with zeros.  
        public const string Sra	            = "sra";            // Shift operator: shift right arithmetic. Shift operators are defined for any one-dimensional array type whose element type is either bit or Boolean.  The arguments of sra are the array that will be shifted and the amount by a which it will be shifted.  This shift operator will fill with the rightmost bit.  
        public const string Srl	            = "srl";            // Shift operator: shift right logical. Shift operators are defined for any one-dimensional array type whose element type is either bit or Boolean.  The arguments of srl are the array that will be shifted and the amount by a which it will be shifted.  This shift operator will fill with zeros.  
        public const string Subtype	        = "subtype";        // A declaration that defines a base type and a constraint. The constraint specifies a subset of values for the base type. An object is said to belong to a subtype if it is of the base type and if it satisfies the constraint.  
        public const string Then	        = "then";           // Introduces statements to execute when the preceding if or elsif statement evaluates true.  
        public const string To	            = "to";             // Specifies direction in a range.  
        public const string Transport	    = "transport";      // An option for specifying delay mechanism in a signal assignment statement. Transport delay is characteristic of hardware devices, such as transmission lines, that exhibit nearly infinite frequency response: any pulse is transmitted, no matter how short its duration. Not supported for synthesis.  
        public const string Type	        = "type";           // Data type. Each data type has a set of values and a set of operations associated with it. User-defined types are created with type declarations. Predefined types can be divided into several categories: scalar, composite, access, and file. In addition, there are non-predefined types established by the IEEE standard 1164. Each of these types is listed below.
         
        // Scalar Types               
        public const string Enum                = "enum "; 	
        public const string True                = "true"; 	
        public const string False               = "false"; 	
        public const string Integer             = "integer"; 	
        public const string Array               = "array";              // A composite type in which all values belong to the same data type (e.g., string is an array of the data type character).  
        public const string String              = "string";             // (1-dimensional array of type character)
        public const string Bit_vector          = "bit_vector";         // (1-dimensional array of type bit)
        public const string Std_ulogic          = "std_ulogic";         // (an enumerated type with the values ’U’, ’X’, ’0’, ’1’, ’Z’, ’W’, ’L’, ’H’, ’-’)
        public const string Std_logic           = "std_logic";          //  (same as std_ulogic except that this is a resolved type)
        public const string Std_ulogic_vector   = "std_ulogic_vector";  // (an array of std_ulogic)
        public const string Std_logic_vector    = "std_logic_vector";   // (an array of std_logic)
        public const string Unsigned            = "unsigned";           // (an array of std_logic)
        public const string Signed              = "signed";             // (an array of std_logic)
                                                


        public const string Unaffected	        = "unaffected"; // Concurrent statement that causes no action to take place; execution continues with the next statement.  
        public const string Units	            = "units";      // An entity class, to be stated during attribute specification of user-defined attributes. Also used in physical type definition statement.  
        public const string Until	            = "until";      // Part of the condition clause of a wait statement.  
        public const string Use	                = "use";        // Clause that makes the contents of a package visible from inside an entity or an architecture.  
        public const string Variable	        = "variable";   // Declared inside a process statement with a variable declaration; assigned with a variable assignment statement. Variables are created at the time of elaboration and retain their values throughout the entire simulation run. Note that variable assignments occur without delay (unlike signal assignments). This has major implications when variable assignments are used as part of a block of sequential statements within a process. See the VHDL User Manual for details.  
        public const string Wait	            = "wait";       // Suspends evaluation of a process. 
                                                                // There are three basic forms of a wait statement: 
                                                                //    wait on sensitivity_list;
                                                                //    wait until boolean_expression;
                                                                //    wait for time_expression;
                                                                // These forms can be combined:
                                                                //      wait on sensitivity_list until boolean_expression for time_expression;

        public const string When	            = "when";   // Used to present choices for conditional logic in a case statement.  
        public const string While	            = "while";  // Used to iterate replications in replicated a loop statement. Also used for conditional logic in selected waveforms.  
        public const string With	            = "with";   // Introduces the select expression of a selected signal assignment statement.  
        public const string Xnor	            = "xnor";   // Logical operator for types bit and Boolean and for one-dimensional arrays of these types. Logical exclusive nor.  
        public const string Xor	                = "xor";    // Logical operator for types bit and Boolean and for one-dimensional arrays of these types. Logical exclusive or.  
    }

    public struct Constants
    {
        public const int kInvalidIndex = -1;
        public const int SignalBitCount = 32;

        public const string kSignalAssignSymbol = "<=";
        public const string ExecutionStartFlagName = "execution_started";

        public const string ClockSync = "ClockSync";
        public const string ResetSync = "ResetSync";
        public const string RisingEdge = "rising_edge";
        public const string ClockSignalName = "clock";
        public const string ResetSignalName = "reset";
        public const string PrefixProcess = "proc";
        public const string PrefixArrayType = "t_array";
    }

    public struct ComponentName
    {
        public struct ALU
        {
            public const string Add = "ALU_Add";
            public const string Sub = "ALU_Sub";
            public const string Mul = "ALU_Mul";
            public const string Div = "ALU_Div";

            public const string OpSignal1 = "op1";
            public const string OpSignal2 = "op2";
            public const string OpSignalResult = "result";
        }
    }
}
