using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace JMS.Snowflake
{
    public interface ISnowflakeGeneratorFactory
    {
        /// <summary>
        /// 创建ISnowflakeGenerator
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <returns></returns>
        ISnowflakeGenerator Create(string name);
    }

    class DefaultSnowflakeGeneratorFactory : ISnowflakeGeneratorFactory
    {
        ConcurrentDictionary<string, ISnowflakeGenerator> _dict = new ConcurrentDictionary<string, ISnowflakeGenerator>();
        public ISnowflakeGenerator Create(string name)
        {
            return _dict.GetOrAdd(name , n=>new IdGenerator(GlobalEnvironment.MachineId));
        }
    }
}
