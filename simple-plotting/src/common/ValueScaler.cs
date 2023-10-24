namespace simple_plotting.src {
    // TODO: Add documentation

    public static class ValueScaler {
        public static void ScaleByRef (ref double? value, double percentScale) {
            if (value == null)
                return;

            if (percentScale < -100d || percentScale > 100d)
                return;

            var scalar = percentScale / 100d;
            value*= scalar;
        }
    }
}
