using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace NEASL.Base;

/// <summary>
/// Basic Manager / Helper Class to order and execute Instructions in order.
/// </summary>
public class InstructionQueryManager
{
    private bool m_isRunning;
    private BlockingCollection<Instruction> InstructionQuery;
    private DateTime m_lastStartTime;
    private Instruction m_currentInstruction;
    private int m_TimeOutTime;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public InstructionQueryManager()
    {
        this.InstructionQuery = new BlockingCollection<Instruction>();
    }

    
    /// <summary>
    /// TOOD: SET A TIMEOUT WHICH CHECKS IF A INSTRUCTION HAS FAILED!
    /// </summary>
    /// <param name="timeOutTime"></param>
    public void SetTimeOutTime(int timeOutTime)
    {
        this.m_TimeOutTime = timeOutTime;
    }

    /// <summary>
    /// Stops the execution.
    /// </summary>
    public void Stop()
    {
        m_isRunning = false;
    }
    
    /// <summary>
    /// Starts the Instruction of the next Instruction in the Query.
    /// </summary>
    public void Start()
    {
        m_isRunning = true;
        m_currentInstruction = GetCurrentItem();
        if (m_currentInstruction == null)   
            Stop();
        
        if (IsRunning() && m_currentInstruction != null)
        {
            if (m_currentInstruction.IsSubSectionLeave)
            {
                lock (m_currentInstruction)
                {
                    RemoveItemFromList(m_currentInstruction);
                    Start();
                }
            }

            if (Context.GetInstance().GetEventManager().ReceiverForInstructionRegistered(m_currentInstruction))
                Context.GetInstance().GetEventManager().FireByInstruction(m_currentInstruction);
            else
            {

               // Console.WriteLine($"Current Instruction ({m_currentInstruction.BaseName}->{m_currentInstruction.MethodName}()) could not be reolved");
                Console.WriteLine("Exiting...");
                Stop();
                this.InstructionQuery = new BlockingCollection<Instruction>();
            }
        }
        else
        {
            //Console.WriteLine("Finished!");
            Stop();
        }

      
    }
    
    /// <summary>
    /// Is the Manager currently Executing? 
    /// </summary>
    /// <returns></returns>
    public bool IsRunning()
    {
        return this.m_isRunning;
    }


    // Returns the current item in order.
    private Instruction currentItem
    {
        get
        {
            if (this.InstructionQuery != null && InstructionQuery.Count > 0
                                              && this.InstructionQuery.ToList().Exists(x => x?.Completed == false))
            {
                var temp = this.InstructionQuery.Where(x => x?.Completed == false).ToList();
                var itm = temp.OrderBy(x => x?.Id)?.First();
                return itm;
            }
            return null;
        }
    }

    /// <summary>
    /// Returns the current Instruction.
    /// </summary>
    /// <returns>NULL || Instruction</returns>
    public Instruction GetCurrentItem()
    {
        return this.currentItem;
    }

    public List<Instruction> GetQuery()
    {
        return this.InstructionQuery.ToList();
    }

