using System;
using System.Collections.Generic;
using System.Text;

namespace JMS.Snowflake
{
    public interface ISnowflakeGenerator
    {
        /// <summary>
        /// 生成新的雪花id
        /// </summary>
        /// <returns></returns>
        long NewId();
    }
    public interface ISnowflakeGenerator<T>: ISnowflakeGenerator
    {
    }

    class DefaultSnowflakeGenerator<T> : ISnowflakeGenerator<T>
    {
        IdGenerator _generator;
        public DefaultSnowflakeGenerator() {
            _generator = new IdGenerator(GlobalEnvironment.MachineId);
        }

        public long NewId()
        {
            return _generator.NewId();
        }
    }
}
