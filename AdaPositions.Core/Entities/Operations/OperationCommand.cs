using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities.Operations
{
    public class OperationCommand : Item {
    public string Command { get; set; }
    public OperationParameters Input { get; set; }
    public OperationParameters Output { get; set; } = new OperationParameters();
    public OperationCommand(string command, OperationParameters inputParams) : base() {
      Command = command;
      Input = inputParams;
    }
  }

}
