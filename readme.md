# NEASL - Not Even A Scripting Language

Welcome to **NEASL** (Not Even A Scripting Language)! This is just a small side project I started for fun. The idea is to create a very basic DSL (Domain Specific Language) that lets you control your C# code with simple scripts.


--- 
## Example:

```plaintext
BUTTON:
    NAME="Button1"
    PRESSED:
        APP->WriteLine(Hello World!)
    :PRESSED
    HOVER:
        BACKGROUND_COLOR(Green)
    :HOVER
    LEAVE:
        APP->SHOW_DIALOG(Title,Content)
    :LEAVE
:BUTTON
```

You can link this to your code, and then control things like button events via these simple scripts using the attributes and classes provided from the NEASL-Framework:

### Example C# Class:
```csharp
using System;
using NEASL.Base.Linking;

namespace NEASL.Base;

[Component(nameof(BUTTON))]
public class BUTTON : BaseLinkedObject
{
    [Signature(nameof(PRESSED), LinkType.Event)]
    public void PRESSED()
    {
        this.PerformScriptEvent(nameof(PRESSED));
    }

    [Signature(nameof(HOVER), LinkType.Event)]
    public void HOVER()
    {
        this.PerformScriptEvent(nameof(HOVER));
    }
    
    [Signature(nameof(LEAVE), LinkType.Event)]
    public void LEAVE()
    {
        this.PerformScriptEvent(nameof(LEAVE));
    }

}
```

A GUI (TEST) Implementation can be found in the NEASL.TEST_GUI running a Avalonia Framework based crossplatform sample application.

### Structure and Types.

As man attributes for classes "_**Main**_", "**_Section_**" and "**_Component_**" can be used. Main is literally the root of the context. Section a context like a page, a component then a control or something that lives in the context of the section.

As a base structure, to differentiate between different objects given the same Name / Id
its important to have some sort of folder / naming structure to prevent them from colliding.

Current thought is therefore to put "APP" as some sort of root folder and then for each Page
its own subfolder:

### Structure (still considering.....):

    APP
        |-> APP.neasl                   //  ->  Main (App) Root / Index Info File.
        |-> /PAGE (Section)             //  ->  Whatever Type of a Section (as name).
            |-> /PAGE1                  //  ->  Whatever given id-name of the Section
                |-> PAGE1.neasl         //  ->  Script file itself.
                |-> /BUTTON             //  ->  Whatever Type name of a component 
                    |-> BUTTON1.neasl   //  ->  Script file itself.
                    |-> BUTTON2.neasl   //  ->  Script file itself.
            |-> /PAGE2
                |-> PAGE2.neasl
                |-> /BUTTON
                    |-> BUTTON1.neasl
                    |-> BUTTON2.neasl
---
### Note

This project is just something I’m developing out of curiosity. It’s not meant to be taken too seriously—just a fun experiment and side project.
Therfore its not planned out and structured quite well...

---

# Feature Implementions
| Feature                    | Description                                                                                               | Implemented |
|----------------------------|-----------------------------------------------------------------------------------------------------------|---------|
| **Variables**              | Allow storing and manipulating data, with simple data types like strings, integers, floats, and booleans. |   🟢     |
| **Control Structures**     | Basic conditional and looping structures.                                                                 |    🔴  |
| - If/Else                  | Branching logic based on conditions.                                                                      |   🔴    |
| - Loops                    | For, while, and do-while loops for repeated execution.                                                    |   🔴    |
| - Switch/Case              | Alternate form of branching based on value matching.                                                      |   🔴    |
| **Functions**              | Reusable blocks of code that can take inputs (parameters) and optionally return a value.                  |   🟡   |
| **Input/Output (I/O)**     | Handling of standard input and output.                                                                    |   🔴    |
| - Standard input           | Reading user input                                                                                        |   🔴    |
| - Standard output          | Writing output to the console                                                                             |   🟢    |
| **Operators**              | Support for arithmetic, comparison, and logical operators.                                                |   🔴    |
| - Arithmetic Operators     | `+`, `-`, `*`, `/`, `%` for mathematical operations.                                                      |  🔴     |
| - Comparison Operators     | `==`, `!=`, `<`, `>`, `<=`, `>=` for comparisons.                                                         |  🔴    |
| - Logical Operators        | `AND`, `OR` , `NOT` for logical expressions.                                                              |  🔴   |
| (Optional) **Comments**    | Lines of code ignored by the interpreter, used to add explanations (`#`, `//`, `/* */`).                  |  🔴    |
| **File Handling**          | Reading from and writing to files (e.g., `open()` in Python or `file` in bash).                           |   🔴    |

### State
| Missing                 | Partially | Implemented |
|-------------------------|-----------|----------|
|  🔴  | 🟡          |    🟢   |

