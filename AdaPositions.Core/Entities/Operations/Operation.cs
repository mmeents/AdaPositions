using AdaPositions.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities.Operations
{

  public interface IOperation {
    OperationProcessor Owner { get; set; }
    OperationCommand Command { get; set; }
    public Func<OperationCommand, Task>? DoWorkHandler { get; set; }

  }

  public class Operation : Item, IOperation
  {
    public OperationProcessor Owner { get; set; }
    public OperationCommand Command { get; set; }
    public Func<OperationCommand, Task>? DoWorkHandler { get; set; }

    public Operation(OperationProcessor owner, long id, OperationCommand command) : base(id) {
      Owner = owner;
      Command = command;
      // Start the asynchronous operation without blocking the constructor
      _ = ExecuteAsync();
    }

    private async Task ExecuteAsync() {
      try {
        if (DoWorkHandler != null) {
          await DoWorkHandler(Command);
        }
      } catch (Exception ex) {
        Owner.ErrorOut.LogMsg($"{DateTime.Now} Id:{Id} {Command.Command} Error: {ex.Message}");
      } finally {
        Owner.Remove(Id);
      }
    }
    
  }




}
