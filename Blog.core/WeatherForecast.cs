namespace Blog.core
{
    public class WeatherForecast
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public int TemperatureC { get; set; }
        /// <summary>
        /// 华氏
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        /// <summary>
        /// 总结
        /// </summary>
        public string? Summary { get; set; }
    }
}
