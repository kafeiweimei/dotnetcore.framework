using System;

namespace Models
{
    /// <summary>
    /// ����Ԥ��ģ��
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// ʱ��
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// �����¶�
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// �����¶�
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// ժҪ
        /// </summary>
        public string Summary { get; set; }
    }
}
