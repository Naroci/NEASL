using System;
using System.Collections.Generic;
using System.IO;
using NEASL.Base.Object;

namespace NEASL.Base;

public class InstructionRunner
{
    private static InstructionRunner m_instructionRunnerl = null;
    InstructionReader reader = new InstructionReader();
            
    /// <summary>
    /// Returns the current Singleton instance for the Instruction runner.
    /// </summary>
    /// <returns></returns>
    public static InstructionRunner GetInstance()
    {
        if (m_instructionRunnerl == null)
            m_instructionRunnerl = new InstructionRunner();

        return m_instructionRunnerl;
    }
    
    /// <summary>
    /// Reads the whole InstructionSection (EX SECTION: .... :SECTION)
    /// Creates a List of Instrcutions and adds them to the QueryManager.
    /// </summary>
    /// <param name="sender">Context of the current Sender / Requesting INEASL_Object.</param>
    /// <param name="instructionSection">clear text String of the section text.</param>
    public async void Execute(INEASL_Object sender, string instructionSection)
    {
        if (string.IsNullOrEmpty(instructionSection))
            return;
        
       var instructions = GetInstructions(sender,instructionSection);
       if (instructions.Count < 1)
       {
           Console.WriteLine("nothing to do.");
           return;
       }
       
       Context.GetInstance().GetQueryManager().AddToQuery(instructions);
       
       if (!Context.GetInstance().GetQueryManager().IsRunning())
           Context.GetInstance().GetQueryManager().Start();
    }

    /// <summary>
    /// Returns a list of Instructions for a given Instructions Section.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="instructionSection"></param>
    /// <returns></returns>
    /// <exception cref="ArithmeticException"></exception>
    private List<Instruction> GetInstructions(INEASL_Object sender,string instructionSection)
    {
        List<Instruction> instructions = new List<Instruction>();
        string line = string.Empty;
        StringReader rs = new StringReader(instructionSection);
        
        List<Instruction> EntryPoints = new List<Instruction>();
        List<Instruction> ExitPoints = new List<Instruction>();
        
        while ((line = rs.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            
            Instruction instruction = reader.getInstruction(sender,line);
            if (instruction != null)
            {
                if (instruction.IsSubSectionEntry)
                    EntryPoints.Add(instruction);
                else if (instruction.IsSubSectionLeave)
                    ExitPoints.Add(instruction);
                
                instructions.Add(instruction);
            }
        }
        
        // Set each Entry / Exit points for the Sub Sections (for example for IF(): ... :IF, subssections
        // so that when they are executed the InstructionQueryManager knows which elements to skip if
        // a statement is false.
        if (EntryPoints.Count > 0 || ExitPoints.Count() > 0) // Match the amount of entry and leave points to make sure that each entry has an exit.
        {
            if (EntryPoints.Count != ExitPoints.Count())
                throw new ArithmeticException("Not all sections are closed!");

            // Align the indexes counterwise to find the matching start / exit points.
            for (int i = 0; i <= EntryPoints.Count - 1; i++) 
            {
                int exitPointIndex = ExitPoints.Count -1 - i;
                EntryPoints[i].EntryLeavePoint = ExitPoints[exitPointIndex];
                ExitPoints[exitPointIndex].EntryLeavePoint = EntryPoints[i];
            }
        }

        return instructions;
    }
    
    
    
    
}