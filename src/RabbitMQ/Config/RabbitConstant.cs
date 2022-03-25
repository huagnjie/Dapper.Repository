namespace RabbitMQJie.Config
{
    public static class RabbitConstant
    {
        public static string DELAY_EXCHANGE => "pay.delay.exchange";
        public static string DELAY_ROUTING_KEY => "pay.delay.routing.key";
        public static string DELAY_QUEUE => "delay.queue";
        public static string DEAD_LETTER_EXCHANGE => "dead.letter.exchange";
        public static string DEAD_LETTER_QUEUE => "dead.letter.queue";
        public static string DEAD_LETTER_ROUTING_KEY => "dead.letter.routing.key";
        public static string TEST_EXCHANGE => "test.exchange";
        public static string TEST_QUEUE => "test.queue";
    }
}
