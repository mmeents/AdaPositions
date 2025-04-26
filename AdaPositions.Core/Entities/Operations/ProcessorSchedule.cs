using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Interfaces;

namespace AdaPositions.Core.Entities.Operations
{
    public class ProcessorSchedule : ConcurrentDictionary<long, OperationCommand>
    {
        public long Nonce = 1;
        public object Owner;
        public ILogMsg ErrorOut;
        public ProcessorSchedule(object owner, ILogMsg errorOut)
        {
            Owner = owner;
            ErrorOut = errorOut;
        }
        public OperationCommand AddToSchedule(string command, OperationParameters inputParams)
        {
            Nonce++;
            var op = new OperationCommand(command, inputParams);
            op.Id = Nonce;
            base[Nonce] = op;
            return op;
        }
        public void Remove(long aKey)
        {
            if (ContainsKey(aKey))
            {
                TryRemove(aKey, out _);
            }
        }
        public OperationCommand? Pop()
        {
            OperationCommand? aR = null;
            if (Keys.Count > 0)
            {
                TryRemove(Keys.OrderBy(x => x).First(), out aR);
            }
            return aR;
        }
    }


}
