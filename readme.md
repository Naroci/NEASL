# NEASL - Not Even A Scripting Language

**NEASL** (Not Even A Scripting Language)! is just a small side project I started just for fun. The idea is to create a very basic toy language that lets you control your C# code with simple scripts.


--- 
### Example
Here is a sample script for a button that changes its color when hovering over it with a mouse cursor, writes something out to the terminal when pressed and displays a dialog message when the mouse cursor leaves it: 
```plaintext
BUTTON:
    NAME=Button1
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
A sample implementation of defining and linking an object to the system.BaseLinkedObject inherited classes automatically register themselves to the context once they got initialized and a script assigned to them. 
```csharp
using System;
using NEASL.Base.Linking;

namespace NEASL.Base;

[Component(nameof(BUTTON))]
public class BUTTON : BaseLinkedObject
{
    public BUTTON(string scriptContent) : base(scriptContent)
    {
    }
    
    public BUTTON() : base()
    {
    }

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
Using the script content and parse it into the constructor:
```csharp
string path = Environment.CurrentDirectory;
string script = File.ReadAllText(System.IO.Path.Combine(path,  @"script/PAGE/BUTTON/BUTTON.neasl"));
BUTTON btn = new BUTTON(script);
```

Or load it via the _**AssignScript**_ method:
```csharp
string path = Environment.CurrentDirectory;
string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";
BUTTON btn = new BUTTON();
btn.AssignScript(path, fileName);
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
### State
| Missing                 | Partially | Implemented |
|-------------------------|-----------|----------|
|  🔴  | 🟡          |    🟢   |
---
| Feature                 | Description                                                                              | Implemented |
|-------------------------|------------------------------------------------------------------------------------------|-------------|
| **Variables**           | Allow storing and manipulating data (NO TYPE SYSTEM YET! Everything is STRING)           | 🟡          |
| **Base Types**          | String, Bool, Int, Arrays                                                                | 🔴          |
| **Control Structures**  | Basic conditional and looping structures.                                                | 🔴          |
| If/Else                 | Branching logic based on conditions.                                                     | 🟡          |
| Loops                   | UNTIL and WHILE for repeated execution.                                                  | 🔴          |
| **Functions**           | Reusable blocks of code that can take inputs (parameters).                               | 🟢          |
| **Return Variables**    | Methods that can return variables as a value.                                            | 🔴          |
| **Input/Output (I/O)**  | Handling of standard input and output.                                                   | 🔴          |
| Read                    | Reading user input                                                                       | 🔴          |
| Write                   | Writing output to the console                                                            | 🟢          |
| **Operators**           | Support for arithmetic, comparison, and logical operators.                               | 🔴          |
| Arithmetic              | `+`, `-`, `*`, `/` for mathematical operations.                                          | 🔴          |
| Comparison              | `=`, `!=`, `<`, `>` for comparisons.                                                     | 🟡          |
| Logical Operators       | `AND`, `OR` for logical expressions.                                                     | 🔴          |
| **(Optional)** Comments | Lines of code ignored by the interpreter, used to add explanations (`#`, `//`, `/* */`). | 🔴          |
| **File (I/O) Handling** | Reading from and writing to files.                                                       | 🔴          |
| Read                    | Reading from files.                                                                      | 🔴          |
| Write                   | Writing to files.                                                                        | 🔴          |