    /// <summary>
    /// Adds a IEnumerable collection of Instructions to the Query
    /// </summary>
    /// <param name="instructions">The IEnumerable to add</param>
    public void AddToQuery(IEnumerable<Instruction> instructions)
    {
        try
        {
            Type defaultType = typeof(Instruction);
            if (this.InstructionQuery != null && this.InstructionQuery.Count > 100)
            {
                var temp = this.InstructionQuery.Where(x => x?.Completed == false);
                lock (this.InstructionQuery)
                {
                    this.InstructionQuery = new BlockingCollection<Instruction>();
                    foreach (var item in temp)
                    {
                        this.InstructionQuery.Add(item);
                    }
                }
            }
            if (instructions != null)
            {
                lock (this.InstructionQuery)
                {
                    lock (instructions)
                    {
                        foreach (var instruction in instructions)
                        {
                            if (instruction != null && !string.IsNullOrEmpty(instruction.ObjectName))
                                this.InstructionQuery.Add(instruction);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(string.Format("ERROR WHILE ADDTOQUERY REQUEST!\n: {0}", e.Message));
        }
    }

    
    /// <summary>
    /// Adds a single Instruction to the Instruction Query.
    /// </summary>
    /// <param name="instruction">The Instruciton to add.</param>
    public void AddToQuery(Instruction instruction)
    {
        AddToQuery(new List<Instruction>() { instruction } );
    }
    
    private List<Instruction> Sort(List<Instruction> eventQueue)
    {
        try
        {
            lock (this.InstructionQuery)
            {
                var newList = this.InstructionQuery.Where(x => !x.Completed).ToList();
                newList = newList.OrderBy(x => x?.Id).ToList();
                return newList;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(string.Format("[EXCEPTION] Exception in GameEventQuery Occured! : {0}", e.Message));
            return null;
        }
    }

    /// <summary>
    /// Returns if the given Instruction exists in the Query.
    /// </summary>
    /// <param name="theEvent">The Instruction to search for.</param>
    /// <returns>FALSE || TRUE</returns>
    public bool ItemExists(Instruction theEvent)
    {
        if (this.InstructionQuery == null)
            return false;

        return this.InstructionQuery.ToList().Contains(theEvent);
    }

    // Make sure to take the result in account to either return / set values or to
    // behave on conditions.
    public void SendCompleted(string sourceName, string MethodName, object[] args, object result = null)
    {
        if (string.IsNullOrEmpty(sourceName) || string.IsNullOrEmpty(MethodName))
            return;
        
        if (m_currentInstruction == null)
        {
            return;
        }
        
        lock (m_currentInstruction)
        {
            if (m_currentInstruction != null && m_currentInstruction.ObjectName.Trim() == sourceName.Trim() &&
                m_currentInstruction.MethodName.Trim() == MethodName.Trim())
            {
                if (m_currentInstruction.Arguments != null && args == null 
                    || args != null && m_currentInstruction.Arguments == null
                    ||args != null && m_currentInstruction.Arguments != null && args.Length != m_currentInstruction.Arguments.Length)
                        Console.WriteLine("SendCompleted -> [INVALID INSTRUCTION REQUEST! Arguments do not match!]");
                
                else if (args == null && m_currentInstruction.Arguments == null
                         || args != null && m_currentInstruction.Arguments != null &&
                         args.Length == m_currentInstruction.Arguments.Length)
                {
                    // TODO: create seperate method! Also include an "ELSE" check which is being executed once the last checked condition was false and a ELSE exists.
                    // Check if the current returned result was part of a condition
                    // (ex. IF(): ELSE etc. to make sure to consider to go into the SubEntry or to ignore it)
                    if (m_currentInstruction.IsCondition 
                        || m_currentInstruction.IsLoop
                        || m_currentInstruction.IsSubSectionEntry
                        || result != null)
                    {
                        if (m_currentInstruction.IsCondition && result != null && result.GetType() == typeof(bool))
                        {
                            if (((bool)result))
                            {
                                
                            }
                            else
                            {
                                var exitPoint = m_currentInstruction.EntryLeavePoint;
                                var itemsToSkip = this.InstructionQuery.ToList().Where(x=>x.Id > m_currentInstruction.Id && x.Id < exitPoint.Id).ToList();
                                int ExitPointIndex = this.InstructionQuery.ToList().IndexOf(exitPoint);
                                if (itemsToSkip  != null && itemsToSkip.Any() && itemsToSkip.Count >= 0)
                                {
                                    for (int i = 0; i < itemsToSkip.Count; i++)
                                    {
                                        RemoveItemFromList(itemsToSkip[i]);
                                    }
                                    RemoveItemFromList(exitPoint);
                                }
                            }
                        }
                      
                    }

                    RemoveItemFromList(m_currentInstruction);
                    Start();
                }
            }
        }
    }
    
    
    
    /// <summary>
    /// Removes a current Instruction from the list by setting its state to Completed.
    /// </summary>
    /// <param name="theEvent">The Instruction inside the Query that should be "removed"</param>
    /// <returns>FALSE || TRUE</returns>
    public bool RemoveItemFromList(Instruction theEvent)
    {
        try
        {
            lock (theEvent)
            {
                theEvent.Completed = true;
                lock (this.InstructionQuery)
                {
                    var index = this.InstructionQuery.ToList().IndexOf(theEvent);
                    if (index < 0)
                        return false;
                
                    this.InstructionQuery.ToList()[index] = theEvent;
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(
                string.Format("[EXCEPTION] Exception in GameEventQuery (RemoveItemFromList) Occured! : {0}",
                    e.Message));
            return false;
        }
    }
}