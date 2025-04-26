using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Interfaces;

namespace AdaPositions.Test.Entities {
  public class LogMsg : ILogMsg {
    private List<string> _logMsgs = new List<string>();
    public LogMsg() {}

    public void DrawStats(string msg) {
      throw new NotImplementedException();
    }

    void ILogMsg.LogMsg(string msg) {
      _logMsgs.Add(msg);
    }
  }
}
