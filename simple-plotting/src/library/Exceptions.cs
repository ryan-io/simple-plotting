namespace simple_plotting {
	public class NoBitmapParserException : Exception {
		public override string Message => simple_plotting.Message.EXCEPTION_NO_BITMAP_PARSER;
	}
	
	public class BitMapParserDisposedException : Exception {
		public override string Message => simple_plotting.Message.EXCEPTION_BITMAP_PARSER_DISPOSED;
	}
}