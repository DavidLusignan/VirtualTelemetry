using Global.Enumerable;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace CoreService.Storage.DTOs {
    public interface IStorable {
        Key Id { get; }
    }
}
