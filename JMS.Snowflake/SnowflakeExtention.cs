using JMS.Snowflake;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SnowflakeExtention
    {
        /// <summary>
        /// 依赖注入ISnowflakeGenerator接口
        /// </summary>
        /// <param name="services"></param>
        /// <param name="machineId">机器id，范围：0-1023</param>
        /// <returns></returns>
        public static IServiceCollection AddSnowflakeGenerator(this IServiceCollection services, int machineId)
        {
            if (machineId < 0 || machineId > 1023)
                throw new ArgumentException("machineId超出范围（0-1023）");

            GlobalEnvironment.MachineId = machineId;
            services.AddSingleton(typeof(ISnowflakeGenerator<>), typeof(DefaultSnowflakeGenerator<>));
            services.AddSingleton<ISnowflakeGeneratorFactory,DefaultSnowflakeGeneratorFactory>();
            return services;
        }
    }
}
