using JMS.Snowflake;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace UnitTest
{
    public class UnitTest1
    {
        /// <summary>
        /// 测试多线程并发时，id会不会重复，并且确认id是不是一直增大的
        /// </summary>
        /// <exception cref="Exception"></exception>
        [Fact]
        public void IdGeneratorTest()
        {
            ConcurrentDictionary<long,bool> ids = new ConcurrentDictionary<long, bool>();
            var generator = new IdGenerator(2);
            new Thread(() => { make(ids, generator); }).Start();
            long lastid = 0;
            DateTime lasttime = DateTime.Now;
            int lastCount = 0;
            while (true)
            {
                var id = generator.NewId();
                if (ids.ContainsKey(id))
                {
                    var time = id >> 22;
                    var machineid = (id >> 12) & 1023;
                    var sequence = id & 4095;
                    var b = ids[id];
                    throw new Exception("id重复");
                }
                if (id < lastid)
                {
                    var time = id >> 22;
                    var machineid = (id >> 12) & 1023;
                    var sequence = id & 4095;

                    var time2 = lastid >> 22;
                    var machineid2 = (lastid >> 12) & 1023;
                    var sequence2 = lastid & 4095;
                    throw new Exception("id变小");
                }
         
                lastid = id;
                if(ids.TryAdd(id, true) == false)
                {
                    throw new Exception("id重复");
                }

                if ((DateTime.Now - lasttime).TotalSeconds > 3)
                {
                    var count = ids.Count;
                    Debug.WriteLine($"已生成：{count} 每秒：{Math.Round(((count - (long)lastCount)/ (DateTime.Now - lasttime).TotalMilliseconds)/10,2)}万个");
                    lasttime = DateTime.Now;
                    lastCount = count;
                    if(count > 100000000)
                    {
                        ids.Clear();
                        lastCount = 0;
                    }
                }
            }
        }

        void make(ConcurrentDictionary<long, bool> ids, IdGenerator generator)
        { 
            long lastid = 0;
            while (true)
            {
                var id = generator.NewId();
                if (ids.ContainsKey(id))
                {
                    var time = id >> 22;
                    var machineid = (id >> 12) & 1023;
                    var sequence = id & 4095;
                    var b = ids[id];
                    throw new Exception("id重复");
                }
                if (id < lastid)
                {
                    var time = id >> 22;
                    var machineid = (id >> 12) & 1023;
                    var sequence = id & 4095;

                    var time2 = lastid >> 22;
                    var machineid2 = (lastid >> 12) & 1023;
                    var sequence2 = lastid & 4095;
                    throw new Exception("id变小");
                }
                lastid = id;
                if (ids.TryAdd(id, false) == false)
                {
                    throw new Exception("id重复");
                }
            }
        }
    }
}