using CoreService;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using Global.Enumerable;
using LiteDB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreServiceTests {
    [TestFixture]
    public class LiteDBStoreTests {
        [SetUp] public void Setup() {
            DTOBsonConversion.Setup();
        }

        [Test] public void StoreRetrieveLapTimes() {
            var lapTimeDic = new Dictionary<int, ParticipantLapTime> {
                {
                    0, new ParticipantLapTime(1, 2, 3, 4)
                },
                {
                    1, new ParticipantLapTime(0.123, 0.456, 0.789, 1)
                }
            };
            var lapTimes = new ParticipantLapTimesDTO(Key.Create(), 0, lapTimeDic);
            var db = new LiteDatabase("test.db");
            var store = new CollectionStore<ParticipantLapTimesDTO>(db);
            store.DeleteAll();
            store.Store(lapTimes);
            var result = store.LoadAll().First();
            Assert.AreEqual(result.Id, lapTimes.Id);
            Assert.AreEqual(result.participantIndex, lapTimes.participantIndex);
            CollectionAssert.AreEqual(result.lapTimes.ToArray(), lapTimes.lapTimes.ToArray());
        }
    }
}
