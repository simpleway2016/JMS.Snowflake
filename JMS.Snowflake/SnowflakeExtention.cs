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
        /// <param name="machineId">机器id</param>
        /// <returns></returns>
        public static IServiceCollection AddSnowflakeGenerator(this IServiceCollection services, int machineId)
        {
            GlobalEnvironment.MachineId = machineId;
            services.AddSingleton(typeof(ISnowflakeGenerator<>), typeof(DefaultSnowflakeGenerator<>));
            services.AddSingleton<ISnowflakeGeneratorFactory,DefaultSnowflakeGeneratorFactory>();
            return services;
        }
    }
}
