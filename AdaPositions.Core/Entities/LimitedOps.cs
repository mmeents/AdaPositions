using AdaPositions.Core.Interfaces;
using AdaPositions.Core.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {

  public enum Ot {
    UpdateRateLimits,
    GetStakeAddresses,
    GetAddressAmounts,
    GetAssetDetails
  }

  public class ToDoItem { 
    public long Id { get; set;}
    public Ot Optype { get; set;}
    public string? StrOpParam1 { get; set;}
    public AddressModel? Address { get; set; }
    public int PageNumber { get; set;} = 1;
    public ToDoItem(Ot optype) {
      Id = 0;
      Optype = optype;      
    }
  }

  public class OperationSchedule : ConcurrentDictionary<long, ToDoItem> {
    public Int64 Nonce = 1;    
    public OperationSchedule() {    
    }
    public ToDoItem AddToSchedule(Ot opType, string? strOpParam1, AddressModel? address, int pageNumber ) {
      Nonce++;
      var op = new ToDoItem(opType){
        Id = Nonce,
        StrOpParam1 = strOpParam1,
        Address = address,
        PageNumber = pageNumber
      };      
      base[Nonce] = op;
      return op;
    }
    public void Remove(Int64 aKey) {
      if (ContainsKey(aKey)) {
        base.TryRemove(aKey, out _);
      }
    }
    public ToDoItem? Pop() {
      ToDoItem? aR = null;
      if (Keys.Count > 0) {
        base.TryRemove(base.Keys.OrderBy(x => x).First(), out aR);
      }
      return aR;
    }
  }

  public class Operation {
    public long Id = 0;
    private BackgroundWorker BgWorker = new BackgroundWorker();
    public ToDoItem ToDo;
    public LimitedOps Owner { get; set; }
    public Operation(LimitedOps owner, ToDoItem toDo) {
      Owner = owner;
      ToDo = toDo;
      BgWorker.DoWork += DoWorkHandeler;
      BgWorker.RunWorkerCompleted += DoWorkCompleteHandeler;
      BgWorker.RunWorkerAsync();
    }
    private void DoWorkCompleteHandeler(object? sender, RunWorkerCompletedEventArgs e) {
      try {
        BgWorker.Dispose();
      } catch (Exception ex) {
        Owner._form1.LogMsg($"{DateTime.Now} Error0 {ex.Message}");
      }

      Owner.Remove(this.Id);
    }
    private void DoWorkHandeler(object? sender, DoWorkEventArgs e) {
      try {
        switch (ToDo.Optype) {
          case Ot.UpdateRateLimits:
            Owner.Owner.GetRateLimits();
            break;
          case Ot.GetStakeAddresses:
            if (ToDo.StrOpParam1 != null) {
              //Owner.Owner.GetAddAddressByStakeFromBlockfrost(ToDo.StrOpParam1, null, ToDo.PageNumber);            
              bool hasMore = Owner.Owner.GetAddAddressByStakeFromBlockfrost(ToDo.StrOpParam1, null, ToDo.PageNumber);
              if (hasMore) Owner.Owner.ScheduleToDo(ToDo.Optype, ToDo.StrOpParam1, null, ToDo.PageNumber + 1);            
            }
            break;  
          case Ot.GetAddressAmounts:
            if (ToDo.Address != null) {
              Owner.Owner.GetAddAddressAmountsFromBlockfrost(ToDo.Address);
            }
            break;
          case Ot.GetAssetDetails:
            if (ToDo.StrOpParam1 != null) {
              Owner.Owner.GetAssetDetailsFromBlockfrost(ToDo.StrOpParam1);
            }
            break;
        }
      } catch (Exception ex) {
        Owner._form1.LogMsg($"{DateTime.Now} DoWorkHandeler {ToDo.Optype.AsString()} {ex.Message}");
      }
    }

    public string OpLabel { get { return ToDo.Optype.AsString() + " "; } }
  }

  public class LimitedOps : ConcurrentDictionary<long, Operation> {
    public Int64 Nonce = 1;
    public AddressService Owner;
    public ILogMsg _form1;
    public OperationSchedule OpSchedule = new OperationSchedule();
    public LimitedOps(AddressService owner, ILogMsg form1) {
      Owner = owner;
      _form1 = form1;
    }
    public Operation AddOp(Operation op) {
      Nonce++;
      op.Id = Nonce;
      base[Nonce] = op;
      return op;
    }
    public void Remove(Int64 aKey) {
      if (ContainsKey(aKey)) {
        base.TryRemove(aKey, out _);
      }
    }
  }


  public static class OpExt {
    public static string AsString(this Ot op) {
      return op switch {
        Ot.UpdateRateLimits => "UpdateRateLimits",
        Ot.GetStakeAddresses => "GetStakeAddresses",
        Ot.GetAddressAmounts => "GetAddressAmounts",
        Ot.GetAssetDetails => "GetAssetDetails",
        _ => "Unknown"
      };
    }
  }

}
