using AdaPositions.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities.Operations
{
    public class OperationProcessor : ConcurrentDictionary<long, Operation>
    {
        private long Nonce = 1;
        public ILogMsg ErrorOut;
        public object Owner { get; set; }

        public OperationProcessor(object owner, ILogMsg errorOut) : base()
        {
            Owner = owner;
            ErrorOut = errorOut;
        }

        public Operation AddOperation(OperationCommand command)
        {
            Nonce++;   
            Operation operation = new Operation(this, Nonce, command);                        
            base[Nonce] = operation;
            return operation;
        }

        public void Remove(long aKey)
        {
            if (ContainsKey(aKey))
            {
                TryRemove(aKey, out _);
            }
        }

    }


}
