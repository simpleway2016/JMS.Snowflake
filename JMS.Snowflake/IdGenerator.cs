using System;
using System.Threading;

namespace JMS.Snowflake
{
    /// <summary>
    /// 雪花id生成器
    /// </summary>
    public class IdGenerator: ISnowflakeGenerator
    {
        /// <summary>
        /// 基准时间
        /// </summary>
        const long Epoch = 1416881713316L;
        /// <summary>
        /// 机器id部分的位移
        /// </summary>
        const int MachineBitOffset = 12;
        /// <summary>
        /// 时间戳部分的位移
        /// </summary>
        const int TimeBitOffset = 22;
        const long MachineMask = 1023L << MachineBitOffset;
        private readonly long _machineId;
        /// <summary>
        /// 当前序列号
        /// 序列号和时间戳应该是一对的，所以，时间戳也要存在sequence里面，否则多线程并发时，会有序列号和时间戳不是一对的问题，从而引起生成的id不是增大的
        /// </summary>
        private long _sequence = 0;

        private long _lastTimestamp = 0;
        object _lock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineId">机器id,0-1023</param>
        public IdGenerator(int machineId)
        {
            if (machineId < 0 || machineId > 1023)
                throw new ArgumentException("machineId超出范围");
            this._machineId = machineId<< MachineBitOffset;
        }

        /// <summary>
        /// 生成新的id
        /// </summary>
        /// <returns></returns>
        public long NewId()
        {
            lock (_lock)
            {
                while (true)
                {
                    var time = getTimeStamp();

                    if (time < _lastTimestamp)
                    {

                        for (int i = 0; i < 20; i++)
                        {
                            Thread.Sleep(100);
                            time = getTimeStamp();
                            if (time >= _lastTimestamp)
                                break;
                        }

                        if (time < _lastTimestamp)
                        {
                            throw new Exception($"系统时钟出现回撤，当前时间比上一次生成id的时间回撤了{_lastTimestamp - time}毫秒");
                        }
                    }

                    if (time > _lastTimestamp)
                    {
                        _sequence = 0;
                        _lastTimestamp = time;
                    }

                    _sequence++;
                    if (_sequence > 4095)
                    {
                        //当machine部分的bit不为0时，证明序列号已经超过1023
                        //序列号超过最大值1023，要等time变动到下一豪秒
                        while (getTimeStamp() == time)
                        {
                            Thread.Yield();
                        }
                        continue;
                    }

                    return _machineId | (_lastTimestamp << TimeBitOffset) | _sequence;
                }
            }
        }

        long getTimeStamp()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds() - Epoch;
        }
    }
}
