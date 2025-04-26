using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Interfaces {
  public interface ILogMsg {
    void LogMsg(string msg);
    void DrawStats(string msg);
  }

}
